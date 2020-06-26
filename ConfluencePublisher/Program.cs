using ConfluencePublisher.Util;
using System;
using ConfluencePublisher.Controller;
using System.IO;
using NuGet.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ConfluencePublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string solutionPath = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();

                PathValidator.validate(solutionPath);

                string configJsonPath = Path.Combine(solutionPath, "site.json");
                dynamic siteConfig = SiteDefinitionReader.getSiteDefinition(configJsonPath);

                ConfluencePagesController.publishPageList(solutionPath, siteConfig, siteConfig.pages);
            }
            catch(Exception e) 
            {
                Console.Error.WriteLine($"{e} : {e.ToString()}");
                Environment.ExitCode = 1;
            }
        }
    }
}
