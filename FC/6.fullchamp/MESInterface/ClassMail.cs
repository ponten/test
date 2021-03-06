using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace ClassMail
{
    public struct MailItem
    {
        public string To;         //收件者
        public string CC;         //副本
        public string From;       //寄件者名字
        public string Subject;    //信件主旨
        public string body;       //信件內容
        public string userName;   
        public string password;  
        public string smtpHost;   //外寄伺服器SMTP
        public int smtpPort;      //外寄伺服器PORT
    }

    public class SendMail
    {        
        public bool Send(MailItem aMailItem,ref string sMsg)
        {
            MailMessage mailMsg = new MailMessage();
            SmtpClient Client = new SmtpClient();
            try
            {
                //發送給多人
                if (!string.IsNullOrEmpty(aMailItem.To))
                {
                    string[] arrEmailTo = aMailItem.To.Split(';');                    
                    foreach (string strEmail in arrEmailTo)
                    {
                        mailMsg.To.Add(new MailAddress(strEmail));                        
                    }
                }
                //副本給多人
                if (!string.IsNullOrEmpty(aMailItem.CC))
                {
                    string[] arrEmailCC = aMailItem.CC.Split(';');
                    foreach (string strEmail in arrEmailCC)
                    {
                        mailMsg.CC.Add(new MailAddress(strEmail));                                                   
                    }
                }
                mailMsg.From = new MailAddress((aMailItem.From + "<" + aMailItem.userName + ">"));
                //new MailAddress(aMailItem.From);
                mailMsg.Subject = aMailItem.Subject;
                mailMsg.SubjectEncoding = System.Text.Encoding.UTF8;//信件主旨編碼
                mailMsg.Body = aMailItem.body;
                mailMsg.BodyEncoding = System.Text.Encoding.UTF8;//信件内容編碼
                mailMsg.IsBodyHtml = true; //是否是HTML郵件 
                
                Client.Host = aMailItem.smtpHost;
                Client.Port = aMailItem.smtpPort;
                //Client.EnableSsl = true;//經過ssl加密
                
                //設發送郵件身分驗證方式 
                //注意如果寄件人地址是abc@def.com,則用户名是abc而不是abc@def.com
                CredentialCache myCache = new CredentialCache();
                myCache.Add(aMailItem.smtpHost, aMailItem.smtpPort, "login", new NetworkCredential(aMailItem.userName, aMailItem.password)); //伺服器需要驗證
                Client.Credentials = myCache;

                //Client.Credentials = new NetworkCredential(aMailItem.userName, aMailItem.password);
            }
            catch (Exception ex)
            {
                sMsg = ex.Message;
                return false;
            }

            try
            {
                Client.Send(mailMsg);
                sMsg = "";
                return true;
            }
            catch (SmtpException ex)
            {
                sMsg = ex.Message;
                return false;
            }
        }
    }
}
