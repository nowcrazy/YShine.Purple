using System;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using YShine.Models.Systemp;
using YShine.Service.ServiceCore;
using YShine.ServiceCore.Service.IService;
using YShine.Tools.Infrastructure.Attribute;

namespace YShine.ServiceCore.Service
{
    [AppService(ServiceType = typeof(ISysMenuService), ServiceLifetime = LifeTime.Transient)]
    public class SysMenuService : BaseService<SysMenu>, ISysMenuService
    {
        public List<string> SelectMenuPermsByUserId(long userId)
        {
            var menus = Context.Queryable<SysMenu, SysRoleMenu, SysUserRole, SysRole>((m, rm, ur, r) => new JoinQueryInfos(
                 JoinType.Left, m.MenuId == rm.Menu_id,
                 JoinType.Left, rm.Role_id == ur.RoleId,
                 JoinType.Left, ur.RoleId == r.RoleId
                 ))
                 .WithCache(60 * 10)
                 .Where((m, rm, ur, r) => m.Status == "0" && r.Status == 0 && ur.UserId == userId)
                 .Select((m, rm, ur, r) => m).ToList();
            var menuList = menus.Where(f => !string.IsNullOrEmpty(f.Perms));

            return menuList.Select(x => x.Perms).Distinct().ToList();
        }
    }
}

