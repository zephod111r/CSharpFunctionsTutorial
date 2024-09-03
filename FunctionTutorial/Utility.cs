using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionTutorial
{
    internal class Utility
    {

        static internal string GetDynamicContent(string pathToFile, string extension)
        {
            string currentPath = Directory.GetCurrentDirectory();

            //TODO: Fix this!
            string path = Path.Combine(currentPath, "../../../resources", pathToFile);

            string dynamicContent = File.ReadAllText(path);

            return dynamicContent;
        }

        static internal string GetContentType(string extension)
        {
            switch (extension.ToLower())
            {
                case ".html":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return "application/javascript";
                case ".json":
                    return "application/json";
                default:
                    return "text/plain";
            }
        }
    }
}
