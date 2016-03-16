using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NI.Application.HR.HRBase.Models.OfferActivity;
using Microsoft.Office.Interop.Excel;
using NI.Application.HR.HRBase.Models.OfferActivity.PersonalInfoFormModels;

namespace NI.Application.HR.HRBase.Controllers
{
    public class ExcelReadController
    {
        public PersonalInfoFormModel getFormModel(HttpPostedFileBase fb, string path)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Sheets sheets;
            object oMissiong = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;

            workbook = app.Workbooks.Open(path, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong,
                oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
            sheets = workbook.Worksheets;

            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)sheets.get_Item(1);//读取第一张表  

            PersonalInfoModel personalInfo = getPersonalInfo(worksheet);
            List<EducationBackgroudModel> eduInfo = getEduInfo(worksheet);
            List<WorkExperienceModel> workExperienceInfo = getWorkExperienceInfo(worksheet);
            List<FamilyMemberModel> familyMemInfo = getFamilyMemInfo(worksheet);
            List<ChildrenInfoModel> childsInfo = getChildsInfo(worksheet);

            PersonalInfoFormModel model = new PersonalInfoFormModel
            {
                PersonalInfo=personalInfo,
                EduInfo=eduInfo,
                WorkExperienceInfo=workExperienceInfo,
                FamilyMemInfo=familyMemInfo,
                ChildsInfo=childsInfo,
                Department=worksheet.get_Range("C42").Value2,
                Position=worksheet.get_Range("F42").Value2,
                TentativeOnboardDate = worksheet.get_Range("I42").Text
            };

            workbook.Close(false, oMissiong, oMissiong);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            workbook = null;
            app.Workbooks.Close();
            app.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            app = null;

            return model;
        }

        private List<ChildrenInfoModel> getChildsInfo(Worksheet worksheet)
        {
            int recordStartRowNo = 38;
            int recordEndRowNo = 40;

            string fullNameColNo = "B";
            string genderColNo = "E";
            string birthColNo = "H";

            List<ChildrenInfoModel> model = new List<ChildrenInfoModel>();
            ChildrenInfoModel childsInfo;

            for (int i = recordStartRowNo; i <= recordEndRowNo; i++)
            {
                if (worksheet.get_Range(fullNameColNo + i).Value2 != null)
                {
                    childsInfo = new ChildrenInfoModel
                    {
                        FullName = worksheet.get_Range(fullNameColNo + i).Value2,
                        Gender = worksheet.get_Range(genderColNo + i).Value2,
                        BirthDate = worksheet.get_Range(birthColNo + i).Text,
                    };

                    model.Add(childsInfo);
                }
            }

            return model;
        }

        private List<FamilyMemberModel> getFamilyMemInfo(Worksheet worksheet)
        {
            int recordStartRowNo = 32;
            int recordEndRowNo = 35;

            string fullNameColNo = "B";
            string relationColNo = "C";
            string employerColNo = "E";
            string depColNo = "G";
            string positionColNo = "I";

            List<FamilyMemberModel> model = new List<FamilyMemberModel>();
            FamilyMemberModel familyMemInfo;

            for (int i = recordStartRowNo; i <= recordEndRowNo; i++)
            {
                if (worksheet.get_Range(fullNameColNo + i).Value2 != null)
                {
                    familyMemInfo = new FamilyMemberModel
                    {
                        FullName = worksheet.get_Range(fullNameColNo + i).Value2,
                        Relations = worksheet.get_Range(relationColNo + i).Value2,
                        Employer = worksheet.get_Range(employerColNo + i).Value2,
                        Department = worksheet.get_Range(depColNo + i).Value2,
                        Position = worksheet.get_Range(positionColNo + i).Value2
                    };

                    model.Add(familyMemInfo);
                }
            }

            return model;
        }

        private List<WorkExperienceModel> getWorkExperienceInfo(Worksheet worksheet)
        {
            int recordStartRowNo = 22;
            int recordEndRowNo = 28;

            string startDateColNo = "B";
            string endDateColNo = "D";
            string employerColNo = "E";
            string depColNo = "G";
            string positionColNo = "I";

            List<WorkExperienceModel> model = new List<WorkExperienceModel>();
            WorkExperienceModel workExperienceInfo;

            for (int i = recordStartRowNo; i <= recordEndRowNo; i++)
            {
                if (worksheet.get_Range(employerColNo + i).Value2 != null)
                {
                    workExperienceInfo = new WorkExperienceModel
                    {
                        StartDate = worksheet.get_Range(startDateColNo + i).Text,
                        EndDate = worksheet.get_Range(endDateColNo + i).Text,
                        Employer = worksheet.get_Range(employerColNo + i).Value2,
                        Department = worksheet.get_Range(depColNo + i).Value2,
                        Position = worksheet.get_Range(positionColNo + i).Value2
                    };

                    model.Add(workExperienceInfo);
                }
            }

            return model;
        }

        private List<EducationBackgroudModel> getEduInfo(Worksheet worksheet)
        {
            int recordStartRowNo = 13;
            int recordEndRowNo=17;

            string startDateColNo="B";
            string endDateColNo="D";
            string schoolColNo="E";
            string majorColNo="G";
            string degreeColNo="I";

            List<EducationBackgroudModel> model = new List<EducationBackgroudModel>();
            EducationBackgroudModel eduInfo;

            for (int i = recordStartRowNo; i<=recordEndRowNo; i++) {
                if (worksheet.get_Range(schoolColNo + i).Value2 != null) {
                    eduInfo = new EducationBackgroudModel { 
                        StartDate=worksheet.get_Range(startDateColNo+i).Text,
                        EndDate = worksheet.get_Range(endDateColNo + i).Text,
                        School=worksheet.get_Range(schoolColNo+i).Value2,
                        Major = worksheet.get_Range(majorColNo+i).Value2,
                        Degree = worksheet.get_Range(degreeColNo+i).Value2
                    };

                    model.Add(eduInfo);
                }
            }

            return model;
        }

        private PersonalInfoModel getPersonalInfo(Worksheet worksheet)
        {
            //Range r = null;
            //r = worksheet.get_Range("I5");
            //Console.WriteLine("身份证号"+r.Value2);
            //r = worksheet.get_Range("I7");
            //Console.WriteLine("邮政编码" + r.Value2);
            //r = worksheet.get_Range("C8");
            //Console.WriteLine("联系电话" + r.Text);
            //r = worksheet.get_Range("I8");
            //Console.WriteLine("联系电话" + r.Value2);
            //r = worksheet.get_Range("B22");
            //Console.WriteLine("出生日期" + r.Text);
            //r = worksheet.get_Range("H38");
            //Console.WriteLine("就职时间" + r.Text);
            
            
            PersonalInfoModel model = new PersonalInfoModel
            {
                ChineseFName=worksheet.get_Range("C4").Value2,
                ChineseGName = worksheet.get_Range("E4").Value2,
                Gender = worksheet.get_Range("G4").Value2,
                MaritalStatus = worksheet.get_Range("I4").Value2,
                EnglishFName = worksheet.get_Range("C5").Value2,
                EnglishGName = worksheet.get_Range("E5").Value2,
                BirthDate = worksheet.get_Range("G5").Text,
                ID = worksheet.get_Range("I5").Text,
                Nationality = worksheet.get_Range("C6").Value2,
                Hukou = worksheet.get_Range("E6").Value2,
                HukouType = worksheet.get_Range("G6").Value2,
                FileLocation = worksheet.get_Range("I6").Value2,
                HomeAddress = worksheet.get_Range("C7").Value2,
                PostCode = worksheet.get_Range("I7").Text,
                Phone = worksheet.get_Range("C8").Text,
                Email = worksheet.get_Range("E8").Value2,
                EmergencyContact = worksheet.get_Range("G8").Value2,
                EmergencyContactPhone = worksheet.get_Range("I8").Text
            };

            return model;
        }
    }
}