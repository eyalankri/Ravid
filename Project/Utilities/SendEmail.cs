using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Ravid.Utilities
{
    public static class SendEmail
    {
       
        public static bool SendEmailWithGmail(string emailContent, string emailRecipients, string emailSubject, bool addBcc)
        {
            var account = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Gmail")["Account"];
            var password = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Gmail")["Password"];


            var _emailSendFrom = account;
            var _emailDisplayName = account;
            var _emailGmailPsw = password;
            var _emailBccRecipients = "eyal.ank@gmail.com";

            var @from = _emailSendFrom;  //Replace this with your own correct Gmail Address

            var to = emailRecipients;
            // //Replace this with the Email Address to whom you want to send the mail

            var mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(@from, _emailDisplayName, System.Text.Encoding.UTF8);


            if (addBcc)
            {
                mail.Bcc.Add(_emailBccRecipients);
            }

            mail.Subject = emailSubject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = emailContent;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = System.Net.Mail.MailPriority.High;


            var client = new SmtpClient
            {
                Credentials = new System.Net.NetworkCredential(@from, _emailGmailPsw),
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true
            };

            //Add the Creddentials- use  email id and password

            //// Gmail works on this port

            // //Gmail works on Server Secured Layer

            try
            {
                 client.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }

        }
    }
}
