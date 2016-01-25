using NUnit.Framework;
using System;
using System.IO;
using ReplyBot;
using System.Xml.Linq;
using System.Xml;

namespace ReplyBotTest
{

	[TestFixture]
	public class XMLHelperTest {

		[Test]
		public void MainTest() {
			Assert.Throws<Exception>(()=>(new XMLHelper(null, null)));
		}

		[Test]
		public void LoadTest(){
			new XMLHelper ("test", null);
			Assert.Pass ();
		}


		[Test]
		public void SaveTest(){
			var xmlpath = Environment.GetFolderPath (Environment.SpecialFolder.UserProfile) + "/.replybot/test.xml";
			XMLHelper x = new XMLHelper ("test", null);
			x.Save ();
			Assert.AreEqual (File.Exists (xmlpath), true);
			File.Delete (xmlpath);
		}

	}
	[TestFixture]
	public class TwitterHelperTest {

		[Test]
		public void SendTweetTestNull(){
			Assert.Throws<NullReferenceException> (() => (TwitterHelper.SendTweet (null, null, 0)));
		}

		[Test]
		public void SendTweetTestInvalidID(){
			Assert.Throws<Exception> (() => TwitterHelper.SendTweet (TweetSharp.TwitterService (), "Blabla", 123));

		}

		[Test]
		public void GetUserTimelineNull(){
			Assert.Throws<NullReferenceException> (() => (TwitterHelper.GetUserTimeline (null, 0, false, false)));
		}


		[Test]
		public void GetUserIdFromUsernameNull(){
			Assert.Throws<NullReferenceException> (() => (TwitterHelper.GetUserIdFromUsername (null, null)));
		}

		[Test]
		public void GetUserIdFromUsernameInvalidName(){
			Assert.Throws<NullReferenceException> (()=> TwitterHelper.GetUserIdFromUsername(TweetSharp.TwitterService(), "jgarigjjgaojgrljfjrigjbjoirjajkfbjioutreotrtobjbsnödjfle"));

		}



	}
	[TestFixture]
	public class ReplyBotTest {

		[Test]
		public void MainTest (){
			new ReplyBot.ReplyBot ();
			Assert.Pass ();
		}

	}
}

