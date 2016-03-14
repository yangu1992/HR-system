using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository
{
    public class PersonelRepository:IPersonelRepository
    {
        public int Add(Table_PersonelInfo entity)
        {
            using (var db = new HrDbContext())
            {
                entity.PersonelInfo_CreatedAt = DateTime.Now;
                entity.PersonelInfo_CreatedBy = "NIA Support";

                db.Table_PersonelInfo.Add(entity);
                db.SaveChanges();

                return entity.PersonelInfo_ID;
            } 
        }

        public Table_PersonelInfo FindByID(int? id) {
            using (var db = new HrDbContext())
            {
                var result = db.Table_PersonelInfo.Find(id);
                return result;
            } 
        }

        public void Update(Table_PersonelInfo entity) {
            using (var db = new HrDbContext())
            {
                Table_PersonelInfo person = (from p in db.Table_PersonelInfo
                                      where p.PersonelInfo_ID == entity.PersonelInfo_ID
                                      select p).First();

                person.PersonelInfo_FName = entity.PersonelInfo_FName ?? person.PersonelInfo_FName;
                person.PersonelInfo_LName = entity.PersonelInfo_LName ?? person.PersonelInfo_LName;
                person.PersonelInfo_Gender = entity.PersonelInfo_Gender;
                person.PersonelInfo_MartialStatus = entity.PersonelInfo_MartialStatus;
                person.PersonelInfo_RomanFName = entity.PersonelInfo_RomanFName ?? person.PersonelInfo_RomanFName;
                person.PersonelInfo_RomanLName = entity.PersonelInfo_RomanLName ?? person.PersonelInfo_RomanLName;
                person.PersonelInfo_BirthDate = entity.PersonelInfo_BirthDate;
                person.PersonelInfo_IdentityID = entity.PersonelInfo_IdentityID;
                person.PersonelInfo_Nationality = entity.PersonelInfo_Nationality;
                person.PersonelInfo_Hukou = entity.PersonelInfo_Hukou;
                person.PersonelInfo_HukouType = entity.PersonelInfo_HukouType;
                person.PersonelInfo_FileLocation = entity.PersonelInfo_FileLocation;
                person.PersonelInfo_HomeAddress = entity.PersonelInfo_HomeAddress;
                person.PersonelInfo_PostCode = entity.PersonelInfo_PostCode;
                person.PersonelInfo_Phone = entity.PersonelInfo_Phone??person.PersonelInfo_Phone;
                person.PersonelInfo_Email = entity.PersonelInfo_Email ?? person.PersonelInfo_Email;
                person.PersonelInfo_EmergencyContact = entity.PersonelInfo_EmergencyContact;
                person.PersonelInfo_EmergencyContactPhone = entity.PersonelInfo_EmergencyContactPhone;
                person.PersonelInfo_ModifiedAt = DateTime.Now;
                person.PersonelInfo_ModifiedBy = "NIA Support";

                db.SaveChanges();
            }
        }
    }
}
