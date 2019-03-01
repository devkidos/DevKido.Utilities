using System;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace DevKido.Utilities
{
    public static class Email
    {
        private static void SendDefaultEmail(string body)
        {
            try
            {
                var mailmessage = new MailMessage { From = new MailAddress("rca.devtesting@gmail.com", "User Registered") };

                body += "<br>Url = " + HttpContext.Current.Request.Url;

                mailmessage.To.Add("anrorathod@gmail.com");
                mailmessage.Subject = "New User Registered";
                mailmessage.Body = body;
                mailmessage.IsBodyHtml = true;

                SmtpClient smtpClient = null;
                smtpClient = new SmtpClient();

                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential("rca.devtesting@gmail.com", "IamDeveloper@1");
                smtpClient.EnableSsl = true;
                 
                smtpClient.Send(mailmessage); 
            }
            catch (Exception ex)
            { 
            }
        }

        /// <summary>
        /// Add keys in AppSettings in web.config 
        /// Host
        /// Port
        /// defaultCredentials = true or false
        /// fromUserName
        /// fromUserPassword
        /// name="newUserOrOther" value="new or other"
        /// </summary>
        /// <param name="fromName"></param>
        /// <param name="displayName"></param>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="newUserOrOther"></param>
        public static string SendEmailUsing(string fromName, string displayName, string toEmail, string subject, string body, string newUserOrOther)
        {
            try
            {
                if (newUserOrOther.ToLower() == "new")
                {
                    SendDefaultEmail(body);
                }

                var mailmessage = new MailMessage { From = new MailAddress(fromName, displayName) };

                mailmessage.To.Add(toEmail);
                mailmessage.Subject = subject;
                mailmessage.Body = body;
                mailmessage.IsBodyHtml = true;

                SmtpClient smtpClient = null;
                smtpClient = new SmtpClient();

                smtpClient.Host = System.Configuration.ConfigurationManager.AppSettings["Host"].ToString();
                smtpClient.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"].ToString());
                smtpClient.UseDefaultCredentials = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["defaultCredentials"].ToString());
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["fromUserName"].ToString(), System.Configuration.ConfigurationManager.AppSettings["fromUserPassword"].ToString());
                smtpClient.EnableSsl = true; 
                smtpClient.Send(mailmessage);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "success";
        }

       
    }
}
