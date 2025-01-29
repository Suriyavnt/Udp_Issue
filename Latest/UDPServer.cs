using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class UdpServer
{
    static void Main()
    {
        UdpClient udpServer = new UdpClient();
        IPEndPoint clientEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9803);
        udpServer.Connect(clientEndpoint);

        int counter = 0;

        while (true)
        {
            string message = $"Hello from server! Message number: {++counter}";
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpServer.Send(data, data.Length);
            Console.WriteLine($"Sent: {message}");

            // Wait for 16ms to 3 seconds before sending the next message
            Thread.Sleep(16);
        }
    }
}
