using System;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using YShine.Models.Systemp;
using YShine.Service.ServiceCore;
using YShine.ServiceCore.Service.IService;
using YShine.Tools.Infrastructure.Attribute;

namespace YShine.ServiceCore.Service
{
    [AppService(ServiceType = typeof(ISysRoleService), ServiceLifetime = LifeTime.Transient)]
    public class SysRoleService: BaseService<SysRole>,ISysRoleService
    {
        public SysRoleService( )
        {

        }

        public List<string> SelectUserRoleKeys(long userId)
        {
            var list = SelectUserRoleListByUserId(userId);
            return list.Select(x => x.RoleKey).ToList();
        }

        public List<SysRole> SelectUserRoleListByUserId(long userId)
        {
            return Queryable()
             .Where(role => role.DelFlag == 0)
             .Where(it => SqlFunc.Subqueryable<SysUserRole>().Where(s => s.UserId == userId).Any())
             .OrderBy(role => role.RoleSort)
             .ToList();
        }
    }
}

