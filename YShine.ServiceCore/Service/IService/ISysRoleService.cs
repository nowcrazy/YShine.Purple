using System;
using YShine.Models.Systemp;

namespace YShine.ServiceCore.Service.IService
{
	public interface ISysRoleService:IBaseService<SysRole>
	{
        List<SysRole> SelectUserRoleListByUserId(long userId);
        public List<string> SelectUserRoleKeys(long userId);
    }
}

