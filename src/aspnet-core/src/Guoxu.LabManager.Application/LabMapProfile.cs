using System.Linq;
using AutoMapper;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Guoxu.LabManager.Authorization.Roles;
using Guoxu.LabManager.BaseInfo.Dto;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.ReagentService.Dto;

namespace Guoxu.LabManager
{
    public class LabMapProfile : Profile
    {
        public LabMapProfile()
        {
            // Role and permission
           

            CreateMap<Location, LocationDto>()
                .ReverseMap()
                .ForMember(w => w.Warehouse, src => src.Ignore())
                .ForMember(w=>w.LocationStorageAttr,src=>src.Ignore());


            CreateMap<Dict, DictDto>()
                .ReverseMap()
                .ForMember(w => w.Child, src => src.Ignore());


            CreateMap<Reagent, ReagentDto>()
                .ForMember(w=>w.ReagentLocationIds,src=>src.MapFrom(q=>q.ReagentLocations.Select(w=>w.LocationId)))
                .ReverseMap()
                .ForMember(w => w.ReagentCatalog, src => src.Ignore())   //ÊÔ¼ÁÀàÐÍ²»Ó³Éä
                .ForMember(w => w.SupplierCompany, src => src.Ignore())
                .ForMember(w => w.ReagentLocations, src => src.Ignore()) 
                .ForMember(w => w.ProductionCompany, src => src.Ignore());


            //CreateMap<NormalReagentOperateRecord, ReagentOperateRecordDto>()
            //    .ForMember(w=>w.ReagentCasNo, src => src.MapFrom(url=>url.)
        }
    }
}
