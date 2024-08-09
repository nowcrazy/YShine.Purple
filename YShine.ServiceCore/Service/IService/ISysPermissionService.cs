using System;
using YShine.Models.Systemp;

namespace YShine.ServiceCore.Service.IService
{
	public interface ISysPermissionService 
	{
        public List<string> GetRolePermission(SysUser user);
        public List<string> GetMenuPermission(SysUser user);
    }
}

