namespace People.SelfHostedApi
{
    using System;
    using Microsoft.Owin.Hosting;

    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:3001"))
            {
                Console.WriteLine("Press any key to terminate...");
                Console.ReadLine();
            }
        }
    }
}
