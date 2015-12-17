using System;
using System.Linq;
using System.Collections.Generic;

namespace ReplyBot
{
	public abstract class ListHelper
	{
		protected XMLHelper xmlhelper;
		public ListHelper (XMLHelper xmlhelper)
		{
			this.xmlhelper = xmlhelper;
		}
		public abstract void Save ();
	}

	public class UserList : ListHelper
	{
		public List<User> List { get; set;}

		public UserList (XMLHelper xmlhelper) : base(xmlhelper)
		{
			List = new List<User> ();

			var getUsers = from x in xmlhelper.xml.Elements ("user")
				select new User () { UserId = Convert.ToInt64 (x.Element ("userid").Value), Type = Convert.ToByte (x.Element ("type").Value), Name = x.Element("name").Value };

			foreach (User u in getUsers) {
				List.Add (u);
			}
		}

		public override void Save()
		{
			//TODO: Implement
		}
	}

	public class TweetList : ListHelper
	{
		public List<string> List { get; set;}

		public TweetList(XMLHelper xmlhelper) : base(xmlhelper)
		{
			List = new List<string> ();

			var getTweets = from x in xmlhelper.xml.Elements("tweetid")
				select x.Value;

			foreach (String s in getTweets){
				List.Add (s);
			}
		}

		public override void Save()
		{
			//TODO: Implement
		}
	}

	public class TextLists : ListHelper
	{
		public List<string> Hate { get; set;}
		public List<string> Neutral { get; set;}
		public List<string> Nice { get; set; }

		public TextLists(XMLHelper xmlhelper) : base(xmlhelper)
		{
			Hate = GetTexts (TextCategory.hate);
			Neutral = GetTexts (TextCategory.neutral);
			Nice = GetTexts (TextCategory.nice);
		}

		public override void Save()
		{
			//TODO: Implement
		}

		public List<string> GetTexts (TextCategory category){
			List<string> texts= new List<string>();
			var getTexts = from x in xmlhelper.xml.Elements("category")
					where Convert.ToInt32(x.Attribute("id").Value) == (int)category
				select x.Element ("text").Value;

			foreach (String s in getTexts) {
				texts.Add (s);
			}
			return texts;
		}

		public string getRandomString(TextCategory category) {
			//TODO: Implement
			return "Random string, not implemented";
		}

		public enum TextCategory : int {random=0, hate=1, neutral=2, nice=3};
	}
}

