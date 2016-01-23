using NUnit.Framework;
using System;
using ReplyBot;

namespace MonoTests.ReplyBot
{
	[TestFixture]
	public class XMLHelperTest {
		
		[Test]
		public void MainTest() {
			Assert.Throws<Exception>(()=>(new XMLHelper(null, null)));
		}

		[Test]
		public void LoadTest(){
			XMLHelper x = new XMLHelper ("test", "test");
			x.Load ();
			Assert.True ();
		}


		[Test]
		public void SaveTest(){
			XMLHelper x = new XMLHelper ("test", "test");
			x.Save ();
			Assert.True ();
		}

	}
	[TestFixture]
	public class TwitterHelperTest {

		[Test]
		public void SendTweetTestNull(){
			Assert.Throws<Exception> (() => (TwitterHelper.SendTweet (null, null, null)));
		}

		[Test]
		public void GetUserTimelineNull(){
			Assert.Throws<Exception> (() => (TwitterHelper.GetUserTimeline (null, null, null, null)));
		}

		[Test]
		public void GetUserIdFromUsernameNull(){
			Assert.Throws<Exception> (() => (TwitterHelper.GetUserIdFromUsername (null, null)));
		}


	}
	[TestFixture]
	public class ReplyBotTest {

		[Test]
		public void MainTest (){
			ReplyBot r = new ReplyBot ();
			Assert.True;
		}
			
	}
	
}