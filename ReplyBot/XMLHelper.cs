using System;
using System.IO;
using System.Xml.Linq;

namespace ReplyBot
{
	public class XMLHelper
	{
		public XElement xml {get;set;}
		private string path;

		public XMLHelper (string path)
		{
			xml = Load (path);
			this.path = path;
			//Console.WriteLine (xml);
		}

		public XElement Load(string path)
		{
			if(File.Exists(path))
				return XElement.Load (path);
			//TODO: Implement option when file does not exist.
			return null;
		}

		public void Save(string path=null)
		{
			if (path == null) {
				xml.Save (this.path);
			}
			xml.Save (path);
		}
	}
}

