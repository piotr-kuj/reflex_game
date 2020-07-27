using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KIK.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KIK.Extensions;
using Microsoft.AspNetCore.Rewrite;
using javax.jws;
using System.Reflection.Emit;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using KIK.Options;
using KIK.Hubs;
using Microsoft.AspNetCore.Http.Connections;

namespace KIK
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Localization");
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
                options => options.ResourcesPath = "Localization").AddDataAnnotationsLocalization();
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddApplicationInsightsTelemetry();
            //obsluga email
            services.Configure<EmailServiceOptions>(_configuration.GetSection("Email"));
            
            //obslugiwanie serwisow uzytkownika i email
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IEmailService, EmailService>();

            // localhostowo zarzadza cache'em
            services.AddDistributedMemoryCache();


            // local file for everyone
            services.AddDirectoryBrowser();
            services.AddRouting();

            services.AddRazorPages();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            // dodanie kanalu komunikacji
            services.AddSignalR();

            services.AddSession(o =>
           {
               o.IdleTimeout = TimeSpan.FromMinutes(30);
           }
            );
        }
        public CultureInfo culture;// = CultureInfo.CurrentCulture;
        public IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // to share files
            app.UseStaticFiles();
            //
            app.UseSession();


            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("CreateUser", context =>
            {
                var firstName = context.Request.Query["firstName"];
                var lastName = context.Request.Query["lastName"];
                var email = context.Request.Query["email"];
                var password = context.Request.Query["password"];
                var userService = context.RequestServices.GetService<IUserService>();
                userService.RegisterUser(new Model.UserModel { FirstName = firstName, LastName = lastName, Email = email, Password = password });
                return context.Response.WriteAsync($"Uzytkownik {firstName} {lastName} zostal utworzony.");
                /*
                culture = CultureInfo.CurrentCulture;
                if (culture.EnglishName.ToLower().Contains("polish"))
                {
                }
                else
                { 
                    return context.Response.WriteAsync($"User {firstName} {lastName} has been created.");
                }
                */
            });

            var newUserRoutes = routeBuilder.Build();
            app.UseRouter(newUserRoutes);

            app.UseWebSockets();
            // communication
            app.UseCommunicationMiddleware();
            // using .. share file with any - OK 
            app.UseDirectoryBrowser();
            

            // using .. routing  
            app.UseRouting();
            app.UseEndpoints(ep =>
            {
                ep.MapRazorPages();

                ep.MapHub<SignalCommunication>("/chatHub");
                /*
                ep.MapHub<SignalCommunication>("/signalcommunication", option =>
                {
                    option.Transports = HttpTransportType.LongPolling;
                });
                */
            });


            var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pl-PL"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };


            app.UseRequestLocalization(localizationOptions);
            app.UseMvc(env =>
            {
                env.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                
                
            });

            app.UseStatusCodePages("text/plain", "Blad HTTP - kod odpowiedzi: {0}");

            var options = new RewriteOptions().AddRewrite("NewUser", "/UserRegistration/Index", false);
            var options2 = new RewriteOptions().AddRewrite("NewUser", "/UserRegistration", false);
            var options3 = new RewriteOptions().AddRewrite("NewUser", "/UseRegistration/EmailConfirmation", false);
            app.UseRewriter(options); 
            app.UseRewriter(options2);
            app.UseRewriter(options3);

        }


        private static void ApiPipeline(IApplicationBuilder app)
        {

        }

    }
}
