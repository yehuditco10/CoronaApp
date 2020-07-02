using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CoronaApp.Dal;
using Microsoft.EntityFrameworkCore;
using CoronaApp.Services;
using CoronaApp.Api.Middleware;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Reflection;
using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CoronaApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<CoronaContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("coronaConnection")));
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            // services.AddScoped<IUserService, UserService>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                       builder =>
                       {
                           builder.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .WithExposedHeaders("X-Pagination");
                       });
            });
        
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            var appSettingsSection = Configuration.GetSection("AppSettings").GetSection("Secret");
            //services.Configure<AppSetting>(appSettingsSection);

            var key = Encoding.ASCII.GetBytes(appSettingsSection.Value);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("CoronaAppOpenApiSpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "CoronaApp",
                        Version = "1",
                        Description = "Through this api you can get the corona locations",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Name = "Yehudit Cohen",
                            Email = "cyehudit10@gmail.com"
                        }
                    });
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                setupAction.IncludeXmlComments(xmlCommentFullPath);
            });
            // configure DI for application services
            //services.AddScoped<IUserService, UserService>();
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
                app.UseExceptionHandler("");//"/error"
                //securing (https)
                app.UseHsts();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAllOrigins");
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/CoronaAppOpenApiSpecification/swagger.json",
                    "CoronaApp");

                //in the beggining
               // setupAction.RoutePrefix = "";
            });
            app.UseStaticFiles();
           // app.UseMvc();


            //middelware
            //app.Use(async (context, next) =>
            //{
            //    // Do work that doesn't write to the Response.
            //    await next.Invoke();
            //    // Do logging or other work that doesn't write to the Response.
            //});
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello, World!");
            //});
        }
    }
}
