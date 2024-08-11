using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using YShine.Tools;

namespace YShine.Extension
{
	public static class SwaggerExtension
    {
		public static void UseSwagger(this IApplicationBuilder app)
		{
			app.UseSwagger(x =>
			{
				x.RouteTemplate = "swagger/{documentName}/swagger.json";
				x.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
				{
					var url = $"{httpReq.Scheme}:{httpReq.Host.Value}";
					var regerer = httpReq.Headers["Referer"].ToString();
					if (regerer.Contains(GlobalConstant.DevApiProxy))
					{
						url = regerer[..(regerer.IndexOf(GlobalConstant.DevApiProxy, StringComparison.InvariantCulture) + GlobalConstant.DevApiProxy.Length - 1)];
						swaggerDoc.Servers =
						new List<OpenApiServer>
						{
							new OpenApiServer{Url=url}
						};
					} 
				});
			});
			app.UseSwaggerUI(x =>
			{
                x.SwaggerEndpoint("sys/swagger.json", "系统管理");
                x.SwaggerEndpoint("article/swagger.json", "文章管理");
                x.SwaggerEndpoint("other/swagger.json", "其他");
                x.DocExpansion(DocExpansion.None); //->修改界面打开时自动折叠
            });
		}
		public static void AddSwaggerConfig(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			services.AddSwaggerGen(x =>
			{
				x.SwaggerDoc("sys", new OpenApiInfo
				{
                    Title = "Api",
                    Version = "v1",
                    Description = "系统管理",
                    Contact = new OpenApiContact { Name = "ZRAdmin doc", Url = new Uri("https://www.baidu.com") }
                }) ;
                x.SwaggerDoc("article", new OpenApiInfo
                {
                    Title = "Api",
                    Version = "v1",
                    Description = "文章管理",
                    Contact = new OpenApiContact { Name = "ZRAdmin doc", Url = new Uri("https://www.baidu.com") }
                });
                x.SwaggerDoc("other", new OpenApiInfo
                {
                    Title = "Api",
                    Version = "v1",
                    Description = "其他",
                    Contact = new OpenApiContact { Name = "ZRAdmin doc", Url = new Uri("https://www.baidu.com") }
                });
              
            });
		}
	}
}

