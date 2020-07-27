using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Rewrite;
using System.Reflection.Emit;

namespace KIK
{
    public class Program
    {
        //webHost = new WebHostBuilder();
        //WebHost.CaptureStartupErrors(true);
        
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();

            //CreateHostBuilder(args).Build().Run();
            //Check(args);
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).CaptureStartupErrors(true)
            .UseStartup<Startup>()
            .PreferHostingUrls(true)
            .UseUrls("http://localhost:5000")
            .Build();


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseWebRoot("http://localhost:44325");
                });

        public static void Check(string[] args)
        {
            WebHost.CreateDefaultBuilder(args).CaptureStartupErrors(true);
        }
    }
}
