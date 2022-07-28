using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guoxu.LabManager.Commons.Dto;

public class MasterStockInInputDto
{
    /// <summary>
    /// 试剂Id
    /// </summary>
    public int ReagentStockId { get; set; }

    /// <summary>
    /// 仓库Id
    /// </summary>
    public int WarehouseId { get; set; }
    /// <summary>
    /// 入库数量
    /// 普通专用    
    /// </summary> 
    public int Acount { get; set; }

    public decimal Weight { get; set; }
}   

public class MasterStockBackInputDto
{
    public List<int> ReagentStockIds { get; set; }

    /// <summary>
    /// 仓库Id
    /// </summary>
    public int WarehouseId { get; set; }
}

public class MasterStockBackV2InputDto
{
    public decimal Weight { get; set; }

    public int ReagentStockId { get; set; }
    /// <summary>
    /// 仓库Id
    /// </summary>
    public int WarehouseId { get; set; }
}