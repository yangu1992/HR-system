using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NI.Application.HR.HRBase.Models.OfferActivity;
using NI.Application.HR.HRBase.Models.OfferActivity.PersonalInfoFormModels;


namespace NI.Application.HR.HRBase.Controllers
{
    public class ExcelReadController
    {
        public PersonalInfoFormModel getFormModel(string path)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, false))
            {
                // Attempt to add a new WorksheetPart.
                // The call to AddNewPart generates an exception because the file is read-only.
                WorkbookPart wbPart = document.WorkbookPart;
                Sheet worksheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == "Form").FirstOrDefault();
                WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(worksheet.Id));

                PersonalInfoModel personalInfo = getPersonalInfo(wbPart, wsPart.Worksheet);
                List<EducationBackgroudModel> eduInfo = getEduInfo(wbPart, wsPart.Worksheet);
                List<WorkExperienceModel> workExperienceInfo = getWorkExperienceInfo(wbPart, wsPart.Worksheet);
                List<FamilyMemberModel> familyMemInfo = getFamilyMemInfo(wbPart, wsPart.Worksheet);
                List<ChildrenInfoModel> childsInfo = getChildsInfo(wbPart, wsPart.Worksheet);

                PersonalInfoFormModel model = new PersonalInfoFormModel
                {
                    PersonalInfo = personalInfo,
                    EduInfo = eduInfo,
                    WorkExperienceInfo = workExperienceInfo,
                    FamilyMemInfo = familyMemInfo,
                    ChildsInfo = childsInfo,
                    Department = getCellValue(wbPart, wsPart.Worksheet, "C42"),
                    Position = getCellValue(wbPart, wsPart.Worksheet, "F42"),
                    TentativeOnboardDate = getCellValue(wbPart, wsPart.Worksheet, "I42")
                };

                return model;
            }
        }


        private List<ChildrenInfoModel> getChildsInfo(WorkbookPart wbPart, Worksheet worksheet)
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
                if (getCellValue(wbPart, worksheet, fullNameColNo + i) != null)
                {
                    childsInfo = new ChildrenInfoModel
                    {
                        FullName = getCellValue(wbPart, worksheet, fullNameColNo + i),
                        Gender = getCellValue(wbPart, worksheet, genderColNo + i),
                        BirthDate = getCellValue(wbPart, worksheet, birthColNo + i),
                    };

                    model.Add(childsInfo);
                }
            }

            return model;
        }

        private List<FamilyMemberModel> getFamilyMemInfo(WorkbookPart wbPart, Worksheet worksheet)
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
                if (getCellValue(wbPart, worksheet, fullNameColNo + i) != null)
                {
                    familyMemInfo = new FamilyMemberModel
                    {
                        FullName = getCellValue(wbPart, worksheet, fullNameColNo + i),
                        Relations = getCellValue(wbPart, worksheet, relationColNo + i),
                        Employer = getCellValue(wbPart, worksheet, employerColNo + i),
                        Department = getCellValue(wbPart, worksheet, depColNo + i),
                        Position = getCellValue(wbPart, worksheet, positionColNo + i)
                    };

                    model.Add(familyMemInfo);
                }
            }

            return model;
        }

        private List<WorkExperienceModel> getWorkExperienceInfo(WorkbookPart wbPart, Worksheet worksheet)
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
                if (getCellValue(wbPart, worksheet, employerColNo + i) != null)
                {
                    workExperienceInfo = new WorkExperienceModel
                    {
                        StartDate = getDate(getCellValue(wbPart, worksheet, startDateColNo + i)),
                        EndDate = getDate(getCellValue(wbPart, worksheet, endDateColNo + i)),
                        Employer = getCellValue(wbPart, worksheet, employerColNo + i),
                        Department = getCellValue(wbPart, worksheet, depColNo + i),
                        Position = getCellValue(wbPart, worksheet, positionColNo + i)
                    };

                    model.Add(workExperienceInfo);
                }
            }

            return model;
        }

        private List<EducationBackgroudModel> getEduInfo(WorkbookPart wbPart, Worksheet worksheet)
        {
            int recordStartRowNo = 13;
            int recordEndRowNo = 17;

            string startDateColNo = "B";
            string endDateColNo = "D";
            string schoolColNo = "E";
            string majorColNo = "G";
            string degreeColNo = "I";

            List<EducationBackgroudModel> model = new List<EducationBackgroudModel>();
            EducationBackgroudModel eduInfo;

            getCellValue(wbPart, worksheet, "E" + "13");

            for (int i = recordStartRowNo; i <= recordEndRowNo; i++)
            {
                if (getCellValue(wbPart, worksheet, schoolColNo + i) != null)
                {
                    eduInfo = new EducationBackgroudModel
                    {
                        StartDate = getDate(getCellValue(wbPart, worksheet, startDateColNo + i)),
                        EndDate = getDate(getCellValue(wbPart, worksheet, endDateColNo + i)),
                        School = getCellValue(wbPart, worksheet, schoolColNo + i),
                        Major = getCellValue(wbPart, worksheet, majorColNo + i),
                        Degree = getCellValue(wbPart, worksheet, degreeColNo + i)
                    };

                    model.Add(eduInfo);
                }
            }

            return model;
        }

        private string getDate(string s)
        {
            double value = Convert.ToDouble(s);
            var time = DateTime.FromOADate(value);

            return time.ToShortDateString();
        }

        private PersonalInfoModel getPersonalInfo(WorkbookPart wbPart, Worksheet worksheet)
        {
            PersonalInfoModel model = new PersonalInfoModel
            {
                ChineseFName = getCellValue(wbPart, worksheet, "C4"),
                ChineseGName = getCellValue(wbPart, worksheet, "E4"),
                Gender = getCellValue(wbPart, worksheet, "G4"),
                MaritalStatus = getCellValue(wbPart, worksheet, "I4"),
                EnglishFName = getCellValue(wbPart, worksheet, "C5"),
                EnglishGName = getCellValue(wbPart, worksheet, "E5"),
                BirthDate = getDate(getCellValue(wbPart, worksheet, "G5")),
                ID = getCellValue(wbPart, worksheet, "I5"),
                Nationality = getCellValue(wbPart, worksheet, "C6"),
                Hukou = getCellValue(wbPart, worksheet, "E6"),
                HukouType = getCellValue(wbPart, worksheet, "G6"),
                FileLocation = getCellValue(wbPart, worksheet, "I6"),
                HomeAddress = getCellValue(wbPart, worksheet, "C7"),
                PostCode = getCellValue(wbPart, worksheet, "I7"),
                Phone = getCellValue(wbPart, worksheet, "C8"),
                Email = getCellValue(wbPart, worksheet, "E8"),
                EmergencyContact = getCellValue(wbPart, worksheet, "G8"),
                EmergencyContactPhone = getCellValue(wbPart, worksheet, "I8")
            };

            return model;
        }

        private string getCellValue(WorkbookPart wbPart, Worksheet worksheet, string location)
        {
            Cell cell = worksheet.Descendants<Cell>().Where(c => c.CellReference.Value == location).FirstOrDefault();

            string value = cell.InnerText;
            if (value == "")
                return null;

            if (cell.DataType != null)
            {
                var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                if (stringTable != null)
                {
                    value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                }
            }

            return value;
        }

    }
}