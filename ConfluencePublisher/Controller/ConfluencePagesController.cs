using MarkdownSharp;
using System;
using System.IO;
using ConfluencePublisher.Service;
using ConfluencePublisher.Exceptions;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using ConfluencePublisher.Model;
using System.Threading.Tasks;

namespace ConfluencePublisher.Controller
{
    class ConfluencePagesController
    {
        private static Markdown markdown = new Markdown(true);

        public static void publishPageList(string solutionPath, dynamic siteConfig, dynamic pages, string parentTitle = null) {
            
            foreach (dynamic pageDefinition in pages)
            {
                dynamic publishedPage = publishOrUpdatePage(solutionPath, siteConfig, pageDefinition, parentTitle);

                if (pageDefinition.attachments != null)
                {

                    foreach (dynamic attachment in pageDefinition.attachments) 
                    {
                        string attachmentPath = Path.Combine(solutionPath, Constraints.SITE_SUBDIRECTORY, Convert.ToString(attachment.filePath));

                        if (!File.Exists(attachmentPath))
                        {
                            throw new AttachmentNotFoundException($"Attachment {attachmentPath} required but was not found");
                        }
                        dynamic publishedAttachment = ConfluenceApiCaller.createAttachment(siteConfig, Convert.ToString(publishedPage.results[0].id), attachment, attachmentPath);
                    }
                }

                if (pageDefinition.chieldPages != null)
                {
                    publishPageList(solutionPath, siteConfig, pageDefinition.chieldPages, Convert.ToString(pageDefinition.title));
                }

            }
        }

        private static dynamic publishOrUpdatePage(string solutionPath, dynamic siteConfig, dynamic pageDefinition, string parentTitle) {
            string markdownPath = Path.Combine(solutionPath, Constraints.SITE_SUBDIRECTORY, Convert.ToString(pageDefinition.markdownPath));

            string convertedBody = markdown.Transform(File.ReadAllText(markdownPath));

            if (String.IsNullOrEmpty(parentTitle))
            {
                parentTitle = siteConfig.parentTitle;
            }
            dynamic publishedPage = ConfluenceApiCaller.getContent(siteConfig, Convert.ToString(pageDefinition.title));
            dynamic parentContent = ConfluenceApiCaller.getContent(siteConfig, parentTitle);

            if (((JArray)parentContent.results).Count() == 0)
            {
                throw new InvalidParentPageException($"Parent page {parentTitle} declared on site.json file must be created and was not found.");
            }

            parentContent = parentContent.results[0];

            if (((JArray)publishedPage.results).Count() == 0)
            {
                publishedPage = ConfluenceApiCaller.createContent(siteConfig, pageDefinition, Convert.ToInt32(parentContent.id), convertedBody);
            }
            else
            {
                DateTime lastModified = File.GetLastWriteTimeUtc(markdownPath);
                string version = publishedPage.results[0].version.when;
                DateTime lastPublished = DateTime.ParseExact(version, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                if (lastPublished < lastModified)
                {
                    publishedPage = ConfluenceApiCaller.updateContent(siteConfig, publishedPage.results[0], parentContent.id, convertedBody);
                    Console.WriteLine($"[SUCCESS] Page {publishedPage.results[0].title} was deployed successfully!");
                }
                else
                {
                    Console.WriteLine($"[INFO] Skipping page {publishedPage.results[0].title} deploy because it's already up to date!");
                }
            }
            return publishedPage;
        }
    }
}
