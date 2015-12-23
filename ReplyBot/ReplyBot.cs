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

		public void AddUserToDatabase ()
		{
			
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

		public void ViewUserDatabase()
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

		public void DeleteUserFromDatabase ()
		{
			//TODO: Add "delete from database" code here
			Console.WriteLine ("Not implemented yet.");
		}

		public void AddTextToDatabase()
		{
			Console.WriteLine ("Please enter your text now.");
			string text = Console.ReadLine ();

			Console.WriteLine (
				"Please choose the category:" + "\n" +
				"'1' for a hate message." + "\n" +
				"'2' for a neutral message." + "\n" +
				"'3' for a nice message."
			);
			char pressedkey = Char.ToUpper (Console.ReadKey ().KeyChar);
			byte mode;
			if (Byte.TryParse (pressedkey.ToString(), out mode) == false) {
				Console.WriteLine ("Not a number, please try again!");
				return;
			}
			switch (mode) {
			case 1:
				textLists.Hate.Add (text);
				break;
			case 2:
				textLists.Neutral.Add (text);
				break;
			case 3:
				textLists.Nice.Add (text);
				break;
			default:
				Console.WriteLine ("Unknown mode, please try again.");
				return;
			}
			Console.WriteLine ("Text added.");
			textLists.Save ();
		}

		public void ViewTextDatabase()
		{
			Console.WriteLine ("Texts in Database:");
			if (textLists.Hate.Count == 0 || textLists.Neutral.Count == 0 || textLists.Nice.Count == 0) {
				Console.WriteLine ("Database is empty. Try adding a text first!");
				return;
			}
			Console.WriteLine ("\nCategory HATE:");
			foreach (string text in textLists.Hate) {
				Console.WriteLine("'" + text + "'");
			}
			Console.WriteLine ("\nCategory NEUTRAL:");
			foreach (string text in textLists.Neutral) {
				Console.WriteLine("'" + text + "'");
			}
			Console.WriteLine ("\nCategory NICE:");
			foreach (string text in textLists.Nice) {
				Console.WriteLine("'" + text + "'");
			}
		}

		public void DeleteTextFromDatabase()
		{
			//TODO: Implement
			Console.WriteLine ("Please choose a category from which you want to delete a text");

			Console.WriteLine (
								"'1' for hate messages." + "\n" +
				"'2' for neutral messages." + "\n" +
				"'3' for nice messages."
			);
			char pressedkey = Char.ToUpper (Console.ReadKey ().KeyChar);
			byte mode;
			if (Byte.TryParse (pressedkey.ToString(), out mode) == false) {
				Console.WriteLine ("Not a number, please try again!");
				return;
			}
			switch (mode) {
			case 1: 
				break;
			case 2:
				break;
			case 3:
				break;
			}

			Console.WriteLine ("Not implemented yet.");
		}


		public static void Main (string[] args)
		{
			ReplyBot replybot = new ReplyBot ();
			//TODO: Add/move administration option and text add option
			/*
			 * E execute
			 * A administration
			 * 	- u User
			 *    - a Add
			 *    - d Delete
			 *    - v View
			 *  - t Texts
			 *    - a Add
			 *    - d Delete
			 *    - v View
			 *  Q quit
			 */
			while (true) {
				Console.WriteLine (
					"Welcome to ReplyBot, your very own Twitter bot!" + "\n" +
					"What would you like to do?" + "\n" +
					"'E' to execute the bot." + "\n" +
					"'A' to administrate." + "\n" +
					"'Q' to quit."
				);
				Console.Write ("> ");

				char pressedkey = Char.ToUpper (Console.ReadKey ().KeyChar);
				Console.WriteLine ("");
				switch (pressedkey) {
				case 'E':
					replybot.Execute ();
					break;
				case 'A':
					AdministrationDialog (replybot);
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

		public static void AdministrationDialog(ReplyBot replybot) {
			while (true) {
				Console.WriteLine (
					"-- ADMINISTRATION --" + "\n" +
					"'U' for user administration." + "\n" +
					"'T' for text administration." + "\n" +
					"'B' to get back to the main menu."
				);
				Console.Write ("> ");

				char pressedkey = Char.ToUpper (Console.ReadKey ().KeyChar);
				Console.WriteLine ("");
				switch (pressedkey) {
				case 'U':
					AdministrationUserDialog (replybot);
					break;
				case 'T':
					AdministrationTextDialog (replybot);
					break;
				case 'B':
					return;
				default:
					Console.WriteLine ("Key not recognized! Please try again!");
					break;
				}
				Console.WriteLine ("\n");
			}
		}

		public static void AdministrationUserDialog(ReplyBot replybot) {
			while (true) {
				Console.WriteLine (
					"--- USER ADMINISTRATION ---" + "\n" +
					"What would you like to do?" + "\n" +
					"'A' to add a user to the recipients database." + "\n" +
					"'V' to view all users in the database." + "\n" +
					"'D' to delete a user from the database." + "\n" +
					"'B' to go back to the main administration menu."
				);
				Console.Write ("> ");

				char pressedkey = Char.ToUpper (Console.ReadKey ().KeyChar);
				Console.WriteLine ("");
				switch (pressedkey) {
				case 'A':
					replybot.AddUserToDatabase ();
					break;
				case 'V':
					replybot.ViewUserDatabase ();
					break;
				case 'D':
					replybot.DeleteUserFromDatabase ();
					break;
				case 'B':
					return;
				default:
					Console.WriteLine ("Key not recognized! Please try again!");
					break;
				}
				Console.WriteLine ("\n");
			}
		}

		public static void AdministrationTextDialog(ReplyBot replybot) {
			Console.WriteLine ("Not implemented.");
			while (true) {
				Console.WriteLine (
					"--- TEXT ADMINISTRATION ---" + "\n" +
					"What would you like to do?" + "\n" +
					"'A' to add a text to the database." + "\n" +
					"'V' to view all texts in the database." + "\n" +
					"'D' to delete a text from the database." + "\n" +
					"'B' to go back to the main administration menu."
				);
				Console.Write ("> ");

				char pressedkey = Char.ToUpper (Console.ReadKey ().KeyChar);
				Console.WriteLine ("");
				switch (pressedkey) {
				case 'A':
					replybot.AddTextToDatabase ();
					break;
				case 'V':
					replybot.ViewTextDatabase ();
					break;
				case 'D':
					replybot.DeleteTextFromDatabase ();
					break;
				case 'B':
					return;
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
