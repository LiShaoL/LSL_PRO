 
// 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using LSL_PRO.Kernel;

namespace LSL_PRO.Utilities
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    public class SMTPManager
    {
        public static string MailUser = ConfigHelper.GetValue("MailUser");
        public static string MailName = ConfigHelper.GetValue("MailName");
        public static string MailHost = ConfigHelper.GetValue("MailHost");
        public static string MailPwd = ConfigHelper.GetValue("MailPwd");
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="  sender">发送人、支持发送多个人每个地址用 ; 号隔开</param>
        /// <param name="Name">名称</param>
        /// <param name="Describe">内容</param>
        /// <param name="File_Path">附件</param>
        /// <returns></returns>
        public static bool MailSending(string Sender, string Name, string Describe, string File_Path)
        {
            try
            {
                MailAddress from = new MailAddress(MailUser, MailName); //邮件的发件人
                MailMessage mail = new MailMessage();

                //设置邮件的标题
                mail.Subject = Name;//任务名称

                //设置邮件的发件人
                //Pass:如果不想显示自己的邮箱地址，这里可以填符合mail格式的任意名称，真正发mail的用户不在这里设定，这个仅仅只做显示用
                mail.From = from;

                //设置邮件的收件人
                string address = "";
                string displayName = "";
                /**/
                /*  这里这样写是因为可能发给多个联系人，每个地址用 ; 号隔开
                一般从地址簿中直接选择联系人的时候格式都会是 ：用户名1 < mail1 >; 用户名2 < mail 2>; 
                因此就有了下面一段逻辑不太好的代码
                如果永远都只需要发给一个收件人那么就简单了 mail.To.Add("收件人mail");
                */
                string[] mailNames = (Sender + ";").Split(';');
                foreach (string name in mailNames)
                {
                    if (name != string.Empty)
                    {
                        if (name.IndexOf('<') > 0)
                        {
                            displayName = name.Substring(0, name.IndexOf('<'));
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        else
                        {
                            displayName = string.Empty;
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        mail.To.Add(new MailAddress(address, displayName));
                    }
                }

                //设置邮件的抄送收件人
                //这个就简单多了，如果不想快点下岗重要文件还是CC一份给领导比较好
                //mail.CC.Add(new MailAddress("Manage@hotmail.com", "尊敬的领导");

                //设置邮件的内容
                mail.Body = Describe;
                //设置邮件的格式
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                //设置邮件的发送级别
                mail.Priority = MailPriority.Normal;

                //设置邮件的附件，将在客户端选择的附件先上传到服务器保存一个，然后加入到mail中
                if (File_Path != "")
                {
                    mail.Attachments.Add(new Attachment(File_Path));
                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                } SmtpClient client = new SmtpClient();
                //设置用于 SMTP 事务的主机的名称，填IP地址也可以了
                client.Host = MailHost;
                //设置用于 SMTP 事务的端口，默认的是 25
                client.Port = 25;
                client.UseDefaultCredentials = false;
                //这里才是真正的邮箱登陆名和密码， 我的用户名为 MailUser ，我的密码是 MailPwd
               // string strMailPwd = DESEncrypt.Decrypt(MailPwd);
                string strMailPwd = MailPwd; //先去除解密
                client.Credentials = new System.Net.NetworkCredential(MailUser, strMailPwd.Trim());
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                ////如果发送失败，SMTP 服务器将发送 失败邮件告诉我
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                //都定义完了，正式发送了，很是简单吧！
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="smtpserver">SMTP服务器</param>
        /// <param name="userName">登录帐号</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="nickName">发件人昵称</param>
        /// <param name="strfrom">发件人</param>
        /// <param name="strto">收件人</param>
        /// <param name="subj">主题</param>
        /// <param name="bodys">内容</param>
        public static void SendMail(string smtpserver, string userName, string pwd, string nickName, string strfrom, string strto, string subj, string bodys)
        {
            try
            {
                SmtpClient _smtpClient = new SmtpClient();
                _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
                _smtpClient.Host = smtpserver;//指定SMTP服务器
                _smtpClient.Credentials = new System.Net.NetworkCredential(userName, pwd);//用户名和密码

                //MailMessage _mailMessage = new MailMessage(strfrom, strto);
                MailAddress _from = new MailAddress(strfrom, nickName);
                MailAddress _to = new MailAddress(strto);
                MailMessage _mailMessage = new MailMessage(_from, _to);
                _mailMessage.Subject = subj;//主题
                _mailMessage.Body = bodys;//内容
                _mailMessage.BodyEncoding = System.Text.Encoding.Default;//正文编码
                _mailMessage.IsBodyHtml = true;//设置为HTML格式
                _mailMessage.Priority = MailPriority.Normal;//优先级
                _smtpClient.Send(_mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
