using Microsoft.Extensions.DependencyInjection;
using YShine.Models.Systemp;
using YShine.Service.ServiceCore;
using YShine.ServiceCore.IService;
using YShine.Tools.Infrastructure.Attribute;

namespace YShine.IService.ServiceCore
{
    [AppService(ServiceType = typeof(ISysConfigService), ServiceLifetime = LifeTime.Transient)]
    public class SysConfigService : BaseService<SysConfig>, ISysConfigService
    {
        public SysConfig GetSysConfigByKey(string key)
        {
            return Queryable().First(f => f.ConfigKey == key);
        }
    }
}

