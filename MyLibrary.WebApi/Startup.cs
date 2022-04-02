using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyLibrary.Business.Filter;
using MyLibrary.WebApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.WebApi
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //容器注册
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc(options =>
            {   //加入返回结果处理
                options.Filters.Add<ApiResultFilter>();
            });
            services.ConfigureCors(_defaultCorsPolicyName, Configuration);

            services.ConfigureAppSettings(Configuration);

            services.ConfigureDatabaseInitializer();

            services.ConfigureMSSQLContext(Configuration);

            services.ConfigureIdentity();

            services.ConfigurePasswordPolicy();

            services.ConfigureAuthentication(Configuration);

            services.ConfigureAuthorization();

            services.ConfigureAuthorizationHandler();

            services.ConfigureVersionedApiExplorer();

            services.ConfigureApiVersioning();

            services.ConfigureAutoMapper();

            services.ConfigureServices();

            services.ConfigureRepositories();

            services.ConfigureController();

            services.ConfigureSwagger();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
        {
            app.UseApiVersioning();
            app.ConfigureCustomExceptionMiddleware();
            app.UseCors(_defaultCorsPolicyName);
            if (env.IsDevelopment())
            {
               // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    foreach (var groupName in provider.ApiVersionDescriptions.Select(x => x.GroupName))
                    {
                        c.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
                    }
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
