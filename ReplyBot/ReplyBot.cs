using System;
using System.Net;
using System.IO;
using System.Configuration;
using TweetSharp;
using System.Xml;

namespace ReplyBot
{
	class ReplyBot
	{
		XMLHelper userDB = new XMLHelper("users.xml");
		XMLHelper tweetDB = new XMLHelper("tweets.xml");



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

		public ReplyBot(){
			
			XmlElement usersxml= userDB.xml;
			XmlElement tweetsxml= tweetDB.xml;

			//string[] u= users.
			//Console.WriteLine ("Hello World!");
			//WebRequest request = WebRequest.Create("http://www.contoso.com/");
			//WebResponse response = request.GetResponse ();
			//string text = new StreamReader (response.GetResponseStream ()).ReadToEnd ();
			//Console.WriteLine (text);

			TwitterService service = new TwitterService(ConsumerKey, ConsumerSecret);
			service.AuthenticateWith(AccessToken, AccessTokenSecret);

			//var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
			//foreach (var tweet in tweets)
			//{
			//	Console.WriteLine("{0} says '{1}'", tweet.User.ScreenName, tweet.Text);
			//}

			var tweets = TwitterHelper.GetUserTimeline(service, nameToSpam, false, true);
			foreach (var tweet in tweets)
			{
				int rand = new Random().Next(0, texts.Length);
				Console.WriteLine ("============= SENDING TWEET ==============");
				if (!DEBUG) {
					TwitterHelper.SendTweet (service, "@" + tweet.User.ScreenName + " " + texts [rand] + " #ReplyBot (" + DateTime.Now.Ticks + ")", tweet.Id);
				}
				//Console.WriteLine("{0} says '{1}' - ID:'{2}'", tweet.User.Name, tweet.Text, tweet.Id);
			}
		
		}

		public void Execute()
		{
			//TODO: Add most from constructor here!
		}

		public void AddToDatabase()
		{
			//TODO: Add "add to database" code here
		}

		public void DeleteFromDatabase()
		{
			//TODO: Add "delete from database" code here
		}

		static readonly string nameToSpam = "stollengrollen";

		public static void Main (string[] args)
		{
			while (true) {
				Console.WriteLine ("Welcome to ReplyBot, your very own Twitter bot!");
				Console.WriteLine ("What would you like to do?");
				Console.WriteLine ("'E' to execute the bot.");
				Console.WriteLine ("'A' to add a user to the recipients database.");
				Console.WriteLine ("'D' to delete a user from the database.");
				Console.WriteLine ("'E' to exit.");
				//ReplyBot replybot = new ReplyBot ();
				switch (Console.ReadKey ().KeyChar) {
				case 'E':
				//replybot.Execute();
					break;
				case 'A':
				//replybot.AddToDatabase ();
					break;
				case 'D':
				//replybot.DeleteFromDatabase ();
					break;
				case 'E':
					Console.WriteLine ("Bye!");
					Environment.Exit (0);
					break;
				default:
					Console.WriteLine ("Key not recognized! Please try again!");
					break;
				}
			}
		}
					
		private static string ConsumerKey
		{
			get { return ConfigurationManager.AppSettings["ConsumerKey"]; }
		}
		private static string ConsumerSecret
		{
			get { return ConfigurationManager.AppSettings["ConsumerSecret"]; }
		}
		private static string AccessToken
		{
			get { return ConfigurationManager.AppSettings["AccessToken"]; }
		}
		private static string AccessTokenSecret
		{
			get { return ConfigurationManager.AppSettings["AccessTokenSecret"]; }
		}
	}
}
