namespace InfiStoone.LabClient.Runtime.Entity
{
    public enum ReagentStockStatusEnum
    {
        待入库 = 0,
        在库 = 1,
        /// <summary>
        /// 领用
        /// </summary>
        离库 = 2,
        /// <summary>
        /// 瓶子回收后 变成已用完
        /// </summary>
        已用完 = 3
    }
}