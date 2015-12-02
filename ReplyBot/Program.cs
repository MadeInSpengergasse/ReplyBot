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

			var service = new TwitterService(ConsumerKey, ConsumerSecret);
			service.AuthenticateWith(AccessToken, AccessTokenSecret);

			var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
			foreach (var tweet in tweets)
			{
				Console.WriteLine("{0} says '{1}'", tweet.User.ScreenName, tweet.Text);
			}

			//var target = service.ListTweetsOnUserTimeline (new ListTweetsOnUserTimelineOptions());
			var options = new ListTweetsOnUserTimelineOptions();
			options.ScreenName = nameToSpam;
			options.IncludeRts = false;
			options.ExcludeReplies = true;
			var target = service.ListTweetsOnUserTimeline(options);
			foreach (var tweet in target)
			{
				int rand = new Random().Next(0, texts.Length);
				Console.WriteLine ("============= SENDING TWEET ==============");
				var sendoptions = new SendTweetOptions ();
				sendoptions.Status = "@" + tweet.User.ScreenName + " " + texts [rand] + " #ReplyBot (" + DateTime.Now.Ticks + ")"; //DateTime is used in order to prevent identical tweets
				sendoptions.InReplyToStatusId = tweet.Id;
				var status = service.SendTweet (sendoptions);
				if (status == null) {
				Console.WriteLine ("ERROR SENDING TWEET! Possible duplicate!");
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

		public string[] loadJson (string filename){
			StreamReader r = new StreamRead(filename);
	}
}