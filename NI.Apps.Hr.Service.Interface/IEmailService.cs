using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI.Apps.Hr.Service.Interface
{
    public interface IEmailService
    {
        void SendEmail(string From,string To,string CC,string Subject);
    }
}
