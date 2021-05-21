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
            ParametriBoiler.PragB4 = 10000;


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
                decimal[] y = new decimal[] { 0, 0 };
                int miliseconds = 0;
                while (true)
                {
                    if (stopwatch.ElapsedTicks * 1000 / Stopwatch.Frequency >= 100)
                    {
                        stopwatch.Stop();
                        stopwatch.Reset();
                        stopwatch.Start();
                        if (ParametriBoiler.SystemEnable == true)
                        {
                            if (ParametriBoiler.PompaG1 == true && ParametriBoiler.ValvaK1 == false)
                            {
                                if (ParametriBoiler.NivelCurent < ParametriBoiler.Capacitate)
                                {
                                    y[1] = Convert.ToDecimal(0.1) * ParametriBoiler.DebitMaxP1 * ParametriBoiler.P1 / 100 + y[0];
                                    y[0] = y[1];
                                }
                                else
                                {
                                    ParametriBoiler.PompaG1 = false;
                                }
                            }
                            else if (ParametriBoiler.PompaG1 == true && ParametriBoiler.ValvaK1 == true)
                            {
                                if (((ParametriBoiler.NivelCurent < ParametriBoiler.Capacitate) || (ParametriBoiler.NivelCurent >= ParametriBoiler.Capacitate && ParametriBoiler.DebitMaxP1 * ParametriBoiler.P1 < ParametriBoiler.DebitMaxP2 * ParametriBoiler.P2)) && ((ParametriBoiler.NivelCurent > ParametriBoiler.PragB1) || (ParametriBoiler.NivelCurent <= ParametriBoiler.PragB1 && ParametriBoiler.DebitMaxP1* ParametriBoiler.P1 > ParametriBoiler.DebitMaxP2 * ParametriBoiler.P2)))
                                {

                                    y[1] = Convert.ToDecimal(0.1) * (ParametriBoiler.DebitMaxP1 * ParametriBoiler.P1 / 100 - ParametriBoiler.DebitMaxP2 * ParametriBoiler.P2 / 100) + y[0];
                                    y[0] = y[1];
                                }
                                else
                                {
                                    //ParametriBoiler.PompaG1 = false;
                                    ParametriBoiler.ValvaK1 = false;
                                    if (y[1] < ParametriBoiler.PragB1)
                                        y[1] = ParametriBoiler.PragB1;
                                }
                            }
                        }
                        if (y[1] > ParametriBoiler.Capacitate)
                            y[1] = ParametriBoiler.Capacitate;
                        ParametriBoiler.NivelCurent = y[1];
                        miliseconds += 100;
                        if (miliseconds == 1000)
                        {
                            Debug.WriteLine(y[1]);
                            miliseconds = 0;
                        }
                    }
                }

            });
            task.Start();
        }

        private async void Initialise_Server()
        {
            TcpListener server = new TcpListener(new IPAddress(new byte[] { 0, 0, 0, 0 }), 8080);
            server.Start();
            while(true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                RunWorker(client);
            }

        }

        private async void RunWorker(TcpClient client)
        {
            string s = "";
            byte[] message = new byte[1];
            await client.GetStream().ReadAsync(message, 0, 1);
            byte i = 0;
            while(message[0] != 0)
            {
                if(message[0] == 47)
                {
                    switch (i)
                    {
                        case 0: ParametriBoiler.Capacitate = Convert.ToDecimal(s);
                                break;
                        case 1: ParametriBoiler.DebitMaxP1 = Convert.ToDecimal(s);
                                break;
                        case 2: ParametriBoiler.DebitMaxP2 = Convert.ToDecimal(s);
                                break;
                        case 3: ParametriBoiler.PragB1 = Convert.ToDecimal(s);
                                break;
                        case 4: ParametriBoiler.PragB2 = Convert.ToDecimal(s);
                                break;
                        case 5: ParametriBoiler.PragB3 = Convert.ToDecimal(s);
                                break;
                        case 6: ParametriBoiler.PragB4 = Convert.ToDecimal(s);
                                break;

                    }
                    i++;
                    s = "";
                }
                else
                    s += (char)message[0];
                await client.GetStream().ReadAsync(message, 0, 1);
            }
            await client.GetStream().WriteAsync(new byte[] { 1 }, 0, 1);
            client.Dispose();
        }
    }
}
