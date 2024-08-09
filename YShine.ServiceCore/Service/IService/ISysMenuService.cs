using System;
using YShine.Models.Systemp;

namespace YShine.ServiceCore.Service.IService
{
	public interface ISysMenuService : IBaseService<SysMenu>
	{
		List<string> SelectMenuPermsByUserId(long userId);
	}
}

