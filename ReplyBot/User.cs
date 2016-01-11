using System;

namespace ReplyBot
{
	public class User
	{
		public User (long userid, TextLists.TextCategory category, string name)
		{
			UserId = userid;
			Category = category;
			Name = name;
		}

		public long UserId {
			get;
			set;
		}

		public TextLists.TextCategory Category {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}
	}
}