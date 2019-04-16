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
        //TODO: make not const
        public const int BUFFER_SIZE = 8192 * 256;

        //TODO: make not const
        public const int MAX_PACKET_SIZE = BUFFER_SIZE / 16;

        //TODO: make not const
        public const int TIMEOUT = 60000;

        protected internal readonly TcpClient TcpClient;

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

        public int NextPacketSize { get; protected internal set; }

        public TcpNetworkingClient(string address)
            : base(address)
        {
            try
            {
                TcpClient = new TcpClient();
                TcpClient.Connect(EndPoint);
                TcpClient.ReceiveBufferSize = BUFFER_SIZE;
                TcpClient.SendBufferSize = BUFFER_SIZE / 4;
                TcpClient.ReceiveTimeout = TIMEOUT;
                TcpClient.SendTimeout = TIMEOUT / 2;
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
            if (Disposed) return;

            TcpClient c = result.Result;
            TcpClients.Add(c);

            c.ReceiveBufferSize = BUFFER_SIZE / 2;
            c.SendBufferSize = BUFFER_SIZE / 4;
            c.ReceiveTimeout = TIMEOUT;
            c.SendTimeout = TIMEOUT  / 2;

            OnClientConnected?.Invoke(c);
            AcceptClient();
        });

        /// <summary>
        /// Receive a packet
        /// </summary>
        /// <returns></returns>
        public virtual Packet GetPacket(TcpClient client)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] data = GetBytes(client, NextPacketSize);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;

                NextPacketSize = 0;

                BinaryFormatter formatter = new BinaryFormatter();

                if (formatter.Deserialize(stream) is Packet packet)
                    return packet;

                throw new NullReferenceException("Whatever we recieved isnt a packet!");
            }
        }

        public override Packet GetPacket() => GetPacket(TcpClient);

        public override void SendBytes(byte[] bytes, IPEndPoint end)
        {
            if (end != null)
            {
                foreach (TcpClient c in TcpClients)
                    if (c.Client.RemoteEndPoint.ToString() == end.ToString())
                    {
                        send(c.GetStream(), bytes, c.Client.RemoteEndPoint);
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

            void send(NetworkStream stream, byte[] data, EndPoint e = null)
            {
                try
                {
                    byte[] intBytes = BitConverter.GetBytes(data.Length);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(intBytes);
                    byte[] result = intBytes;

                    stream.Write(result, 0, 4);

                    //TODO: Determine if an offset of 4 is needed
                    stream.Write(data, 0, data.Length);

                    string address = e != null ? e.ToString() : EndPoint.ToString();
                    Logger.Log($"No exceptions while sending bytes to {address}", LoggingTarget.Runtime, LogLevel.Debug);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error sending bytes!");
                }
            }
        }

        public override byte[] GetBytes() => GetBytes(TcpClient, NextPacketSize);

        public virtual byte[] GetBytes(TcpClient client, int s)
        {
            byte[] data = new byte[s];
            client.GetStream().Read(data, 0, data.Length);

            return data;
        }

        public override void Dispose()
        {
            base.Dispose();
            TcpClient?.Close();
            TcpClient?.Dispose();
            TcpListener?.Stop();
            foreach (TcpClient c in TcpClients)
            {
                c.Close();
                c.Dispose();
            }
        }
    }
}
