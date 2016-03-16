using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NI.Application.HR.HRBase.Models;
using NI.Application.HR.HRBase.Models.OfferActivity;
using Outlook = Microsoft.Office.Interop.Outlook;
using Word = Microsoft.Office.Interop.Word;

namespace NI.Application.HR.HRBase.Controllers
{
    public class EmailControl
    {
        private string EmailTemplatePath = "~/Views/EmailTemplate/";
        public void Send(EmailModel email)
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mailItem = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);

            Outlook.Accounts accounts = app.Session.Accounts;

            // Outlook.Account  GetAccountForEmailAddress(oApp, "support@mydomain.com");
            // Use this account to send the e-mail. 
            Outlook.Account account = null;
            foreach (Outlook.Account item in accounts)
            {
                // When the e-mail address matches, return the account. 
                if (item.SmtpAddress == "huihui.gong@ni.com")
                {
                    account = item;
                }
            }
            mailItem.SendUsingAccount = account;
            mailItem.To = email.ToEmail;
            mailItem.CC = email.CCEmail;
            mailItem.Subject = email.Subject;
            mailItem.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
            mailItem.HTMLBody = email.Body;

            if (!string.IsNullOrEmpty(email.AttachmentsLocation))
            {
                //find all the attchments
                DirectoryInfo folder = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(email.AttachmentsLocation));
                foreach (FileInfo file in folder.GetFiles())
                {
                    mailItem.Attachments.Add(file.FullName, Outlook.OlAttachmentType.olByValue);
                }
            }

            if (!string.IsNullOrEmpty(email.NIOfferPath))
            {
                mailItem.Attachments.Add(email.NIOfferPath, Outlook.OlAttachmentType.olByValue);
            }

            ((Outlook._MailItem)mailItem).Send();
            mailItem = null;
            app = null;
        }

        //send approval email to Mgr for approve
        public void SendApprovalEmail(OfferCreateModel model)
        {
            EmailModel email = new EmailModel();

            email.ToEmail = model.Email.ToEmail;
            email.CCEmail = model.Email.CCEmail;
            email.Subject = model.Email.Subject;
            email.ToEmail = model.Email.ToEmail;

            string emailBody = getApprovalEmailBody(model.Personel.FName + model.Personel.LName, model.Offer.Postion);
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
            var fileName = "NI Offer Letter.docx";
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Attachments/ApproveToMgr/");

            string newPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/" + model.Offer.ID + "/");
            if (!Directory.Exists(newPath))//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(newPath);
            }

            //Create an instance for word app
            var application = new Word.Application();
            application.Visible = true;

            var template = application.Documents.Open(path + fileName);
            template.StoryRanges[Word.WdStoryType.wdMainTextStory].Copy();
            var newDoc = template.Application.Documents.Add();
            newDoc.StoryRanges[Word.WdStoryType.wdMainTextStory].PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);
            newDoc.SaveAs2(newPath + fileName);

            //replace
            Word.Find findObject = application.Selection.Find;
            string searchText = null;
            string replaceText = null;

            string[] searchList = { "[Chinese Name]", "[Current Date]", "[English Name]", "[Position]" ,
                                  "[Onboarding Date]","[Location]","[Salary]","[Salary*12]",
                                  "[Current Date + 7]"};
            string[] replaceList ={model.Personel.FName+model.Personel.LName,
                                     DateTime.Now.ToShortDateString(),
                                     model.Personel.RomanFName+model.Personel.RomanLName,
                                     model.Offer.Postion,
                                     model.Offer.OnBoardingDate.ToString(),
                                     model.Offer.Location,
                                     model.Salary.Salary.ToString(),
                                     (model.Salary.Salary*12).ToString(),
                                     DateTime.Now.AddDays(7).ToShortDateString()
                                     };
            int index = searchList.Length;
            for (int i = 0; i < index; i++)
            {
                searchText = searchList[i];
                replaceText = replaceList[i];
                SearchAndReplace(findObject, searchText, replaceText);
            }

            object SaveChanges = false; //保存更改
            object OriginalFormat = System.Type.Missing;
            object missing = System.Type.Missing;
            ((Microsoft.Office.Interop.Word._Document)template).Close(ref SaveChanges, ref OriginalFormat, ref missing);
            ((Microsoft.Office.Interop.Word._Document)newDoc).Close(Word.WdSaveOptions.wdSaveChanges, ref OriginalFormat, ref missing);
            ((Microsoft.Office.Interop.Word._Application)application).Quit(ref SaveChanges, ref OriginalFormat, ref missing);

            return (newPath + fileName);
        }

        private void SearchAndReplace(Word.Find findObject, string searchText, string replaceText)
        {
            findObject.ClearFormatting();
            findObject.Text = searchText;
            findObject.Replacement.ClearFormatting();
            findObject.Replacement.Text = replaceText;

            object replaceAll = Word.WdReplace.wdReplaceAll;

            object missing = System.Type.Missing;
            findObject.Execute(ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref replaceAll, ref missing, ref missing, ref missing, ref missing);
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

        public void SendDomainRequestEmail(string CandidateName, string Position, string LineMgr, DateTime? OnboardDate)
        {
            EmailModel email = new EmailModel();

            email.ToEmail = "yan.gu@ni.com";    //<IT Penang>
            email.CCEmail = "yan.gu@ni.com";    //Huiyan.guo@ni.com, Yan.huang@ni.com
            email.Subject = "new comer on " + OnboardDate;

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

            string result = reader.ReadToEnd();
            result = result.Replace("$Candidate Name$", CandidateName);
            result = result.Replace("$Position$", position);
            result = result.Replace("$Line Manager$", lineMgr);
            result = result.Replace("$Onboarding Date$", onboardDate.ToString());

            return result;
        }


        //send offer letter for candidate to hr
        internal void SendOfferEmail(OfferCreateModel model)
        {
            EmailModel email = new EmailModel();

            email.ToEmail = model.Email.ToEmail;
            email.CCEmail = model.Email.CCEmail;
            email.Subject = model.Email.Subject;
            email.ToEmail = model.Email.ToEmail;

            string emailBody = getOfferEmailBody(model.Personel.Email, model.Personel.FName + model.Personel.LName, model.Offer.Postion);
            email.Body = emailBody;

            email.AttachmentsLocation = "~/Content/Attachments/OfferToCandidate/";

            email.NIOfferPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/" + model.Offer.ID + "/NI Offer Letter.docx"); ;
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

        internal void SendWelcomeEmail(string name, string candidateEmail, string MgrEmail, DateTime onboardingDate)
        {
            EmailModel email = new EmailModel();

            email.ToEmail = candidateEmail;
            email.CCEmail = MgrEmail;
            email.Subject = "Welcome to join NI ! - " + name;
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
            result = result.Replace("$OnboardingDate$", onboardingDate.ToShortDateString());

            return result;
        }

        //send welcome email to line manager
        internal void SendWelcomeEmail(string LineMgrEmail)
        {
            EmailModel email = new EmailModel();

            email.ToEmail = LineMgrEmail;
            email.Subject = "Onboarding Process to Line Manager for CANDIDATE";
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