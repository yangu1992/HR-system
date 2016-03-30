using Novacode;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NI.Application.HR.HRBase.Models;
using NI.Application.HR.HRBase.Models.OfferActivity;

using System.Net.Mail;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using NI.Apps.Hr.HrBase.Controllers;
using NI.Apps.Hr.Entity;
using System.Data.Entity.Core.Objects;

namespace NI.Application.HR.HRBase.Controllers
{
    public class EmailControl
    {
        private string EmailTemplatePath = "~/Views/EmailTemplate/";
        public void Send(EmailModel email)
        {
            MailMessage msg = new MailMessage();
            //msg.From = new MailAddress(email.FromEmail);
            string from = "yan.gu@ni.com";
            msg.From = new MailAddress(from); //msg.From=new MailAddress(user email); change with from 


            //if (!string.IsNullOrEmpty(email.ToEmail))
            //{
            //    foreach (var address in email.ToEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
            //    {
            //        msg.To.Add(address);
            //    }
            //}

            //if (!string.IsNullOrEmpty(email.CCEmail))
            //{
            //    foreach (var address in email.CCEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
            //    {
            //        msg.CC.Add(address);
            //    }
            //}

            //for test
            msg.To.Add(from);
            //msg.CC.Add("junyu.wu@ni.com");
            msg.CC.Add("huihui.gong@ni.com");
           
            msg.Subject = email.Subject;
            msg.IsBodyHtml = true;
            msg.Body = email.Body;

            if (!string.IsNullOrEmpty(email.AttachmentsLocation))
            {
                //find all the attchments
                DirectoryInfo folder = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(email.AttachmentsLocation));
                foreach (FileInfo file in folder.GetFiles())
                {
                    msg.Attachments.Add(new Attachment(file.FullName));
                }
            }

            if (!string.IsNullOrEmpty(email.NIOfferPath))
            {
                msg.Attachments.Add(new Attachment(email.NIOfferPath));
            }

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            string pwd = getEmailPwd(from);
            client.Credentials = new System.Net.NetworkCredential(from, pwd);
            //client.Credentials = new System.Net.NetworkCredential("yan.gu@ni.com", "73SL3TGV!"); //change with login user
            client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
            client.Host = "smtp.office365.com";
            client.TargetName = "STARTTLS/smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;

            try
            {
                client.Send(msg);
                // lblText.Text = "Message Sent Succesfully";
            }
            catch (Exception ex)
            {
                // lblText.Text = ex.ToString();
            }
        }

        private string getEmailPwd(string emailAddress)
        {
            using (var db = new HrDbContext())
            {
                ObjectParameter p = new ObjectParameter("returnVal", typeof(string));
                db.Proc_GetEmailAccountPwd(p, emailAddress);
                string pwd = (p.Value == null) ? string.Empty : p.Value.ToString();

                return pwd;
            }
        }

        //send approval email to Mgr for approve
        public void SendApprovalEmail(OfferCreateModel model)
        {
            EmailModel email = new EmailModel();

            email.ToEmail = model.Email.ToEmail;
            email.CCEmail = model.Email.CCEmail;
            email.Subject = model.Email.Subject;
            email.ToEmail = model.Email.ToEmail;

            string emailBody = getApprovalEmailBody(model.Personel.LName + model.Personel.FName, model.Offer.Postion);
            email.Body = emailBody;

            string NIOfferPath = generateNIOfferLetter(model);
            email.NIOfferPath = NIOfferPath;
            //email.AttachmentsLocation = "~/Content/Attachments/ApproveToMgr/";
            //generate candidate detail pdf as an attachment
            //   PDFGenerateControl pdf = new PDFGenerateControl();
            //   string pdfPath = pdf.GenerateApprovalPDF(model);
            //   email.AttachmentsLocation = pdfPath;

            Send(email);
        }

        private string generateNIOfferLetter(OfferCreateModel model)
        {
            string fileName = "NI Offer Letter.docx";
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Attachments/ApproveToMgr/");
            string templateFilePath = Path.Combine(path,fileName);

            var position=model.Offer.Postion;
            var name=model.Personel.LName+model.Personel.FName;
            string newFileName = fileName.Split(new Char[] { '.' })[0] + "-" + position + "-" + name+".docx";
            string newPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/" + model.Offer.ID + "/");
            string newFilePath = Path.Combine(newPath, newFileName);

            if (!Directory.Exists(newPath))//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(newPath);
            }
     
            if (File.Exists(newFilePath))
            {
                while (!IsFileReady(newFilePath)) { }
            }
            System.IO.File.Copy(templateFilePath, newFilePath, true);

            while (!IsFileReady(newFilePath)) { }
            SearchAndReplace(newFilePath, model);

            return (newFilePath);
        }

        private bool IsFileReady(string fileName)
        {
            try {
                using (FileStream inputStream = File.Open(fileName, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    if (inputStream.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch(Exception) {
                return false;
            }         
        }

        private void SearchAndReplace(string doc, OfferCreateModel model)
        {

            using (DocX document = DocX.Load(doc))
            {
                /*
                 * Replace each instance of the string pear with the string banana.
                 * Specifying true as the third argument informs DocX to track the
                 * changes made by this replace. The fourth argument tells DocX to
                 * ignore case when matching the string pear.
                 */
                string[] searchList = { "[Chinese Name]", "[Current Date]", "[English Name]", "[Position]" ,
                                  "[Onboarding Date]","[Location]","[Salary]","[Salary*12]",
                                  "[Current Date + 7]"};
                
                string[] replaceList ={model.Personel.LName+model.Personel.FName,
                                     DateTime.Now.ToString("MMMM dd, yyyy"),
                                     model.Personel.RomanLName+", "+model.Personel.RomanFName,
                                     model.Offer.Postion,
                                     (model.Offer.OnBoardingDate??DateTime.Now).ToString("MMMM dd, yyyy"),
                                     model.Offer.Location,
                                     string.Format("{0:0,00}", model.Salary.Salary),
                                     string.Format("{0:0,00}", (model.Salary.Salary*12)),
                                     DateTime.Now.AddDays(7).ToString("MMMM dd, yyyy")
                                     };

                int index = searchList.Length;
                string searchText = null;
                string replaceText = null;

                for (int i = 0; i < index; i++)
                {
                    searchText = searchList[i];
                    replaceText = replaceList[i];

                    Novacode.Formatting format = new Novacode.Formatting();
                    if (searchText == "[Position]" || searchText=="[Onboarding Date]"){
                        format.Bold = true;
                    }
                    document.ReplaceText(searchText, replaceText, false, RegexOptions.IgnoreCase,format);
                }       
                
                // Save changes made to this document
                document.Save();
            }// Release this document from memory.
            

            
        }
        private string getApprovalEmailBody(string candidateName, string position)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(EmailTemplatePath + "OfferDetailForMrgApprovalEmail.html");

            StreamReader reader = new StreamReader(path);

            string result = reader.ReadToEnd();
            result = result.Replace("$Candidate$", candidateName);
            result = result.Replace("$Position$", position);

            return result;
        }

        public void SendDomainRequestEmail(string CandidateName, string Position, string LineMgr, DateTime? OnboardDate,EmailModel email)
        {
            string emailBody = getDomainRequestEmailBody(CandidateName, Position, LineMgr, OnboardDate);
            email.Body = emailBody;
            //generate candidate detail pdf
            // PDFGenerateControl pdf = new PDFGenerateControl();
            // string pdfPath = pdf.GenerateApprovalPDF(model);
            Send(email);
        }

        private string getDomainRequestEmailBody(string CandidateName, string position, string lineMgr, DateTime? onboardDate)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(EmailTemplatePath + "DomainRequestEmail.html");

            StreamReader reader = new StreamReader(path);

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ja-JP");
            string result = reader.ReadToEnd();
            result = result.Replace("$Candidate Name$", CandidateName);
            result = result.Replace("$Position$", position);
            result = result.Replace("$Line Manager$", lineMgr);
            result = result.Replace("$Onboarding Date$", (onboardDate ?? DateTime.Now).ToShortDateString().ToString());

            return result;
        }


        //send offer letter for candidate to hr
        internal void SendOfferEmail(OfferCreateModel model)
        {
            EmailModel email = model.Email;

            string emailBody = getOfferEmailBody(model.Personel.Email, model.Personel.LName + model.Personel.FName, model.Offer.Postion);
            email.Body = emailBody;

            email.AttachmentsLocation = "~/Content/Attachments/OfferToCandidate/";

            var position = model.Offer.Postion;
            var name = model.Personel.LName + model.Personel.FName;
            email.NIOfferPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/" + model.Offer.ID + "/NI Offer Letter"+ "-" + position + "-" + name+".docx") ;
            // mailItem.Attachments.Add(pdfPath, Outlook.OlAttachmentType.olByValue);

            Send(email);
        }

        private string getOfferEmailBody(string candidateEmail, string name, string position)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(EmailTemplatePath + "OfferToCandidateEmail.html");

            StreamReader reader = new StreamReader(path);

            string result = reader.ReadToEnd();
            result = result.Replace("$Candidate Email Address$", candidateEmail);
            result = result.Replace("$CandidateName$", name);
            result = result.Replace("$Position$", position);

            return result;
        }

        internal void SendWelcomeEmail(string name, EmailModel email, DateTime onboardingDate)
        {
            email.Body = getCandidateWelcomeBody(name, onboardingDate);
            email.AttachmentsLocation = "~/Content/Attachments/WelcomeToCandidate/";

            Send(email);
        }

        private string getCandidateWelcomeBody(string name, DateTime onboardingDate)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(EmailTemplatePath + "WelcomeToCandidateEmail.html");

            StreamReader reader = new StreamReader(path);

            string result = reader.ReadToEnd();
            result = result.Replace("$CandidateName$", name);
            result = result.Replace("$OnboardingDate$", onboardingDate.ToShortDateString().ToString());

            return result;
        }

        //send welcome email to line manager
        internal void SendWelcomeEmail(EmailModel email)
        {
            email.Body = getLineMgrWelcomeBody();
            email.AttachmentsLocation = "~/Content/Attachments/WelcomeToLineMgr/";

            Send(email);
        }

        private string getLineMgrWelcomeBody()
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(EmailTemplatePath + "WelcomeToLineMgrEmail.html");

            StreamReader reader = new StreamReader(path);

            return reader.ReadToEnd();
        }
    }
}