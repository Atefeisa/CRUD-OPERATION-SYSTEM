using MVC.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace MVC.PL.Helper
{
    public class EmailSetting
    {
        public static void SendEmail(Email email)
        {

            // Email Server : gmail.com 

            var client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("atefeisa10@gmail.com", "akdnqcerkfjpbkzh");

            client.Send("atefeisa10@gmail.com", email.Recipients, email.Subject, email.Body); //Send Email
        }
    }
}
