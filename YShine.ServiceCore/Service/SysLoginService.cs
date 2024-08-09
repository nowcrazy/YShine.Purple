using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using UAParser.Objects;
using YShine.Models.Systemp;
using YShine.Models.Systemp.Dto;
using YShine.Service.ServiceCore;
using YShine.ServiceCore.IService;
using YShine.ServiceCore.Service.IService;
using YShine.Tools.Common.Helper;
using YShine.Tools.Infrastructure.Attribute;
using YShine.Tools.Infrastructure.CustomException;
using YShine.Tools.WebExtensions;

namespace YShine.ServiceCore.Service
{
    [AppService(ServiceType = typeof(ISysLoginService), ServiceLifetime = LifeTime.Transient)]
    public class SysLoginService: BaseService<SysLogininfor>, ISysLoginService
    {
        private readonly ISysUserService _SysUserService;
        private readonly IHttpContextAccessor httpContextAccessor;
        //private readonly IStringLocalizer<SharedResource> _localizer;
        public SysLoginService(
               ISysUserService sysUserService,
            IHttpContextAccessor httpContextAccessor)
		{
            _SysUserService = sysUserService;
            this.httpContextAccessor = httpContextAccessor;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <exception cref="CustomExceptions"></exception>
        public void CheckLockUser(string userName)
        {
            var lockTimeStamp = CacheService.GetLockUser(userName);
            var lockTime = DateTimeHelper.ToLocalTimeDateBySeconds(lockTimeStamp);
            var ts = lockTime - DateTime.Now;
            if (lockTimeStamp>0 && ts.TotalSeconds>0)
            {
                throw new CustomExceptions(ResultCode.LOGIN_ERROR, $"你的账号已被锁,剩余{Math.Round(ts.TotalMinutes, 0)}分钟");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginBody"></param>
        /// <param name="logininfor"></param>
        /// <returns></returns>
        /// <exception cref="CustomExceptions"></exception>
        public SysUser Login(LoginBodyDto loginBody, SysLogininfor logininfor)
        {
            if (loginBody.Password.Length != 32)
            {
                loginBody.Password = NETCore.Encrypt.EncryptProvider.Md5(loginBody.Password);
            }
            SysUser user = _SysUserService.Login(loginBody);
            logininfor.UserName = loginBody.Username;
            logininfor.Status = "1";
            logininfor.LoginTime = DateTime.Now;
            logininfor.Ipaddr = loginBody.LoginIP;
            logininfor.ClientId = loginBody.ClientId;

            ClientInfo clientInfo = httpContextAccessor.HttpContext.GetClientInfo();
            logininfor.Browser = clientInfo.ToString();
            logininfor.Os = clientInfo.OS.ToString();

            if (user == null || user.UserId <= 0)
            {
                //logininfor.Msg = _localizer["login_pwd_error"].Value;
                logininfor.Msg = "用户名或密码错误";
                AddLoginInfo(logininfor);
                throw new CustomExceptions(ResultCode.LOGIN_ERROR, logininfor.Msg, false);
            }
            logininfor.UserId = user.UserId;
            if (user.Status == 1)
            {
                //logininfor.Msg = _localizer["login_user_disabled"].Value;//該用戶已禁用
                logininfor.Msg = "该用户已禁用";
                AddLoginInfo(logininfor);
                throw new CustomExceptions(ResultCode.LOGIN_ERROR, logininfor.Msg, false);
            }

            logininfor.Status = "0";
            logininfor.Msg = "登录成功";
            AddLoginInfo(logininfor);
            _SysUserService.UpdateLoginInfo(loginBody.LoginIP, user.UserId);
            return user;
        }
        /// <summary>
        /// 记录登录日志
        /// </summary>
        /// <param name="sysLogininfor"></param>
        /// <returns></returns>
        public void AddLoginInfo(SysLogininfor sysLogininfor)
        {
            Insert(sysLogininfor);
        }

        /// <summary>
        /// 清空登录日志
        /// </summary>
        public void TruncateLogininfo()
        {
            Truncate();
        }

        /// <summary>
        /// 删除登录日志
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteLogininforByIds(long[] ids)
        {
            return Delete(ids);
        }
    }
}

