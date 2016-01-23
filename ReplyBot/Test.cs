using NUnit.Framework;
using System;

namespace ReplyBot
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
			Assert.Pass ();
		}


		[Test]
		public void SaveTest(){
			XMLHelper x = new XMLHelper ("test", "test");
			x.Save ();
			Assert.Pass ();
		}

	}
	[TestFixture]
	public class TwitterHelperTest {

		[Test]
		public void SendTweetTestNull(){
			Assert.Throws<Exception> (() => (TwitterHelper.SendTweet (null, null, 0)));
		}

		[Test]
		public void GetUserTimelineNull(){
			Assert.Throws<Exception> (() => (TwitterHelper.GetUserTimeline (null, 0, false, false)));
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
			Assert.Pass ();
		}
			
	}
	
}