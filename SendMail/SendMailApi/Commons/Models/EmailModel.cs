using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMailApi.Commons.Models
{
    public class EmailModel
    {
        public string EmailAddress { get; set; } = String.Empty;
        public string Subject { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public List<string> lstPath { get; set; } = new List<string>();
        //public IEnumerable<MyAttachment>? Attachments { get; set; }
        
    }

    public class MyAttachment
    {
        public string ContentType { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public byte[] Data { get; set; } = new byte[10];
    }
}
