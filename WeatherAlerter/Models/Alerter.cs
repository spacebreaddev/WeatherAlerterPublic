using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Configuration;

namespace WeatherAlerter.Models
{
    //this should be moved out as to be used for other alerters and not tied to the weather one.
    class Alerter
    {
        /// <summary>
        /// email message to send
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// recipient of message, email or cell
        /// </summary>
        public Recipient Recipient { get; set; }

        /// <summary>
        /// email subject
        /// </summary>
        public string Subject { get; set; }

        public Sender Sender { get; set; }

         

        public void SendAlert()
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(Sender.Email);

                //https://stackoverflow.com/questions/31246531/can-i-send-sms-messages-from-a-c-sharp-application
                message.To.Add(new MailAddress(Recipient.Email));
                                                                 

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(Sender.Name, Sender.Password)
                };

                                //message.CC.Add(new MailAddress("carboncopy@foo.bar.com"));
                message.Subject = Subject;
                message.Body = Body;

                string messageToSend = message.Body;
                string messageLeftToSend = messageToSend;
                bool done = false;

                while (messageLeftToSend.Length > 0 && !done)
                {
                    if (messageLeftToSend.Length > 160)
                    {
                        messageToSend = messageLeftToSend.Substring(0, 160);
                        messageLeftToSend = messageLeftToSend.Substring(160); //trim off what we sent

                        message.Body = messageToSend;
                    }
                    else //send whatever is left
                    {
                        message.Body = messageLeftToSend;
                        done = true;
                    }
                    //send the message
                    //uncomment this to send the sms
                    smtp.Send(message);
                    Console.WriteLine(message.Body);
                }

                Console.WriteLine("message sent!");
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    class Sender
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    class Recipient
    {
        public string Email { get; set; }
    }
}
