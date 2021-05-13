using PS2IMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PS2IMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ParametriBoiler.SystemEnable = false;
            ParametriBoiler.PompaG1 = false;
            ParametriBoiler.ValvaK1 = false;
            ParametriBoiler.P1 = 0;
            ParametriBoiler.P2 = 0;
            ParametriBoiler.Capacitate = 10000;
            ParametriBoiler.DebitMaxP1 = 100;
            ParametriBoiler.DebitMaxP2 = 100;
            ParametriBoiler.NivelCurent = 0;
            ParametriBoiler.PragB1 = 1000;
            ParametriBoiler.PragB2 = 2000;
            ParametriBoiler.PragB3 = 3000;
            ParametriBoiler.PragB4 = 4000;
            ParametriBoiler.PragB5 = 5000;



            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Initialise_Server();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            timer_10ms(stopwatch);
        }

        private void timer_10ms(Stopwatch stopwatch)
        {
            Task task = new Task(() => {
                while(true)
                {
                    Debug.WriteLine(stopwatch.ElapsedTicks*1000/Stopwatch.Frequency);
                    if (stopwatch.ElapsedTicks * 1000 / Stopwatch.Frequency >= 1000)
                    {
                        stopwatch.Stop();
                        stopwatch.Reset();
                        Debug.WriteLine("1 second passed");
                        stopwatch.Start();
                    }
                }

            });
            task.Start();
        }

        private async void Initialise_Server()
        {
            TcpListener server = new TcpListener(new IPAddress(new byte[] { 0, 0, 0, 0 }), 800);
            server.Start();
            while(true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                RunWorker(client);
            }

        }

        private async void RunWorker(TcpClient client)
        {
            byte[] length = new byte[1];
            await client.GetStream().ReadAsync(length, 0, 1);
            Debug.WriteLine(length[0].ToString());
            client.Dispose();
        }
    }
}
