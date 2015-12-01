using System;
using System.Net;
using System.IO;
using System.Configuration;
using TweetSharp;

namespace ReplyBot
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//Console.WriteLine ("Hello World!");
			//WebRequest request = WebRequest.Create("http://www.contoso.com/");
			//WebResponse response = request.GetResponse ();
			//string text = new StreamReader (response.GetResponseStream ()).ReadToEnd ();
			//Console.WriteLine (text);

			var service = new TwitterService(ConsumerKey, ConsumerSecret);
			service.AuthenticateWith(AccessToken, AccessTokenSecret);

			var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
			foreach (var tweet in tweets)
			{
				Console.WriteLine("{0} says '{1}'", tweet.User.ScreenName, tweet.Text);
			}

			//var target = service.ListTweetsOnUserTimeline (new ListTweetsOnUserTimelineOptions());
			var options = new ListTweetsOnUserTimelineOptions();
			options.ScreenName="z3ntu";
			options.IncludeRts = false;
			var target = service.ListTweetsOnUserTimeline(options);
			foreach (var tweet in target)
			{
				Console.WriteLine("{0} says '{1}'", tweet.User.ScreenName, tweet.Text);
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
