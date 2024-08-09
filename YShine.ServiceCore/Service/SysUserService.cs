using System;
using Microsoft.Extensions.DependencyInjection;
using YShine.Models.Systemp;
using YShine.Models.Systemp.Dto;
using YShine.Service.ServiceCore;
using YShine.ServiceCore.Service.IService;
using YShine.Tools.Infrastructure.Attribute;

namespace YShine.ServiceCore.Service
{
    [AppService(ServiceType = typeof(ISysUserService), ServiceLifetime = LifeTime.Transient)]
    public class SysUserService: BaseService<SysUser>, ISysUserService
    {
		public SysUserService()
		{
		}

        public SysUser Login(LoginBodyDto loginBody)
        {
            return Queryable().First(x => x.UserName == loginBody.Username && x.Password.ToLower() == loginBody.Password.ToLower()&&x.DelFlag==0) ;
        }

        public void UpdateLoginInfo(string userIP, long userId)
        {
            Update(new SysUser() { LoginIP = userIP, LoginDate = DateTime.Now, UserId = userId }, it => new { it.LoginIP, it.LoginDate });
        }
    }
}

