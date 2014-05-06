using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace TeachMe.Helpers
{
    public enum EmailTemplate
    {
        Feedback,
        Registration,
        ResetPassword
    }
    public class Email
    {

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="to">email to</param>
        /// <param name="name">to username</param>
        /// <param name="confirmationToken">token</param>
        /// <param name="emailForm">email form view</param>
        /// /// <param name="hostUrl">host url</param>
        public static void Send(string to, string name, string confirmationToken, EmailTemplate template)
        {
            var hostUrl = HttpContext.Current.Request.Url.Host;
            Thread thread = new Thread(() => SendEmailInBackground(to, name, "", "", confirmationToken, template, hostUrl));
            thread.Start();

            //dynamic email = new Email(emailForm);
            //email.To = to;
            //email.From = new System.Net.Mail.MailAddress("jce.teachme@gmail.com", "TeachMe Support");
            ////email.Subject = mail.Subject;
            //email.UserName = username;
            //email.ConfirmationToken = confirmationToken;
            //email.Send();
        }
        public static void Send(string to, string username, string subject, string body, EmailTemplate template)
        {
            var hostUrl = HttpContext.Current.Request.Url.Host;
            Thread thread = new Thread(() => SendEmailInBackground(to, username, subject, body, "", template, hostUrl));
            thread.Start();
        }
        private static void SendEmailInBackground(string to, string username, string subject, string body, string confirmationToken, EmailTemplate template, string hostUrl)
        {
            dynamic email = new Postal.Email(template.ToString());
            email.To = to;
            email.From = new System.Net.Mail.MailAddress("jce.teachme@gmail.com", "TeachMe Support");
            if (!string.IsNullOrEmpty(subject))
                email.Subject = subject;
            if (!string.IsNullOrEmpty(body))
                email.Body = body;
            email.UserName = username;
            email.ConfirmationToken = confirmationToken;
            email.HostUrl = hostUrl;
            email.Send();
        }


    }
}