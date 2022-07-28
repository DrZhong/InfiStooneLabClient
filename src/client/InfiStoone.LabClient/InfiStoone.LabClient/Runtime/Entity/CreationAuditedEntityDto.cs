using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Runtime.Entity
{
    public abstract class CreationAuditedEntityDto<T>
    {
        public T Id { get; set; }
    }

    public abstract class CreationAuditedEntityDto:CreationAuditedEntityDto<int>
    { 
    }
}
