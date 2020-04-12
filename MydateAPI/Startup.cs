using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MydateAPI.Data;
using MydateAPI.Helpers;
using MydateAPI.Repositories;
using MydateAPI.Repositories.Interfaces;
using MydateAPI.Repositories.Repo;

namespace MydateAPI
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
            services.AddDbContext<DataContext>(x => x.UseSqlite
            (Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers().AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors();

            // CORS Policy definition.
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("DefaultPolicy",
            //        builder =>
            //        builder.WithOrigins("http://localhost:4200",
            //                            "https://localhost:4200",
            //                            "http://localhost:5000",
            //                            "https://localhost:5000",
            //                            "35.167.74.121",
            //                            "35.166.202.113",
            //                            "35.160.3.103",
            //                            "54.183.64.135",
            //                            "54.67.77.38",
            //                            "54.67.15.170",
            //                            "54.183.204.205",
            //                            "35.171.156.124",
            //                            "18.233.90.226",
            //                            "3.211.189.167")
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //}
            //);

            services.AddAutoMapper(typeof(MyDateRepository).Assembly);

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMyDateRepository, MyDateRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
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
                app.UseExceptionHandler(builder => {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }
            //app.UseHttpsRedirection();

            app.UseCors(X => X.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            //app.UseCors(x => x.WithOrigins("http://localhost:4200")
            //    .AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            //app.UseCors("DefaultPolicy");

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
