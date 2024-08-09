using System;
using YShine.Models.Systemp;
using YShine.Models.Systemp.Dto;

namespace YShine.ServiceCore.Service.IService
{
	public interface ISysUserService : IBaseService<SysUser>
    {
        void UpdateLoginInfo(string userIP, long userId);
        SysUser Login(LoginBodyDto loginBody);
    }
}

