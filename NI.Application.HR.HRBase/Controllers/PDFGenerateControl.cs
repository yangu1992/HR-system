using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NI.Application.HR.HRBase.Models.OfferActivity;

namespace NI.Application.HR.HRBase.Controllers
{
    public class PDFGenerateControl
    {
        public string  GenerateApprovalPDF(OfferCreateModel model){
            string name = model.Personel.FName + model.Personel.LName;
            string fileName=name+".pdf";
            string filePath=Path.Combine(HttpContext.Current.Server.MapPath("~/PDFFiles/"+name),fileName);

            Document doc=new Document(PageSize.A4,2,2,2,2);

            //Create paragraph for show in PDF file header
            Paragraph p=new Paragraph("Candidate Offer Detail");
            //p.SetAlignment("center");

            PdfWriter.GetInstance(doc,new FileStream(filePath,FileMode.Create));

            PdfPTable pdfTab=new PdfPTable(4);
            pdfTab.HorizontalAlignment=1;
            pdfTab.SpacingBefore=20f;
            pdfTab.SpacingAfter=20f;

            pdfTab.AddCell("FName");
            pdfTab.AddCell(model.Personel.FName);
            pdfTab.AddCell("LName");
            pdfTab.AddCell(model.Personel.LName);
            pdfTab.AddCell("position");
            pdfTab.AddCell(model.Offer.Postion);

            doc.Open();
            doc.Add(p);
            doc.Add(pdfTab);
            doc.Close();

            return filePath;
        }
    }
}