#region usings

#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using osu.Framework.Logging;
using Sym.Networking.Packets;

#endregion

#pragma warning disable 618

#endregion

namespace Sym.Networking.NetworkingClients
{
    public class TcpNetworkingClient : NetworkingClient
    {
        public const int BUFFER_SIZE = 8192 * 256;

        public virtual int PACKET_SIZE => BUFFER_SIZE / 16;

        public const int TIMEOUT = 10000;

        protected readonly TcpClient TcpClient;

        protected internal NetworkStream NetworkStream => TcpClient.GetStream();

        protected readonly TcpListener TcpListener;

        protected internal readonly List<TcpClient> TcpClients = new List<TcpClient>();

        public Action<TcpClient> OnClientConnected;

        public override int Available
        {
            get
            {
                try
                {
                    return TcpClient?.Available ?? 0;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public TcpNetworkingClient(string address)
            : base(address)
        {
            try
            {
                TcpClient = new TcpClient();
                TcpClient.Connect(EndPoint);
                TcpClient.ReceiveBufferSize = BUFFER_SIZE;
                TcpClient.ReceiveTimeout = TIMEOUT;
                Logger.Log($"No exceptions while creating peer TcpClient with address {address}!", LoggingTarget.Runtime, LogLevel.Debug);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error while setting up a new Peer TcpClient!");
                Dispose();
            }
        }

        public TcpNetworkingClient(int port)
            : base(port)
        {
            try
            {
                TcpListener = new TcpListener(port);
                TcpListener.Start();
                AcceptClient();
                Logger.Log($"No exceptions while updating server TcpListener with port {port}", LoggingTarget.Runtime, LogLevel.Debug);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error while setting up a new Server TcpListener!");
                Dispose();
            }
        }

        public void AcceptClient() => TcpListener.AcceptTcpClientAsync().ContinueWith(result =>
        {
            TcpClient client = result.Result;
            TcpClients.Add(client);
            client.SendBufferSize = BUFFER_SIZE;
            client.SendTimeout = TIMEOUT;
            OnClientConnected?.Invoke(client);
            AcceptClient();
        });

        public override void SendPacket(Packet packet, IPEndPoint end = null)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, packet);

                stream.Position = 0;

                byte[] data = new byte[PACKET_SIZE];

                stream.Read(data, 0, PACKET_SIZE);

                SendBytes(data, end);
            }
        }

        /// <summary>
        /// Receive a packet
        /// </summary>
        /// <returns></returns>
        public virtual Packet GetPacket(TcpClient client)
        {
            this.client = client;
            return GetPacket();
        }

        public override void SendBytes(byte[] bytes, IPEndPoint end)
        {
            if (end != null)
            {
                foreach (TcpClient client in TcpClients)
                    if (client.Client.LocalEndPoint.ToString() == end.ToString())
                    {
                        send(client.GetStream(), bytes);
                        break;
                    }
            }
            else
            {
                if (!TcpClient.Client.Connected)
                {
                    Logger.Log($"TcpClient is not connected to {EndPoint.Address}!", LoggingTarget.Network, LogLevel.Error);
                    return;
                }

                send(NetworkStream, bytes);
            }

            void send(NetworkStream stream, byte[] data)
            {
                try
                {
                    stream.Write(data, 0, PACKET_SIZE);
                    Logger.Log($"No exceptions while sending bytes to {EndPoint.Address}", LoggingTarget.Runtime, LogLevel.Debug);
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Error sending bytes!");
                }
            }
        }

        private TcpClient client;

        public override byte[] GetBytes()
        {
            if (client == null) client = TcpClient;

            byte[] data = new byte[PACKET_SIZE];
            client.GetStream().Read(data, 0, data.Length);

            client = null;

            return data;
        }

        public override void Dispose()
        {
            TcpClient?.Close();
            TcpClient?.Dispose();
            TcpListener?.Stop();
            foreach (TcpClient client in TcpClients)
            {
                client.Close();
                client.Dispose();
            }
        }
    }
}
