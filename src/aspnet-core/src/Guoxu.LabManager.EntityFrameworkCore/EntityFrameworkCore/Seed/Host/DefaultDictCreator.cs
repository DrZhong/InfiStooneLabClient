using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Editions;
using Abp.Application.Features;
using Guoxu.LabManager.Domains;
using Guoxu.LabManager.Editions;

namespace Guoxu.LabManager.EntityFrameworkCore.Seed.Host
{
    public class DefaultDictCreator
    {
        private readonly LabManagerDbContext _context;

        public DefaultDictCreator(LabManagerDbContext context)
        {
            _context = context;
            _context.ShouldInterceptSaveChange = false;
        }

        public void Create()
        {
            CreateDict();
        }

        private void CreateDict()
        {
            //var purity = _context.Dict.IgnoreQueryFilters().FirstOrDefault(e => e.Value == Dict.ReagentPurity);
            //if (purity == null)
            //{
            //    purity = new Dict() { Name = "�Լ����ȵ�λ", Value = Dict.ReagentPurity,WarehouseType = WarehouseType.�Լ�};
            //    _context.Dict.Add(purity);
            //    _context.SaveChanges(); 
            //}

            //var purity1 = _context.Dict.IgnoreQueryFilters().FirstOrDefault(e => e.Value == Dict.ReagentCapacityUnit);
            //if (purity1 == null)
            //{
            //    purity1 = new Dict() { Name = "������λ", Value = Dict.ReagentCapacityUnit, WarehouseType = WarehouseType.�Լ� };
            //    _context.Dict.Add(purity1);
            //    _context.SaveChanges();
            //}

            //var purity2 = _context.Dict.IgnoreQueryFilters().FirstOrDefault(e => e.Value == Dict.ReagentStorageCondition);
            //if (purity2 == null)
            //{
            //    purity2 = new Dict() { Name = "�洢����", Value = Dict.ReagentStorageCondition, WarehouseType = WarehouseType.�Լ� };
            //    _context.Dict.Add(purity2);
            //    _context.SaveChanges();
            //}

        } 
 
    }
}
