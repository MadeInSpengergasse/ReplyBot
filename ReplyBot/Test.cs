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

	}
}