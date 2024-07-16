using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;
using LOGISTIK.Models;

namespace LOGISTIK.Helper
{
    public class SendMail
    {
        string smtp = "mailrelay-internal.infineon.com";
        string fromEmail = "BTHGreenlineR@infineon.com";
        string Fail = "Muhammad.Ilham@infineon.com";
        string cc1 = "Panjaitan.Hiras@infineon.com";
        string cc2 = "Wiwiek.Purwanti@infineon.com";
        string cc3 = "Evita.Ika@infineon.com";
        string cc4 = "Jubasril.Jubasril@infineon.com";
        string CC5 = "Muhammad.Ilham@infineon.com";

        public void SendMailToSuperior(string emailTemplate, string Name, string Date, string selectedEmailsString)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(smtp);
                mail.From = new MailAddress(fromEmail);
                mail.Subject = "SIMULLATION || FW: New Logistic Shipment Request For " + Date;

                if (!string.IsNullOrEmpty(selectedEmailsString))
                {
                    mail.To.Add(selectedEmailsString);

                    mail.CC.Add(cc1);
                    mail.CC.Add(cc2);
                    mail.CC.Add(cc3);
                    mail.CC.Add(cc4);
                    //mail.CC.Add(CC5);
                }

                mail.Body = emailTemplate;
                mail.IsBodyHtml = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential("OJT TEAM", "1234");

                try
                {
                    SmtpServer.Send(mail);
                    Console.WriteLine("Email berhasil dikirim.");   
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                        Console.WriteLine($"Pengiriman ke {ex.InnerExceptions[i].FailedRecipient} gagal. Status: {status}");
                    }
                }
                catch (SmtpFailedRecipientException ex)
                {
                    SmtpStatusCode status = ex.StatusCode;
                    Console.WriteLine($"Pengiriman ke {ex.FailedRecipient} gagal. Status: {status}");
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"Pengiriman email gagal. Error: {ex.Message}");
                }
            }
            catch
            {

            }
        }
    }
}