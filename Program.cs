using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TempelApp
{

    public class Program
    {

        
        public static void Main(string[] args)
        {
            if (args.Length > 0) {
                string subcmd = args[0];
                if (subcmd == "console") {
                    Console.WriteLine("Run ShowCase...");
                    ShowCase.MainAsync().Wait();
                }
            } else {
                CreateHostBuilder(args).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
