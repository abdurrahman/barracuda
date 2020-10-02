using Barracuda.Application;
using Barracuda.Application.Message;
using Barracuda.Core;
using Barracuda.WebSocket.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Barracuda.WebSocket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddDbContext<BarracudaDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                    assembly => assembly.MigrationsAssembly(typeof(BarracudaDbContext).Assembly.FullName)));
            
            services.AddScoped(typeof(IRepository<>), typeof(BarracudaEfRepository<>));
            services.AddScoped(typeof(IRepository<,>), typeof(BarracudaEfRepository<,>));
            services.AddSingleton<IObjectMapper, MapsterObjectMapper>();
            services.AddScoped<IMessageService, MessageService>();
            
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            services.AddSignalR();
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapHub<MessageHub>("/messagehub"); });
            app.UseMvc();
        }
    }
}