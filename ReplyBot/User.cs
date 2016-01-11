using System;

namespace ReplyBot
{
	public class User
	{
		public User()
		{
			UserId = 0;
			Category = 0;
			Name = "NoName";
		}

		public User (long userid, byte category, string name)
		{
			UserId = userid;
			Category = category;
			Name = name;
		}

		public long UserId {
			get;
			set;
		}

		public byte Category {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}
	}
}