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
			
		public ReplyBot ()
		{
			service = new TwitterService (ConsumerKey, ConsumerSecret);
			service.AuthenticateWith (AccessToken, AccessTokenSecret);
		}

		public void Execute ()
		{
			foreach(var user in userList.List)
			{
				Console.WriteLine (user.UserId + " - " + user.Name);
				var tweets = TwitterHelper.GetUserTimeline (service, user.UserId, false, true);
				foreach (var tweet in tweets) {
					Console.WriteLine (textLists.getRandomString (TextLists.TextCategory.random));
					if (!tweetList.List.Contains (tweet.Id.ToString())) {
						string tweetText = "@" + tweet.User.ScreenName + " " + textLists.getRandomString (TextLists.TextCategory.random) + " #ReplyBot (" + DateTime.Now.Ticks + ")";
						if (!Debug) {
							Console.WriteLine ("Sending tweet...");
							TwitterHelper.SendTweet (service, tweetText, tweet.Id);
							tweetList.List.Add (tweet.Id.ToString());
						} else {
							Console.WriteLine ("Not sending tweet because in debug mode.");
							Console.WriteLine (tweetText);
						}
					} else {
						Console.WriteLine ("Tweet already sent.");
					}
				}
			}
			tweetList.Save ();
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
			userList.Save ();
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
			//TODO: Add/move administration option and text add option
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
