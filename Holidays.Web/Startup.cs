using Holidays.Contracts;
using Holidays.Nager.Date;
using Holidays.Services;
using Holidays.Web.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Holidays.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<NagerDateClientOptions>();
            services.AddSingleton<NagerDateClient>();
            services.AddSingleton<IHolidaysProvider, NagerDateCachedClient>();
            services.AddTransient<IHolidaysService, HolidaysService>();

            services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UsePathBase(new PathString("/api"));
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
