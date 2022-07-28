using Abp.Organizations;
using System.ComponentModel.DataAnnotations;

namespace Guoxu.LabManager.Organization.Dto
{
    public class CreateOrganizationUnitInput
    {
        public long? ParentId { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sort { get; set; }
    }
}