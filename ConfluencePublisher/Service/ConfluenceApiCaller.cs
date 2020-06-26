using MarkdownSharp;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConfluencePublisher.Exceptions;
using System.Net.Http.Headers;

namespace ConfluencePublisher.Service
{
    class ConfluenceApiCaller
    {
        private static Markdown markdown = new Markdown(true);
        private static readonly Encoding encoding = Encoding.UTF8;

        public static dynamic getContent(dynamic siteConfig, string pageName) {

            string url = siteConfig.url + Constraints.CONTENT + String.Format(Constraints.GET_PAGE_PARAMETERS, siteConfig.space, pageName.Replace(" ", "%20"));
            return callApi(siteConfig, url, HttpMethod.Get.ToString());
        }

        public static dynamic createContent(dynamic siteConfig, dynamic page, int parentId, string convertedBody) {

            string url = siteConfig.url + Constraints.CONTENT;

            var body = new
            {
                title = Convert.ToString(page.title),
                type = "page",
                space = new { key = siteConfig.space },
                ancestors = new[] { new { id = parentId } },
                body = new { storage = new { value = convertedBody, representation = "storage" } }
            };

            return callApi(siteConfig, url, "POST", new StringContent(JsonConvert.SerializeObject(body)), "application/json");
        }

        public static dynamic updateContent(dynamic siteConfig, dynamic page, int parentId, string convertedBody)
        {
            string url = siteConfig.url + String.Format(Constraints.UPDATE_CONTENT, page.id);

            var body = new
            {
                version = new { number = page.version.number + 1 },
                title = Convert.ToString(page.title),
                type = "page",
                space = new { key = siteConfig.space },
                ancestors = new[] { new { id = parentId } },
                body = new { storage = new { value = convertedBody, representation = "storage" } }
            };

            return callApi(siteConfig, url, "PUT", new StringContent(JsonConvert.SerializeObject(body)), "application/json");
        }

        public static dynamic getAttachment(dynamic siteConfig, string pageId, string filename) {
            string url = siteConfig.url + String.Format(Constraints.ATTACHMENT, pageId) + String.Format(Constraints.GET_ATTACHMENT_PARAMETERS, filename);
            return callApi(siteConfig, url, HttpMethod.Get.ToString());
        }
        public static dynamic createAttachment(dynamic siteConfig, string pageId, dynamic attachmentDefinition, string attachmentPath)
        {
            string url = siteConfig.url + String.Format(Constraints.ATTACHMENT, pageId);

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(attachmentPath)), "file", Path.GetFileName(attachmentPath));
            multipartContent.Add(new StringContent("true"), "minorEdit");
            multipartContent.Add(new StringContent(Convert.ToString(attachmentDefinition.comment)), "comment");

            return callApi(siteConfig, url, "POST", multipartContent);
        }

        public static dynamic updateAttachment(dynamic siteConfig, string pageId, dynamic publishedAttachment, dynamic attachmentDefinition)
        {
            string url = siteConfig.url + String.Format(Constraints.UPDATE_ATTACHMENT_DATA, pageId, Convert.ToString(publishedAttachment.results[0].id));

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(attachmentDefinition.filePath)), "file", Path.GetFileName(attachmentDefinition.filePath));
            multipartContent.Add(new StringContent("true"), "minorEdit");
            multipartContent.Add(new StringContent(Convert.ToString(attachmentDefinition.comment)), "comment");

            return callApi(siteConfig, url, "POST", multipartContent);
        }

        public static dynamic updateAttachmentProperties(dynamic siteConfig, string pageId, dynamic publishedAttachment, dynamic attachmentDefinition)
        {
            string url = siteConfig.url + String.Format(Constraints.UPDATE_ATTACHMENT_PROPERTIES, pageId, Convert.ToString(publishedAttachment.results[0].id));

            var body = new
            {
                version = new { number = publishedAttachment.results[0].version.number + 1 },
                id = Convert.ToString(publishedAttachment.results[0].id),
                type = "attachment",
                metadata = new { mediaType = attachmentDefinition.mediaType, comment = attachmentDefinition.comment }
            };


            return callApi(siteConfig, url, "PUT", new StringContent(JsonConvert.SerializeObject(body)), "application/json");
        }

        private static dynamic callApi(dynamic siteConfig, string url, string method = "GET", HttpContent content = null, string contentType = "application/json") {

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(method), url))
                {

                    var base64authorization = Convert.ToBase64String(encoding.GetBytes($"{siteConfig.username}:{siteConfig.password}"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64authorization);
                    if (content != null)
                    {
                        request.Content = content;
                        if (content is MultipartFormDataContent)
                        {
                            request.Headers.TryAddWithoutValidation("X-Atlassian-Token", "nocheck");
                        }
                        else if (content is StringContent)
                        {
                            request.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                        }
                    }

                    var task = Task.Run(async () => await httpClient.SendAsync(request));
                    var result = task.Result;
                    if (!result.IsSuccessStatusCode) 
                    {
                        throw new ConfluenceApiCallException($"Error occured when try to call Confluence API: {result.Content.ReadAsStringAsync()}");
                    }

                    return JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result);
                }
            }

        }

      
    }
}
