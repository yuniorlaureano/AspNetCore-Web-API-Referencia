using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

namespace CityInfo.API
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Contract resorver, json serialization
            //services.AddMvc()
            //        .AddJsonOptions(options => 
            //        {
            //            if (options.SerializerSettings.ContractResolver != null)
            //            {
            //                var castedResorver = options.SerializerSettings.ContractResolver
            //                as DefaultContractResolver;
            //                castedResorver.NamingStrategy = null;
            //            }
            //        });

            services.AddMvc()
                    .AddMvcOptions(options => 
                    {
                        options.OutputFormatters.Add(
                            new XmlDataContractSerializerOutputFormatter());
                    });

           
            //Data Source=(localdb)\ProjectsV13;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
            services.AddDbContext<CityInfoContext>(options => 
            {
                options.UseSqlServer(_configuration.GetConnectionString("InfoCity"));
            });

            services.AddTransient<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoContext cityInfoContext)
        {
            // No need to add these loggers in ASP.NET Core 2.0: the call to WebHost.CreateDefaultBuilder(args) 
            // in the Program class takes care of that.

            //loggerFactory.AddConsole();

            loggerFactory.AddDebug(LogLevel.Information);

            //loggerFactory.AddProvider(new NLogLoggerProvider());
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            cityInfoContext.EnsureSeedDataForContext();

            AutoMapper.Mapper.Initialize(config => 
            {
                config.CreateMap<City, CityWithoutPointOfInterest>();
                config.CreateMap<City, CityDto>();
                config.CreateMap<PointOfInterest, PointOfInterestDto>();
                config.CreateMap<PointOfInterestDto, PointOfInterest>();
            });

            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
