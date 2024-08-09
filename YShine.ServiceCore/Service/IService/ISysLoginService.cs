using System;
using YShine.Models.Systemp;
using YShine.Models.Systemp.Dto;

namespace YShine.ServiceCore.Service.IService
{
	public interface ISysLoginService : IBaseService<SysLogininfor>
    {
        void CheckLockUser(string userName);
        SysUser Login(LoginBodyDto loginBody, SysLogininfor sysLogininfor);
    }
}

