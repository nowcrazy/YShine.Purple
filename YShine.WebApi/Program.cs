using NLog.Web;
using SqlSugar.IOC;
using YShine.Tools;
using YShine.Tools.Common.Helper.RedisHelp;
using YShine.Tools.WebExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLog();
// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//普通验证码
builder.Services.AddCaptcha(builder.Configuration);
//builder.Services.AddIPRate(builder.Configuration);

//jwt 认证
builder.Services.AddJwt();

////绑定整个对象到Model上
//builder.Services.Configure<OptionsSetting>(builder.Configuration);

//配置文件，获取appsetting中的数据，依赖注入，定义静态方法工外界调用
builder.Services.AddSingleton(new AppSettings(builder.Configuration));

//app服务注册
builder.Services.AddAppService();
/*使用静态类扩展方法，自动注入服务，如一下代码，需要在接口中标注
"[AppService(ServiceType = typeof(ISysConfigService), ServiceLifetime = LifeTime.Transient)]"
typeof中写入接口明，serviceslifetime中标注注入方式，Transient（短暂的，无状态）
builder.Services.AddScoped<ISysConfigService, SysConfigService>();
builder.Services.AddScoped<ISysLoginService, SysLoginService>();
builder.Services.AddScoped<ISysUserService, SysUserService>();
builder.Services.AddScoped<ISysPermissionService, SysPermissionService>();
builder.Services.AddScoped<ISysRoleService, SysRoleService>();
builder.Services.AddScoped<ISysMenuService, SysMenuService>();*/

//注册REDIS 服务
var openRedis = builder.Configuration["RedisServer:open"];
if (openRedis == "1")
{
    RedisServer.Initalize();
}

//swagger扩展，文档管理
//builder.Services.AddSwaggerConfig();

// 添加本地化服务
builder.Services.AddLocalization(options => options.ResourcesPath = "");

#region 注入sqlsugar，连接 ,后续会使用特效自动注入
builder.Services.AddSqlSugar(new IocConfig()
{
    ConfigId = 0,
    ConnectionString = "Data Source=111.180.193.69;port=3306;User ID=root;Password=Mysql_998;Database=ZrAdmin;CharSet=utf8;sslmode=none;",
    DbType = IocDbType.MySql,
    IsAutoCloseConnection = true
});
#endregion




var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

