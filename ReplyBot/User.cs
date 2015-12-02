using System;

namespace ReplyBot
{
	public class User
	{
		long userid;
		byte type;

		public User ()
		{
			userid = 12233;
			type = 0;
		}

		public User (long userid, byte type)
		{
			UserId = userid;
			Type = type;
		}

		public long UserId {
			get { return userid; }
			set { this.userid = value; }
		}

		public byte Type {
			get { return type; }
			set { this.type = value; }
		}
	}
}