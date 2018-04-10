using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;


namespace TextAdventure.Server
{
    class WebsocketUtility
    {
        public static Byte[] encodeWebsocketMessage(Byte[] rb) //Raw bytes
        {
            int l = rb.Length;
            Byte[] eb; //Encoded bytes
            int indexStartRawData = -1;
            if (rb.Length < 124)
            {
                eb = new Byte[rb.Length + 2];
                eb[1] = (Byte)l;
                indexStartRawData = 2;
            }
            else if (rb.Length >= 126 && rb.Length <= 65535)
            {
                eb = new Byte[rb.Length + 4];
                eb[1] = 126;
                eb[2] = (Byte)((l >> 8) & 255);
                eb[3] = (Byte)((l) & 255);
                indexStartRawData = 4;
            }
            else
            {
                eb = new Byte[rb.Length + 10];
                eb[1] = 127;
                eb[2] = (Byte)((l >> 56) & 255);
                eb[3] = (Byte)((l >> 48) & 255);
                eb[4] = (Byte)((l >> 40) & 255);
                eb[5] = (Byte)((l >> 32) & 255);
                eb[6] = (Byte)((l >> 24) & 255);
                eb[7] = (Byte)((l >> 16) & 255);
                eb[8] = (Byte)((l >> 8) & 255);
                eb[9] = (Byte)((l) & 255);
                indexStartRawData = 10;
            }
            eb[0] = 129;
            rb.CopyTo(eb, indexStartRawData);
            return eb;
        }

        public static Byte[] decodeWebsocketMessage(Byte[] eb) //encoded bytes
        {
            Byte secondByte = eb[1];
            int length = secondByte & 127;
            int indexFirstMask = 2;

            if (length == 126)
            {
                indexFirstMask = 4;
            }

            else if (length == 127)
            {
                indexFirstMask = 10;
            }

            Byte[] masks = { eb[indexFirstMask], eb[indexFirstMask + 1], eb[indexFirstMask + 2], eb[indexFirstMask + 3] };

            Byte[] db = new Byte[eb.Length - indexFirstMask]; //Decoded bytes
            Console.WriteLine("Decoded was of length:" + (eb.Length - indexFirstMask));

            for (int i = indexFirstMask, j = 0; i < eb.Length; i++, j++)
            {
                db[j] = (Byte)(eb[i] ^ masks[j % 4]);
            }
            Byte[] adjustedReturn = new Byte[db.Length - 4]; //Always has four zeros in it
            Array.Copy(db, 4, adjustedReturn, 0, adjustedReturn.Length);
            return adjustedReturn;
        }

        public static void websocketHandshake(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            while (!stream.DataAvailable) ;
            Byte[] bytes = new Byte[client.Available];
            stream.Read(bytes, 0, bytes.Length);

            String data = Encoding.UTF8.GetString(bytes);
            if (new Regex("^GET").IsMatch(data))
            {
                Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                    + "Connection: Upgrade" + Environment.NewLine
                    + "Upgrade: websocket" + Environment.NewLine
                    + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                        SHA1.Create().ComputeHash(
                            Encoding.UTF8.GetBytes(

                                new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                            )
                        )
                    ) + Environment.NewLine
                    + Environment.NewLine);

                stream.Write(response, 0, response.Length);
            }
        }

        public static IPAddress getLocalIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("Local IP address not found!");
        }
    }
}
