using System;
using System.IO;
using System.Xml.Linq;

namespace ReplyBot
{
	public class XMLHelper
	{
		public XElement xml {get;set;}
		private string path;
		private string datapath;
		private string resname;

		public XMLHelper (string filename, string resname)
		{
			this.resname = resname;

			this.datapath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.replybot"; // home directory (~) + special hidden directory
			Directory.CreateDirectory (datapath);
			this.path = datapath+"/"+filename;
			this.xml = Load ();
		}

		public XElement Load()
		{
			if (File.Exists (path)) {
				return XElement.Load (path);
			} else {
				XElement xml = XElement.Load (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (resname));
				Save (xml);
				Console.WriteLine("XML file doesnt exist!");
				return xml;
			}
		}

		public void Save(XElement customxml=null)
		{
			if (customxml != null) {
				customxml.Save (path);
				Console.WriteLine ("aa");
			} else {
				xml.Save (this.path);
			}
		}
	}
}

