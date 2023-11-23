using EMRReport.API.Configuration;
using EMRReport.Common.TokenManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using EMRReport.ServiceContracts.ServiceObjects;
using System;
using Serilog;
using Serilog.Events;

namespace EMRReport.API
{
    public sealed class Startup
    {
        #region Public Properties

        public IConfiguration _configuration { get; }

        #endregion

        #region Constructors & Destructor

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Methods
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCrossDomainPolicy();
            services.AddControllers(setUpAction =>
            {
                setUpAction.ReturnHttpNotAcceptable = true;
            });
            services.AddJWTAuthorization(_configuration);
            services.AddAutoMapperProfiles();
            services.AddValidators();
            services.AddServices();
            services.AddRepositories();
            services.AddSqlServerDbContext(_configuration.GetValue<string>("ConnectionStrings:ScrubberDBConnectionString"));
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("PolicyForAngular");
            app.UseSerilogRequestLogging(options =>
            {
                options.GetLevel = (ctx, elapsed, ex) =>
                {
                    if (ex != null || ctx.Response.StatusCode > 499)
                        return LogEventLevel.Error;
                    if (elapsed >= 0)
                        return LogEventLevel.Warning;
                    return LogEventLevel.Information;
                };
            });
            //app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Configuration API Services"); });
        }

        #endregion
    }
}