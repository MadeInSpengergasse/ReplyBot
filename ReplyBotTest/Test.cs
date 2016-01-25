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

		string xmlpath = Environment.GetFolderPath (Environment.SpecialFolder.UserProfile) + "/.replybot/test.xml";

		[Test]
		public void NullTest() {
			new XMLHelper(null, null);
		}

		[Test]
		public void LoadTest(){
			if (File.Exists (xmlpath)) {
				File.Delete (xmlpath);
			}
			File.Copy ("../../test.xml", xmlpath);
			new XMLHelper ("test.xml", null);
			File.Delete (xmlpath);
			Assert.Pass ();
		}

		[Test]
		public void SaveTest(){
			new XMLHelper ("test.xml", null).Save();
			bool fileExists = File.Exists (xmlpath);
			File.Delete (xmlpath);
			Assert.AreEqual (fileExists, true);
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
			Assert.Throws<NullReferenceException> (() => TwitterHelper.SendTweet (null, "Blabla", 123));

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
			Assert.Throws<NullReferenceException> (()=> TwitterHelper.GetUserIdFromUsername(null, "jgarigjjgaojgrljfjrigjbjoirjajkfbjioutreotrtobjbsnödjfle"));

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

