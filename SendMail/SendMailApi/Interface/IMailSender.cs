using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendMailApi.Commons.Models;

namespace SendMailApi.Interface
{
    public interface IMailSender
    {
        Task SendEmail(EmailModel emailModel);
        Task SendEmail(string mailAdd, string title, string content);
    }
}
