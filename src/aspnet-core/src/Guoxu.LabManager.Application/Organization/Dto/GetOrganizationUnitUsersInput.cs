using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;
using Guoxu.LabManager.Dto;

namespace Guoxu.LabManager.Organization.Dto
{
    public class GetOrganizationUnitUsersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        //[Range(1, long.MaxValue)]
        public long? Id { get; set; }

        public bool? IsOnline { get; set; }

        public string Filter { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "user.Name, user.Surname";
            }
            else if (Sorting.Contains("userName"))
            {
                Sorting = Sorting.Replace("userName", "user.userName");
            }
            else if (Sorting.Contains("addedTime"))
            {
                Sorting = Sorting.Replace("addedTime", "uou.creationTime");
            }
        }
    }
}