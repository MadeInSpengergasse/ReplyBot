using System;
using System.IO;
using System.Xml.Linq;

namespace ReplyBot
{
	public class XMLHelper
	{
		public XElement xml {get;set;}
		private string path;

		public XMLHelper (string path, string resname)
		{
			xml = Load (path, resname);
			this.path = path;
			//Console.WriteLine (xml);
		}

		public XElement Load(string path, string resname)
		{
			if (File.Exists (path))
				return XElement.Load (path);
			else {
				String dflt = new StreamReader (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (resname)).ReadToEnd ();
				Console.WriteLine (dflt);
			}
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

