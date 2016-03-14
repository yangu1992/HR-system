using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Service.Interface;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Repository;
using NI.Apps.Hr.Repository.Interface;

namespace NI.Apps.Hr.Service
{
    public class PersonelService : IPersonelService
    {
        private IPersonelRepository _personelRepository;
        public IPersonelRepository PersonelRepository
        {
            get { return _personelRepository ?? (_personelRepository = new PersonelRepository()); }
        }
        public bool AddNewPersonel(Table_PersonelInfo newPerson){
            
            throw new NotImplementedException();
        }

        public Table_PersonelInfo FindPersonelByID(int? id) {
            return this.PersonelRepository.FindByID(id);
        }
    }
}
