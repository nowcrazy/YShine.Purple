using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using YShine.Models.Systemp;
using YShine.ServiceCore.Service.IService;
using YShine.Tools;
using YShine.Tools.Infrastructure.Attribute;

namespace YShine.ServiceCore.Service
{
    [AppService(ServiceType = typeof(ISysPermissionService), ServiceLifetime = LifeTime.Transient)]
    public class SysPermissionService: ISysPermissionService
    {
        private readonly ISysRoleService _sysRoleService;
        private readonly ISysMenuService _sysMenuService;
        public SysPermissionService(ISysRoleService sysRoleService, ISysMenuService sysMenuService)
		{
            _sysRoleService = sysRoleService;
            _sysMenuService = sysMenuService;

        }

        public List<string> GetMenuPermission(SysUser user)
        {
            List<string> perms = new();
            // 管理员拥有所有权限
            if (user.IsAdmin || GetRolePermission(user).Exists(f => f.Equals(GlobalConstant.AdminRole)))
            {
                perms.Add(GlobalConstant.AdminPerm);
            }
            else
            {
                perms.AddRange(_sysMenuService.SelectMenuPermsByUserId(user.UserId));
            }
            return perms;
        }

        public List<string> GetRolePermission(SysUser user)
        {
            List<string> roles = new();
            // 管理员拥有所有权限
            if (user.IsAdmin)
            {
                roles.Add("admin");
            }
            else
            {
                roles.AddRange(_sysRoleService.SelectUserRoleKeys(user.UserId));
            }
            return roles;
        }
    }
}

