using Keefa1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using keef2.Hub_Config;
using keef2.Models;

namespace keef2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        //CORS ProblemSolution
        //readonly string AllowedSpecificOrigins = "start";
       public string mypolicy = "mypolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                       options.AddPolicy(name: mypolicy,
                           builder =>
                           {
                               builder.AllowAnyHeader().AllowAnyMethod()
                                   .AllowAnyOrigin();
                           })); //add this
            services.AddDbContextPool<DocContext>(
                            options => options.UseSqlServer(Configuration.GetConnectionString("Cs")));
            services.AddMvc().AddNewtonsoftJson();

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddControllers();
            #region comment
            //    services.AddMvc()
            //     .AddJsonOptions(o =>
            //     {
            //         o.JsonSerializerOptions
            //.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            //     });

            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(builder =>
            //    {
            //        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowAnyMethod().Build();
            //    });
            //});
            #endregion


            services.AddControllersWithViews();
            services.AddDbContext<DocContext>();
            services.AddIdentity<AccountUser, IdentityRole>(options=>
             { options.User.RequireUniqueEmail = true; }).AddEntityFrameworkStores<DocContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
          //  app.UseCors("");
            
            app.UseCors(mypolicy); //add this

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ReviewHub>("/reviewHub");
            });
            //app.UseMvc();

        }
    }
}
