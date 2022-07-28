using Guoxu.LabManager.Domains.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Commons.Dto
{
    public class HomeDto
    {
        public HomeMasterDto Master { get; set; }   

        public HomeNormalDto Normal { get; set; }
    }
   
}
