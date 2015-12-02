using System;
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
		XMLHelper userDB = new XMLHelper ("users.xml");
		XMLHelper tweetDB = new XMLHelper ("tweets.xml");

		TwitterService service;

		bool DEBUG = true;

		static readonly string[] texts = {
			"You suck!! #dislike",
			"I don't like you.",
			"+1 you are cool!",
			"EPIC FAIL!!!!",
			"I kek'd a little.",
			"I approve of this.",
			"Do you really mean this?"
		};

		static readonly string nameToSpam = "stollengrollen";

					
		public ReplyBot ()
		{
			service = new TwitterService (ConsumerKey, ConsumerSecret);
			service.AuthenticateWith (AccessToken, AccessTokenSecret);	
		}

		public void Execute ()
		{
			List<User> users = new List<User> ();

			var y = from x in userDB.xml.Elements ("user")
			        select new User () { UserId = Convert.ToInt64 (x.Element ("userid").Value), Type = Convert.ToByte (x.Element ("type").Value) };
			
			foreach (User u in y) {
				users.Add (u);
			}

			var tweets = TwitterHelper.GetUserTimeline (service, nameToSpam, false, true);
			foreach (var tweet in tweets) {
				int rand = new Random ().Next (0, texts.Length);
				if (!DEBUG) {
					Console.WriteLine ("Sending tweet");
					TwitterHelper.SendTweet (service, "@" + tweet.User.ScreenName + " " + texts [rand] + " #ReplyBot (" + DateTime.Now.Ticks + ")", tweet.Id);
				} else {
					Console.WriteLine ("Not sending tweet because in debug mode.");
				}
				//Console.WriteLine("{0} says '{1}' - ID:'{2}'", tweet.User.Name, tweet.Text, tweet.Id);
			}
		}

		public void AddToDatabase ()
		{
			//TODO: Add "add to database" code here
		}

		public void DeleteFromDatabase ()
		{
			//TODO: Add "delete from database" code here
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
				switch (Char.ToUpper (Console.ReadKey ().KeyChar)) {
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
					Console.WriteLine ("\nBye!");
					Environment.Exit (0);
					break;
				default:
					Console.WriteLine ("\nKey not recognized! Please try again!");
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
