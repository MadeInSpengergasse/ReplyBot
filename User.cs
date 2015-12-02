using System;

namespace Application
{
	public class User
	{
		string name;

		public User ()
		{
			Name = "Test";
		}

		public User (string name)
		{
			Name = name;
		}

		public string Name {
			get { return name; }
			set { this.name = name; }
		}
	}
}