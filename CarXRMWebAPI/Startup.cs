using AutoMapper;
using CarXRMWebAPI.Factory;
using CarXRMWebAPI.Models.Attributes;
using CarXRMWebAPI.Models.Security;
using CarXRMWebAPI.Options;
using CarXRMWebAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Security.Principal;
using System.Text;

namespace CarXRMWebAPI
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        private const string SecretKey = "getthiskeyfromenvironment";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //ConnectionString = Configuration.GetSection("ConnectionStrings").GetSection("<Your DB Connection Name>").Value;

        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            //services.AddDbContext<VinExplodeContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddMvc();

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", corsBuilder.Build());
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });

            ////swagger
            var xmlPath = GetXmlCommentsPath();

            //// Register the Swagger generator, defining one or more Swagger documents - Commented out Swagger causing issues with authentication
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "XRM API", Version = "v1", Description = "XRM API for third party vendors", });
            //    //c.IncludeXmlComments(xmlPath);
            //    c.AddSecurityDefinition("Bearer", new ApiKeyScheme() { In = "header", Description = "Please paste JWT Token with Bearer + White Space + Token into field", Name = "Authorization", Type = "apiKey" });
            //});

            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.Configure<IISOptions>(options =>
            {
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;                
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
                configureOptions.RequireHttpsMetadata = false;
                configureOptions.IncludeErrorDetails = true;
            });

            //services.AddAuthorization(options =>
            //{
            //    //options.AddPolicy("ReadCustomer",
            //    //                  policy => policy.RequireClaim("UserName", "hondacust"));

            //    //options.AddPolicy("ReadCustomer",
            //    //                  policy => policy.RequireClaim("Role", "HondaSalesManager,HondaServiceManager"));
            //});

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddScoped<ClaimsUser>(provider => new ClaimsUser(provider.GetService<IPrincipal>()));
            services.AddSingleton<ILoginRepository, LoginRepository>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<IJwtToken, JwtToken>();
            services.AddSingleton<XRMApiSecurityAttribute, XRMApiSecurityAttribute>();

            //services.AddSingleton<XRMAPIAuthorizeAttribute, XRMAPIAuthorizeAttribute>();
            //services.AddSingleton<ApiAuthenticationFilter, ApiAuthenticationFilter>();            
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper();

            services.AddMvc(config =>
            {
                //only allow authenticated users
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            });

            //services.AddMvc(mvc =>
            //{
            //    mvc.Filters.Add(new XRMApiSecurityFilter(XRMRoles.HondaSalesManager, XRMAccess.Read));
            //});

            //services.AddMvc(mvc =>
            //{
            //    mvc.Filters.Add(new XRMApiSecurityAttribute(PermissionSet.Customer, Permission.ReadCustomer));
            //});

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {   
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseAuthentication();

            //app.UseWhen(context => context.Request.Path.StartsWithSegments(new Microsoft.AspNetCore.Http.PathString("/api")), branch =>
            //{
            //    IDictionary<string, object> v = app.Properties;
            //    branch.UseAuthenticateMiddleware();
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            //loggerFactory.AddLambdaLogger(Configuration.GetLambdaLoggerOptions());
            // Shows UseCors with CorsPolicyBuilder.

            //Add this before app.UseMvc
            //app.Use(async (context, next) =>
            //{
            //    if (!context.User.Identities.Any(i => i.IsAuthenticated))
            //    {
            //        //Assign all anonymous users the same generic identity, which is authenticated
            //        context.User = new ClaimsPrincipal(new GenericIdentity("anonymous"));
            //    }
            //    await next.Invoke();

            //});

            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseMvc();
            

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "XRM API V1");
            //});

            //app.UseStatusCodePagesWithReExecute("/error");
        }

        private string GetXmlCommentsPath()
        {
            var app = PlatformServices.Default.Application;
            return System.IO.Path.Combine(app.ApplicationBasePath, "WebAPI.xml");
        }
    }
}
