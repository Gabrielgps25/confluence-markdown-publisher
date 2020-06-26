using ConfluencePublisher.Exceptions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConfluencePublisher.Util
{
    class SiteDefinitionReader
    {
        public static dynamic getSiteDefinition(string jsonConfigPath) {

            var resourceName = @"ConfluencePublisher.Resources.json.schema";

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            using (StreamReader r = new StreamReader(jsonConfigPath))
            {
                JSchema schema = JSchema.Parse(reader.ReadToEnd());
                JObject configurationJson = JObject.Parse(r.ReadToEnd());

                if (!configurationJson.IsValid(schema, out IList<string> messages)) {
                    throw new InvalidJsonSchemaException($"Site Json Configuration: {jsonConfigPath} invalid format with messages: {string.Join(";\n", messages.ToArray())}");
                }
                return configurationJson;
            }
        }

        public static dynamic getConfluenceNugetConfig(string jsonConfigPath)
        {
            var resourceName = @"ConfluencePublisher.Resources.json.schema";

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            using (StreamReader r = new StreamReader(jsonConfigPath))
            {
                JSchema schema = JSchema.Parse(reader.ReadToEnd());
                JObject configurationJson = JObject.Parse(r.ReadToEnd());

                if (!configurationJson.IsValid(schema, out IList<string> messages))
                {
                    throw new InvalidJsonSchemaException($"Site Json Configuration: {jsonConfigPath} invalid format with messages: {string.Join(";\n", messages.ToArray())}");
                }
                return configurationJson;
            }
        }
    }
}
