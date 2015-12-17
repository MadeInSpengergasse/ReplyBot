using System;

namespace ReplyBot
{
	public class User
	{
		public User()
		{
			UserId = 0;
			Type = 0;
			Name = "NoName";
		}

		public User (long userid, byte type, string name)
		{
			UserId = userid;
			Type = type;
			Name = name;
		}

		public long UserId {
			get;
			set;
		}

		public byte Type {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}
	}
}