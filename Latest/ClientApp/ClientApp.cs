using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class UdpClientApp
{
    static UdpClient udpClient;
    static Thread udpThread;

    static void Main()
    {
        // Start the UDP listener
        StartUdpListener();

        // Start the TCP listener in a separate thread
        Thread tcpThread = new Thread(ListenForTcpMessages);
        tcpThread.Start();

        // Keep the main thread alive to handle both UDP and TCP listeners
        while (true)
        {
            Thread.Sleep(1000);
        }
    }

    // Function to handle UDP listening
    static void StartUdpListener()
    {
        if (udpClient != null)
        {
            udpClient.Close();
        }

        udpClient = new UdpClient(9802);
        Console.WriteLine("UDP client listening on port 9801...");
        var remoteEndPoint = new IPEndPoint(IPAddress.Any, 9802);
        udpThread = new Thread(() =>
        {
            while (true)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref  remoteEndPoint);
                    Console.WriteLine($"UDP Received: {Encoding.UTF8.GetString(data)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error receiving UDP data: " + ex.Message);
                }
            }
        })
        {
            IsBackground = true
        };
        udpThread.Start();
    }

    // Function to handle TCP listening and reset UDP connection when a message is received
    static void ListenForTcpMessages()
    {
        TcpListener tcpListener = new TcpListener(IPAddress.Parse("0.0.0.0"), 9803);
        tcpListener.Start();
        Console.WriteLine("TCP client listening on port 9803...");

        while (true)
        {
            try
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                NetworkStream stream = tcpClient.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"TCP Received: {message}");

                    // Stop and restart UDP listener
                    Console.WriteLine("Stopping UDP reception and resetting...");
                    StartUdpListener();
                }

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling TCP connection: " + ex.Message);
            }
        }
    }
}
