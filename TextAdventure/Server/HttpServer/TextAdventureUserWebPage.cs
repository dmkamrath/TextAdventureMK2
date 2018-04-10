using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextAdventure.Server.HttpServer
{
    class TextAdventureUserWebPage
    {
        private static string filePath = "TextAdventurePageSimple.html";
        public string webPage;

        public TextAdventureUserWebPage()
        {
            webPage = htmlFileToString();
        }

        private static string htmlFileToString()
        {
            StreamReader sr = new StreamReader(filePath);
            FileStream outStream = new FileStream("OutPage.html",FileMode.Create);
            string pageString = sr.ReadToEnd();
            
            outStream.Write(Encoding.UTF8.GetBytes(pageString), 0, pageString.Length);
            return pageString;
        }
    }
}
