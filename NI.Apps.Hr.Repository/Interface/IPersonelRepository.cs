using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository.Interface
{
    public interface IPersonelRepository
    {
        int Add(Table_PersonelInfo newPerson);

        Table_PersonelInfo FindByID(int? id);
        void Update(Table_PersonelInfo entity);
    }
}
