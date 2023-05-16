using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendMailApi.Interface;
using SendMailApi.Commons.MailSender;
using SendMailApi.Commons.Models;
using Newtonsoft.Json.Linq;
using Mailjet.Client;

namespace SendMailApi.Provider
{
    public class EmailService : EmaiSender, IMailSender
    {
        protected override async Task Send(EmailModel email)
        {
            try
            {
                JArray jArray = new JArray();
                JArray attachments = new JArray();
                if (email.lstPath != null && email.lstPath.Count > 0)
                {
                    email.lstPath.ForEach(path =>
                    {
                        attachments.Add(GetFileAttachment(path));
                    });
                }
                //if (email.Attachments != null && email.Attachments.Count() > 0)
                //{
                //    email.Attachments.ToList().ForEach(attachment =>
                //    attachments.Add(
                //        new JObject {
                //            new JProperty("Content-type",attachment.ContentType),
                //            new JProperty( "Filename",attachment.Name),
                //            new JProperty("content",Convert.ToBase64String(attachment.Data))
                //    }));
                //}
                jArray.Add(new JObject {
                            {
                            "FromEmail",
                            "nguyenthithuthuy0996@gmail.com"
                            }, {
                            "FromName",
                            "nguyenthithuthuy0996@gmail.com"
                            }, {
                            "Recipients",
                            new JArray {
                                new JObject {
                                {
                                    "Email",
                                    email.EmailAddress
                                }, {
                                    "Name",
                                   email.EmailAddress
                                }
                                }
                            }
                            }, {
                            "Subject",
                            email.Subject
                            }, {
                            "Text-part",
                            email.Body
                            }, {
                            "Html-part",
                            email.Body
                            },  {
                            "Attachments",
                            attachments
                            }});

                var client = CreateMailJetV3Client();
                MailjetRequest request = new MailjetRequest
                {
                    Resource = Mailjet.Client.Resources.Send.Resource,
                }
                 .Property(Mailjet.Client.Resources.Send.Messages, jArray);

                var response = await client.PostAsync(request);
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private JObject GetFileAttachment(string filePath)
        {
            JObject attachment = new JObject();
            if (File.Exists(filePath))
            {
                string content = "";
                string fileName = Path.GetFileName(filePath);
                string mimeType = "application/unknown";
                string ext = System.IO.Path.GetExtension(fileName).ToLower();
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                if (regKey != null && regKey.GetValue("Content Type") != null)
                    mimeType = regKey.GetValue("Content Type").ToString();             

                using (FileStream reader = new FileStream(filePath, FileMode.Open))
                {
                    byte[] buffer = new byte[reader.Length];
                    reader.Read(buffer, 0, (int)reader.Length);
                    content = Convert.ToBase64String(buffer);
                }

                if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(mimeType))
                {
                    attachment = new JObject(
                        new JProperty("Content-type", mimeType),
                           new JProperty("Filename", fileName),
                           new JProperty("content",(content))
                    );

                }
            }
            return attachment;
        }

    }
}
