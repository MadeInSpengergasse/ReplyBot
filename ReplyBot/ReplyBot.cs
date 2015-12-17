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
		UserList userList = new UserList(new XMLHelper ("users.xml", "default_users"));
		TweetList tweetList = new TweetList(new XMLHelper ("tweets.xml", "default_tweets"));
		TextLists textLists = new TextLists(new XMLHelper ("texts.xml", "default_texts"));

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
			var tweets = TwitterHelper.GetUserTimeline (service, nameToSpam, false, true);
			foreach (var tweet in tweets) {
				int rand = new Random ().Next (0, texts.Length);
				if (!Debug) {
					Console.WriteLine ("Sending tweet");
					TwitterHelper.SendTweet (service, "@" + tweet.User.ScreenName + " " + texts [rand] + " #ReplyBot (" + DateTime.Now.Ticks + ")", tweet.Id);
				} else {
					Console.WriteLine ("Not sending tweet because in debug mode.");
					Console.WriteLine (tweetList.List.FirstOrDefault().ToString());
					Console.WriteLine (userList.List.FirstOrDefault ().UserId);
					Console.WriteLine (textLists.Hate.FirstOrDefault().ToString ());
				}
				//Console.WriteLine("{0} says '{1}' - ID:'{2}'", tweet.User.Name, tweet.Text, tweet.Id);
			}
		}

		public void AddToDatabase ()
		{
			//TODO: Add "add to database" code here
			Console.WriteLine("Please enter the handle of the user you want to add.");
			Console.Write ("@");
			string username = Console.ReadLine ();
			TwitterUser user = TwitterHelper.GetUserIdFromUsername (service, username);
			if (user == null) {
				Console.WriteLine ("Unknown user! Please retry!");
				return;
			}
			Console.WriteLine ("What type of messages do you want to send to this user?");
			Console.WriteLine ("'0' for random messages.");
			Console.WriteLine ("'1' for hate messages.");
			Console.WriteLine ("'2' for neutral messages.");
			Console.WriteLine ("'3' for nice messages.");
			Console.Write ("> ");

			char pressedkey = Char.ToUpper (Console.ReadKey ().KeyChar);
			byte mode;
			if (Byte.TryParse (pressedkey.ToString(), out mode) == false) {
				Console.WriteLine ("Not a number, please try again!");
				return;
			}
			Console.WriteLine ("");
			List<byte> validModes = new List<byte>{ 0, 1, 2, 3 };
			if (validModes.IndexOf (mode) == -1) {
				Console.WriteLine ("Unknown mode, please try again!");
				return;
			}
			userList.List.Add(new User(user.Id, mode, user.ScreenName));
			Console.WriteLine ("Added user '" + user.ScreenName + "' with ID '" + user.Id + "' to the database!");
		}

		public void ViewDatabase()
		{
			Console.WriteLine ("Users in Database:");
			if (userList.List.Count == 0) {
				Console.WriteLine ("Database is empty. Try adding a user first!");
				return;
			}
			foreach (User user in userList.List) {
				Console.WriteLine("'" + user.Name + "' with ID '" + user.UserId + "'");
			}
		}

		public void DeleteFromDatabase ()
		{
			//TODO: Add "delete from database" code here
			Console.WriteLine ("Not implemented yet.");
		}


		public static void Main (string[] args)
		{
			ReplyBot replybot = new ReplyBot ();
			while (true) {
				Console.WriteLine ("Welcome to ReplyBot, your very own Twitter bot!");
				Console.WriteLine ("What would you like to do?");
				Console.WriteLine ("'E' to execute the bot.");
				Console.WriteLine ("'A' to add a user to the recipients database.");
				Console.WriteLine ("'V' to view all users in the database.");
				Console.WriteLine ("'D' to delete a user from the database.");
				Console.WriteLine ("'Q' to quit.");
				Console.Write ("> ");

				char pressedkey = Char.ToUpper (Console.ReadKey ().KeyChar);
				Console.WriteLine ("");
				switch (pressedkey) {
				case 'E':
					replybot.Execute ();
					break;
				case 'A':
					replybot.AddToDatabase ();
					break;
				case 'V':
					replybot.ViewDatabase ();
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
