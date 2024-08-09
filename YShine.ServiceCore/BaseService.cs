using System;
using YShine.Repository;

namespace YShine.Service.ServiceCore
{
    public class BaseService<T> : BaseRepository<T> where T : class, new()
    {
        /// <summary>
        /// 基础服务定义
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public BaseService()
		{
		}
	}
}

