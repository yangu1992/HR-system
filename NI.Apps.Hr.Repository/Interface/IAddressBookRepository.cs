using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI.Apps.Hr.Repository.Interface
{
    public interface IAddressBookRepository
    {
        void Add(int OfferID, string DomainLogin, DateTime ContractStartDate);
    }
}
