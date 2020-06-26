using System;
using System.IO;
using ConfluencePublisher.Exceptions;

namespace ConfluencePublisher.Util
{
    class PathValidator
    {
        public static void validate(string solutionPath) {
            if (String.IsNullOrEmpty(solutionPath) || !Directory.Exists(solutionPath))
            {
                throw new InvalidPathException("Solution absolute path not exists.");
            }

            string configJsonPath = Path.Combine(solutionPath, "site.json");

            if (String.IsNullOrEmpty(configJsonPath) || !File.Exists(configJsonPath))
            {
                throw new InvalidPathException($"[ERROR] File {configJsonPath} not exists.");
            }
        }
    }
}
