using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Vyayaam.Services;
using Vyayaam.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using AutoMapper;

namespace Vyayaam
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //************************************************************************************************
        // We cant just inject DB Seeder in here as DB Context is added in SCOPED Service(are cached for single request) but as this is(Seeder) is lightest possible service
        // we want it to be transient(not cached just created used and destroyed). Now the problem is that we cant inject it in Configuew coz all should be of same scope 
        // i.e. all injected dependencies or services should be of same type, here scoped else it will give error.
        //************************************************************************************************
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<VyayaamContext>(config =>
            {
                config.UseSqlServer(_config.GetConnectionString("VyayaamConnectionString"));
            });

            services.AddAutoMapper();

            services.AddTransient<IMailService, NullMailService>();
            /*inject real messaging/mail services here
             services.AddTransient<IMailService, EMailService>();
             services.AddTransient<IMailService, SmsService>();*/
            services.AddTransient<VyayaamSeeder>();

            services.AddScoped<IVyayaamRepository, VyayaamRepository>();


            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseDefaultFiles();
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Errors");
            }

            app.UseMvc(config =>
                config.MapRoute("default",
                "{controller}/{action}/{id?}",
                new { controller = "UserManagement", Action = "Index" })
            );

            if (env.IsDevelopment())
            {
                /*
                 Now we can't create an instance of Seeder as DBContext is required 
                 which in turn can be retrieved via current "Scope"
                */
                //var seeder = new VyayaamSeeder();
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    /*we use using() here so that we create this scope 
                     * and then deallocate it as soon as seeding is done
                     as this is expensive operation but ok for Dev env.*/
                    var seeder = scope.ServiceProvider.GetService<VyayaamSeeder>();
                    seeder.Seed();
                }
            }
        }
    }
}
