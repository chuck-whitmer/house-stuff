using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Hai
{
    public class Email
    {
        string smtp = "smtp12.sherwebcloud.com";
        string username = "chuck@whitmer.us";
        string password;
        SmtpClient smtpClient;
        string recipient;
        StringBuilder message = new StringBuilder(1000);

        public Email(string recipient)
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(Connect.strMyRegistry);
            if (regkey != null)
            {
                password = (string)regkey.GetValue("SmtpPassword", "");
                if (password.Length == 0)
                    throw new Exception("Need SMTP password");
            }

            smtpClient = new SmtpClient(smtp)
            {
                Port = 587,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };
            this.recipient = recipient;

            // smtpClient.Send("chuck@whitmer.us", "chuck@whitmer.us", "It's a test", "Testing");
        }

        public void Add(string str)
        {
            message.Append(str);
            message.Append(Environment.NewLine);
        }

        public void Send()
        {
            smtpClient.Send(username, recipient, "House Status", message.ToString());
            message.Clear();
        }
    }
}
