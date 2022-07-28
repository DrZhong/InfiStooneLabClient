using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager
{
    public class DomainServiceBase: DomainService
    {   
        protected DomainServiceBase()
        {
            LocalizationSourceName = LabManagerConsts.LocalizationSourceName;
        }
    }
}
