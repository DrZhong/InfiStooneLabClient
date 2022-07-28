using Abp.AutoMapper;
using Guoxu.LabManager.Authentication.External;

namespace Guoxu.LabManager.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
