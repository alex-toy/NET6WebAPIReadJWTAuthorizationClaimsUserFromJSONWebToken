using CustomerAPI.Models;
using CustomerAPI.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Text;

namespace CustomerAPI
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

            services.AddDbContext<AppDbContext>(options =>
            {
                string connectionString = Configuration.GetConnectionString("constring");
                options.UseSqlServer(connectionString);
            });

            AppDbContext _dbcontext = services.BuildServiceProvider().GetService<AppDbContext>();
            services.AddSingleton<IRefreshTokenGenerator>(provider => new RefreshTokenGenerator(_dbcontext));

            IConfigurationSection _jwtsetting = Configuration.GetSection("JWTSetting");
            services.Configure<JWTSetting>(_jwtsetting);

            string authkey = Configuration.GetValue<string>("JWTSetting:securitykey");

            services.AddAuthentication(item =>
            {
                item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(item =>
            {
                item.RequireHttpsMetadata = true;
                item.SaveToken = true;
                item.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authkey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("api", new OpenApiInfo()
                {
                    Description = "Customer API with curd operations",
                    Title = "Customer",
                    Version = "1"
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .WithOrigins("http://localhost:4200", "http://localhost:82")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            //app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Uploads")),
            //    RequestPath = new PathString("/Uploads")
            //});

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            //  string StringValues=string.Empty;

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    // Add CORS header to allow error message to be visible to Angular
                    if (context.Request.Headers.TryGetValue("Origin", out StringValues origin))
                    {
                        context.Response.Headers.Add("Access-Control-Allow-Origin", origin.ToString());
                    }
                });
            });

            app.UseSwaggerUI(options => options.SwaggerEndpoint("api/swagger.json", "Customer"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
