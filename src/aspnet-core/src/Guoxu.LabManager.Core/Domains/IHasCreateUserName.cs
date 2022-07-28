using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guoxu.LabManager.Domains
{
    public interface IHasCreateUserName
    {
        /// <summary>
        /// 创建者
        /// </summary>
        string CreateUserName { get; set; }
    }

    public interface IHasUpdateUserName
    {
        /// <summary>
        /// 创建者
        /// </summary>
        string UpdateUserName { get; set; }
    }

    public interface IHasDeleteUserName
    {
        /// <summary>
        /// 创建者
        /// </summary>
        string DeleteUserName { get; set; }
    }
}
