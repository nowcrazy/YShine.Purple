using System;
using CSRedis;
using Microsoft.Extensions.Configuration;

namespace YShine.Tools.Common.Helper.RedisHelp
{
	public class RedisServer
	{
        public static CSRedisClient Cache;
        public static CSRedisClient Session;
        public static void Initalize()
        {
            Cache = new CSRedisClient(AppSettings.GetConfig("RedisServer:Cache"));
            Session = new CSRedisClient(AppSettings.GetConfig("RedisServer:Session"));
            /*在没有使用builder.Services.AddAppService(); 注入servicer服务时
            Cache = new CSRedisClient("192.168.5.38:6379,defaultDatabase=0,poolsize=50,ssl=false,writeBuffer=10240,prefix=cache:");
            Session = new CSRedisClient("192.168.5.38:6379,defaultDatabase=0,poolsize=50,ssl=false,writeBuffer=10240,prefix=cache:");*/
        }
    }
}

