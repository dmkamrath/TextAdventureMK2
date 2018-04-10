using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace TextAdventure.Server.HttpServer
{
    public class SimpleHttpServer
    {
        private HttpListener listener = new HttpListener();
        private string httpResponse = new TextAdventureUserWebPage().webPage;

        public SimpleHttpServer()
        {
            int port = 80;
            listener.Prefixes.Add("http://*:" + port.ToString() + "/");
            listener.Start();
        }

        public void run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Http listening");
                try
                {
                    while (listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var context = c as HttpListenerContext;
                            try
                            {
                                string respondString = httpResponse;
                                byte[] buffer = Encoding.UTF8.GetBytes(respondString);
                                context.Response.ContentLength64 = buffer.Length;
                                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                            catch { }
                            finally
                            {
                                context.Response.OutputStream.Close();
                            }
                        }, listener.GetContext());
                    }
                }
                catch { }
            });
        }
        
        public void stop()
        {
            listener.Stop();
            listener.Close();
        }
    }
}
