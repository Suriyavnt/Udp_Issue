using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpControlApp
{
    static void Main()
    {
        TcpClient tcpClient = new TcpClient("127.0.0.1", 7803);
        NetworkStream stream = tcpClient.GetStream();

        while (true)
        {
            string message = "Reset UDP connection";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine($"Sent message to client: {message}");

            // Wait for 5 seconds before sending the next control message
            System.Threading.Thread.Sleep(5000);
        }
    }
}
