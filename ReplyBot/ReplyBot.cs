﻿using System;
using System.Net;
using System.IO;
using System.Configuration;
using TweetSharp;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace ReplyBot
{
	class ReplyBot
	{
		XMLHelper userDB = new XMLHelper ("users.xml", "default_users");
		XMLHelper tweetDB = new XMLHelper ("tweets.xml", "default_tweets");
		XMLHelper textsDB = new XMLHelper ("texts.xml", "default_texts");

		TwitterService service;

		static readonly string[] texts = {
			"You suck!! #dislike",
			"I don't like you.",
			"+1 you are cool!",
			"EPIC FAIL!!!!",
			"I kek'd a little.",
			"I approve of this.",
			"Do you really mean this?"
		}; //TODO: Read texts from textsDB

		static readonly string nameToSpam = "stollengrollen"; //TODO: Read users from userDB

		public ReplyBot ()
		{
			service = new TwitterService (ConsumerKey, ConsumerSecret);
			service.AuthenticateWith (AccessToken, AccessTokenSecret);
		}

		public void Execute ()
		{
			List<User> users = new List<User> ();
			List<string> answeredTweets = new List<string> ();
			List<string> hateTexts = GetTexts(1);
			List<string> neutralTexts = GetTexts(2);
			List<string> niceTexts = GetTexts(3);

			var getUsers = from x in userDB.xml.Elements ("user")
			        select new User () { UserId = Convert.ToInt64 (x.Element ("userid").Value), Type = Convert.ToByte (x.Element ("type").Value) };

			foreach (User u in getUsers) {
				users.Add (u);
			}

			var getTweets = from x in tweetDB.xml.Elements("tweetid")
				select x.Value;

			foreach (String s in getTweets){
				answeredTweets.Add (s);
			}


				
			var tweets = TwitterHelper.GetUserTimeline (service, nameToSpam, false, true);
			foreach (var tweet in tweets) {
				int rand = new Random ().Next (0, texts.Length);
				if (!Debug) {
					Console.WriteLine ("Sending tweet");
					TwitterHelper.SendTweet (service, "@" + tweet.User.ScreenName + " " + texts [rand] + " #ReplyBot (" + DateTime.Now.Ticks + ")", tweet.Id);
				} else {
					Console.WriteLine ("Not sending tweet because in debug mode.");
					Console.WriteLine (answeredTweets.FirstOrDefault().ToString());
					Console.WriteLine (users.FirstOrDefault ().UserId);
					Console.WriteLine (hateTexts.FirstOrDefault().ToString ());
				}
				//Console.WriteLine("{0} says '{1}' - ID:'{2}'", tweet.User.Name, tweet.Text, tweet.Id);
			}
		}

		public void AddToDatabase ()
		{
			//TODO: Add "add to database" code here
			Console.WriteLine("Please enter the @handle of the user you want to add without the @.");
			string username = Console.ReadLine ();
			TwitterUser user = TwitterHelper.GetUserIdFromUsername (service, username);
			Console.WriteLine (user);
			Console.WriteLine ("Not implemented yet.");
		}

		public void DeleteFromDatabase ()
		{
			//TODO: Add "delete from database" code here
			Console.WriteLine ("Not implemented yet.");
		}


		public static void Main (string[] args)
		{
			while (true) {
				Console.WriteLine ("Welcome to ReplyBot, your very own Twitter bot!");
				Console.WriteLine ("What would you like to do?");
				Console.WriteLine ("'E' to execute the bot.");
				Console.WriteLine ("'A' to add a user to the recipients database.");
				Console.WriteLine ("'D' to delete a user from the database.");
				Console.WriteLine ("'Q' to quit.");
				ReplyBot replybot = new ReplyBot ();

				char pressedkey = Char.ToUpper (Console.ReadKey ().KeyChar);
				Console.WriteLine ("");
				switch (pressedkey) {
				case 'E':
					replybot.Execute ();
					break;
				case 'A':
					replybot.AddToDatabase ();
					break;
				case 'D':
					replybot.DeleteFromDatabase ();
					break;
				case 'Q':
					Console.WriteLine ("Bye!");
					Environment.Exit (0);
					break;
				default:
					Console.WriteLine ("Key not recognized! Please try again!");
					break;
				}
				Console.WriteLine ("\n");
			}
		}

		private static string ConsumerKey {
			get { return ConfigurationManager.AppSettings ["ConsumerKey"]; }
		}

		private static string ConsumerSecret {
			get { return ConfigurationManager.AppSettings ["ConsumerSecret"]; }
		}

		private static string AccessToken {
			get { return ConfigurationManager.AppSettings ["AccessToken"]; }
		}

		private static string AccessTokenSecret {
			get { return ConfigurationManager.AppSettings ["AccessTokenSecret"]; }
		}

		private static bool Debug {
			get { return bool.Parse(ConfigurationManager.AppSettings ["Debug"]); }
		}

		public List<string> GetTexts (int category){
			List<string> texts= new List<string>();
			var getTexts = from x in textsDB.xml.Elements("category")
					where Convert.ToInt32(x.Attribute("id").Value) ==category
				select x.Element ("text").Value;

			foreach (String s in getTexts) {
				texts.Add (s);
			}
			return texts;
		}
		//string[] u= users.
		//Console.WriteLine ("Hello World!");
		//WebRequest request = WebRequest.Create("http://www.contoso.com/");
		//WebResponse response = request.GetResponse ();
		//string text = new StreamReader (response.GetResponseStream ()).ReadToEnd ();
		//Console.WriteLine (text);

		//var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
		//foreach (var tweet in tweets)
		//{
		//	Console.WriteLine("{0} says '{1}'", tweet.User.ScreenName, tweet.Text);
		//}
	}

}
