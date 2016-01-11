using System;
using System.Linq;
using System.Xml.Linq;
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
 				select new User (Convert.ToInt64 (x.Element ("userid").Value), ((TextLists.TextCategory)Convert.ToByte (x.Element ("category").Value)), x.Element("name").Value);

			foreach (User u in getUsers) {
				List.Add (u);
			}
		}

		public override void Save()
		{
			XElement new_xml = new XElement ("users");
			foreach (User user in List) {
				var userxml = new XElement ("user",
					new XElement ("userid", user.UserId),
					new XElement("category", (int)user.Category),
					new XElement("name", user.Name)
				);
				new_xml.Add (userxml);
			}
			xmlhelper.xml = new_xml;
			xmlhelper.Save ();
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
			XElement new_xml = new XElement ("tweets");
			foreach (string tweet in List) {
				new_xml.Add (new XElement ("tweetid", tweet));
			}
			xmlhelper.xml = new_xml;
			xmlhelper.Save ();
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
			XElement new_xml = new XElement ("texts");

			// HATE LIST
			new_xml.Add (GetXmlFromListForCategories("1", "hate", Hate));
			// NEUTRAL LIST
			new_xml.Add (GetXmlFromListForCategories("2", "neutral", Neutral));
			// NICE LIST
			new_xml.Add (GetXmlFromListForCategories("3", "nice", Nice));

			// GENERAL
			xmlhelper.xml = new_xml;
			xmlhelper.Save ();
		}

		public XElement GetXmlFromListForCategories(string categoryId, string categoryDescription, List<string> listToGetTextsFrom) {
			var newxml = new XElement("category",
				new XAttribute("id", categoryId),
				new XAttribute("description", categoryDescription)
			);
			foreach (string text in listToGetTextsFrom) {
				newxml.Add (new XElement ("text", text));
			}
			return newxml;
		}

		public List<string> GetTexts (TextCategory category){
			
			List<string> texts= new List<string>();
			var getTexts = from x in xmlhelper.xml.Elements("category")
					where x.Attribute("id").Value == ((int)category).ToString()
				select  x.Elements("text").ToArray();

			foreach (XElement s in getTexts.First()) {
				texts.Add (s.Value);
			}
			return texts;
		}

		public string getRandomString(TextCategory category) {
			switch (category) {

			case TextCategory.hate:
				return Hate [new Random ().Next (0, Hate.Count)];
			case TextCategory.neutral:
				return Neutral [new Random ().Next (0, Neutral.Count)];
			case TextCategory.nice:
				return Nice [new Random ().Next (0, Nice.Count)];
			case TextCategory.random:
				return getRandomString ((TextCategory)new Random ().Next (0, 4));
			}
			
			return "Internal error.";
		}

		public enum TextCategory : int {random=0, hate=1, neutral=2, nice=3};
	}
}