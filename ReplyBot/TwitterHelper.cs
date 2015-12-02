using System;
using TweetSharp;
using System.Collections.Generic;

namespace ReplyBot
{
	public class TwitterHelper
	{
		public static bool SendTweet(TwitterService service, string status, int inReplyToStatusId)
		{
			var sendoptions = new SendTweetOptions ();
			sendoptions.Status = status;
			sendoptions.InReplyToStatusId = inReplyToStatusId;
			var response = service.SendTweet (sendoptions);
			if (response == null) {
				Console.WriteLine ("ERROR SENDING TWEET! Possible duplicate!");
				return false;
			}
			return true;
		}

		public static IEnumerable<TwitterStatus> GetUserTimeline(TwitterService service, string screenname, bool includeRts, bool excludeReplies)
		{
			ListTweetsOnUserTimelineOptions options = new ListTweetsOnUserTimelineOptions();
			options.ScreenName = screenname;
			options.IncludeRts = includeRts;
			options.ExcludeReplies = excludeReplies;
			return service.ListTweetsOnUserTimeline(options);
		}
	}
}

