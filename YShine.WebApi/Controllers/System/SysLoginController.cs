using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Lazy.Captcha.Core;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using SqlSugar;
using YShine.IService.ServiceCore;
using YShine.Models.Systemp;
using YShine.Models.Systemp.Dto;
using YShine.ServiceCore.IService;
using YShine.ServiceCore.Service;
using YShine.ServiceCore.Service.IService;
using YShine.Tools;
using YShine.Tools.Infrastructure.Controllers;
using YShine.Tools.Infrastructure.CustomException;
using YShine.Tools.Infrastructure.JWT;
using YShine.Tools.Infrastructure.Models;
using YShine.Tools.WebExtensions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YShine.Admin.WebApi.Controllers.System
{
    [ApiExplorerSettings(GroupName ="sys")]
    public class SysLoginController : BaseController
    {
        private readonly ICaptcha _captcha;
        private readonly ISysConfigService _sysConfigService;
        private readonly ISysLoginService _sysLoginService;
        private readonly ISysRoleService _sysRoleService;
        private readonly ISysPermissionService _sysPermissionService;
        //private readonly OptionsSetting _optionsSetting;
        public SysLoginController(
            ICaptcha captcha ,
            ISysConfigService sysConfigService,
            ISysLoginService sysLoginService,
            ISysRoleService  sysRoleService,
            ISysPermissionService sysPermissionService
            )
        {
            _captcha = captcha;
            _sysConfigService = sysConfigService;
            _sysLoginService = sysLoginService;
            _sysRoleService = sysRoleService;
            _sysPermissionService = sysPermissionService;
            //_optionsSetting = optionsSetting;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginBodyDto loginBody)
        {
            if (loginBody==null)
            {
                throw new CustomExceptions("请求参数错误");
            }
            loginBody.LoginIP = HttpContextExtension.GetClientUserIp(HttpContext);
            SysConfig sysConfig = _sysConfigService.GetSysConfigByKey("sys.account.captchaOnOff");
            if (sysConfig?.ConfigValue != "off" && !_captcha.Validate(loginBody.Uuid, loginBody.Code))
            {
                //return ToResponse(ResultCode.CAPTCHA_ERROR, "验证码错误");
            }

            _sysLoginService.CheckLockUser(loginBody.Username);
            string location = HttpContextExtension.GetIpInfo(loginBody.LoginIP);
            var user = _sysLoginService.Login(loginBody, new SysLogininfor() { LoginLocation = location });

            List<SysRole> roles = _sysRoleService.SelectUserRoleListByUserId(user.UserId);
            //权限集合 eg *:*:*,system:user:list
            List<string> permissions = _sysPermissionService.GetMenuPermission(user);

            TokenModel loginUser = new(user.Adapt<TokenModel>(), roles.Adapt<List<Roles>>());
            CacheService.SetUserPerms(GlobalConstant.UserPermKEY + user.UserId, permissions);
            return SUCCESS(JwtUtil.GenerateJwtToken(JwtUtil.AddClaims(loginUser)));
        }



        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("captchaImage")] 
        public IActionResult CaptchaImage()
        {
            string uuid = Guid.NewGuid().ToString().Replace("-", "");

            SysConfig sysConfig = _sysConfigService.GetSysConfigByKey("sys.account.captchaOnOff");
            //var captchaOff = sysConfig?.ConfigValue ?? "0";
            var captchaOff = 0;
            var info = _captcha.Generate(uuid, 60);
            var obj = new { captchaOff, uuid, img = info.Base64 };// File(stream, "image/png")

            return SUCCESS(obj);
        }
    }
}

 