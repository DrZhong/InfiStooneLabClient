using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guoxu.LabManager.Organization.Dto
{
    public class UserToOrganizationUnitInput
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }



    public class UsersToOrganizationUnitInput
    {

        public List<long> UserIds { get; set; }

        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }
}