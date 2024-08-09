using System;
using YShine.Models.Systemp;

namespace YShine.ServiceCore.IService
{
	public interface ISysConfigService : IBaseService<SysConfig>
    {
        SysConfig GetSysConfigByKey(string key);
    }
}

