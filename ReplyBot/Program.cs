using System;
using System.Net;
using System.IO;
using System.Configuration;
using TweetSharp;
using System.Xml;

namespace ReplyBot
{
	class MainClass
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

		static readonly string nameToSpam = "stollengrollen";

		public static void Main (string[] args)
		{
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
