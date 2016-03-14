using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;
using System.Data.Entity.Core.Objects;

namespace NI.Apps.Hr.Repository
{
    public class AddressBookRepository:IAddressBookRepository
    {
        public void Add(int OfferID, string Domain, DateTime ContractStartDate)
        {
            using (var db = new HrDbContext())
            {
                //insert new DmainLogin into AddressBook2.Table_Account
                ObjectParameter p = new ObjectParameter("returnVal", typeof(int));
                db.Proc_InsertNewAccount(p, OfferID, Domain, ContractStartDate);
                int DomainLoginID = (int)p.Value;
                //insert new related AddressInfo into AddressBook2.Table_AddressBook
                db.Proc_InsertAddressBook(OfferID, DomainLoginID);
            }
        }
    }
}
