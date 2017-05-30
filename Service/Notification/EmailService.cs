using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 邮件业务逻辑
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        private string host = ConfigurationManager.AppSettings["Email:Host"];
        private int port = int.Parse(ConfigurationManager.AppSettings["Email:Port"]);
        private string emailAddress = ConfigurationManager.AppSettings["Email:Address"];
        private string senderName = ConfigurationManager.AppSettings["Email:Name"];
        private string password = ConfigurationManager.AppSettings["Email:Password"];

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            var email = new MailMessage(new MailAddress(emailAddress, senderName),
               new MailAddress(message.Destination))
               {
                   Subject = message.Subject,
                   Body = message.Body,
                   IsBodyHtml = true
               };

            using (var client = new SmtpClient(host, port))
            {
                client.Credentials = new NetworkCredential(emailAddress, password);
                try
                {
                    client.Send(email);
                    return Task.FromResult(0);
                }
                catch(Exception ex)
                {
                    return Task.FromResult(ex.Message);
                }
            }
        }

        /// <summary>
        /// 模板替换
        /// </summary>
        /// <param name="body"></param>
        /// <param name="school"></param>
        /// <returns></returns>
        //public string Replace(string body, School school, List<Token> tokens)
        //{
        //    if (string.IsNullOrEmpty(body) || school == null)
        //        return string.Empty;

        //    body = body.Replace("%{Name}%", school.Name);
        //    body = body.Replace("%{EnglishName}%", school.EnglishName);
        //    body = body.Replace("%{Address}%", school.Address);
        //    body = body.Replace("%{Telephone}%", school.Telephone);
        //    body = body.Replace("%{PostCode}%", school.PostCode);
        //    body = body.Replace("%{Description}%", school.Description);

        //    foreach (var token in tokens)
        //    {
        //        body = body.Replace(string.Format("%{0}%", token.Key), token.Value);
        //    }
        //    return body;
        //}
    }
}
