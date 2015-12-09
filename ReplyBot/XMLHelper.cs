using System;
using System.IO;
using System.Xml.Linq;

namespace ReplyBot
{
	public class XMLHelper
	{
		public XElement xml {get;set;}
		private string filename;

		public XMLHelper (string filename, string resname)
		{
			xml = Load (filename, resname);
			this.filename = filename;
			//Console.WriteLine (xml);
		}

		public XElement Load(string filename, string resname)
		{
			if (File.Exists (filename))
				return XElement.Load (filename);
			else {
				return XElement.Load (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (resname));
			}
		}

		public void Save(string filename=null)
		{
			if (filename == null) {
				xml.Save (this.filename);
			}
			xml.Save (filename);
		}
	}
}

