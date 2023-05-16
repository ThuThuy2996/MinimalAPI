using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailjet.Client;
using SendMailApi.Commons.Models;
using SendMailApi.Interface;

namespace SendMailApi.Commons.MailSender
{
    public abstract class EmaiSender : IMailSender
    {
        protected abstract Task Send(EmailModel email);

        public static MailjetClient CreateMailJetV3Client()
        {
            return new MailjetClient("f16e9e577a9912535799bc56f7702a9b", "b3c6236e8730696e7aeaf4cbc7f3b9a7");
        }

        public Task SendEmail(EmailModel email)
        {
            return Send(email);
        }
        public Task SendEmail(string mailAdd, string title, string content)
        {
            return SendEmail(new EmailModel { EmailAddress = mailAdd, Subject = title, Body = content });
        }
    }
}
