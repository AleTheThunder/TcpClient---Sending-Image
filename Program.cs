using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TcpClient___Sending_Image
{
    class Program
    {
        static void Main()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse("127.0.0.1"), 8080);

                NetworkStream stream = client.GetStream();

                // Bildgröße empfangen
                byte[] sizeBytes = new byte[4];
                int bytesRead = stream.Read(sizeBytes, 0, sizeBytes.Length);
                int imageSize = BitConverter.ToInt32(sizeBytes, 0);

                // Bild empfangen
                byte[] imageBytes = new byte[imageSize];
                bytesRead = 0;
                while (bytesRead < imageSize)
                {
                    bytesRead += stream.Read(imageBytes, bytesRead, imageSize - bytesRead);
                }

                // Bild speichern
                File.WriteAllBytes("received_image.jpg", imageBytes);

                // Bild öffnen
                Process.Start("explorer.exe", "received_image.jpg");

                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
