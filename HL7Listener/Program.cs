using System;
using System.Net;
using System.Threading;

namespace HL7Listener
{
    class Program
    {
        private static readonly byte[] Localhost = { 127, 0, 0, 1 };
        private const int Port = 1080;

        static void Main(string[] args)
        {
            System.Net.IPAddress address = new IPAddress(Localhost);
            System.Net.IPEndPoint endPoint = new IPEndPoint(address, Port);

            try
            {
                // Create a thread for listening to a port.
                Subscriber subscriber = new Subscriber(endPoint);
                System.Threading.Thread listnerThread = new Thread(new ThreadStart(subscriber.Listen));
                listnerThread.Start();
            }
            catch (Exception e)
            {
                // Exception handling
                Console.WriteLine("An unexpected exception occured: {0}", e.Message);
            }
        }
    }
}
