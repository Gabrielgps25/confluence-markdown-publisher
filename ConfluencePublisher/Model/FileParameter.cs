using System;
using System.Collections.Generic;
using System.Text;

namespace ConfluencePublisher.Model
{
    class FileParameter
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Comment { get; set; }
        public FileParameter(byte[] file) : this(file, null) { }
        public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }

        public FileParameter(byte[] file, string filename, string contenttype, string comment)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
            Comment = comment;
        }
    }
}
