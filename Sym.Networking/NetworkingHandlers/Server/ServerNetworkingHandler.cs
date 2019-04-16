#region usings

using System;
using System.Collections.Generic;
using System.Net;
using osu.Framework.Logging;
using Sym.Networking.NetworkingClients;
using Sym.Networking.Packets;

#endregion

namespace Sym.Networking.NetworkingHandlers.Server
{
    public class ServerNetworkingHandler : ServerNetworkingHandler<Peer>
    {
        protected override Peer GetClient(IPEndPoint end) => new Peer(end);
    }

    public abstract class ServerNetworkingHandler<T> : NetworkingHandler<T>
        where T : Peer
    {
        #region Fields

        protected virtual double TimeOutTime => 60000;

        /// <summary>
        /// Call this when we connect to a Host
        /// </summary>
        public Action<Peer> OnPeerConnected;

        /// <summary>
        /// All Connecting peers / peers losing connection
        /// </summary>
        public readonly List<T> Peers = new List<T>();

        #endregion

        protected ServerNetworkingHandler()
        {
            OnAddressChange += (ip, port) =>
            {
                if (Udp)
                    UdpNetworkingClient = new UdpNetworkingClient(port);

                TcpNetworkingClient = new TcpNetworkingClient(Port);
                TcpNetworkingClient.OnClientConnected += value =>
                {
                    string[] address = value.Client.RemoteEndPoint.ToString().Split(':');

                    T t = GetClient(new IPEndPoint(IPAddress.Parse(address[0]), int.Parse(address[1])));

                    t.LastConnectionTime = Time.Current;
                    t.Statues = ConnectionStatues.Connected;

                    t.TcpClient = value;
                    Peers.Add(t);
                    SendToPeer(new ConnectedPacket(), t);
                    OnPeerConnected?.Invoke(t);
                };
            };
            OnUdpChange += value =>
            {
                if (value)
                    UdpNetworkingClient = new UdpNetworkingClient(Port);
                else
                {
                    UdpNetworkingClient.Dispose();
                    UdpNetworkingClient = null;
                }
            };
        }

        #region Update Loop

        protected override void Update()
        {
            base.Update();

            CheckClients();
        }

        /// <summary>
        /// Handle any packets we got before sending them to OnPackerReceive
        /// </summary>
        /// <param name="info"></param>
        protected override void PacketReceived(PacketInfo<T> info)
        {
            switch (info.Packet)
            {
                case DisconnectPacket disconnectPacket:
                    ClientDisconnecting(info);
                    break;
                case TestPacket testPacket:
                    if (info.Client != null)
                    {
                        info.Client.LastConnectionTime = Time.Current;
                        info.Client.ConnectionTryCount = 0;
                        info.Client.Statues = ConnectionStatues.Connected;
                        break;
                    }
                    Logger.Log("Recieved a test packet from a client we have never seen!", LoggingTarget.Network, LogLevel.Error);
                    break;
            }
        }

        protected virtual void CheckClients()
        {
            foreach (T t in Peers)
            {
                if (t.LastConnectionTime + TimeOutTime / 6 <= Time.Current && t.ConnectionTryCount == 0)
                    TestConnection(t);

                if (t.LastConnectionTime + TimeOutTime / 3 <= Time.Current && t.ConnectionTryCount == 1)
                    TestConnection(t);

                if (t.LastConnectionTime + TimeOutTime / 2 <= Time.Current && t.ConnectionTryCount == 2)
                    TestConnection(t);

                if (t.LastConnectionTime + TimeOutTime <= Time.Current)
                {
                    t.Statues = ConnectionStatues.Disconnected;
                    break;
                }
            }
        }

        #endregion

        #region Packet and Client Helper Functions

        protected override List<PacketInfo<T>> GetPackets()
        {
            List<PacketInfo<T>> packets = new List<PacketInfo<T>>();
            foreach (T t in Peers)
            {
                restart:

                CheckQueue(t.TcpClient, t.NextPacketSize, out int s);
                t.NextPacketSize = s;

                if (read(t)) goto restart;
            }

            return packets;

            bool read(T t)
            {
                if (t.NextPacketSize > 0 && t.TcpClient.Available >= t.NextPacketSize)
                {
                    packets.Add(new PacketInfo<T>(t, TcpNetworkingClient.GetPacket(t.TcpClient.GetStream(), t.NextPacketSize)));
                    t.NextPacketSize = 0;
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Called to remove a client that is disconnecting
        /// </summary>
        /// <param name="info"></param>
        protected void ClientDisconnecting(PacketInfo<T> info) => info.Client.Statues = ConnectionStatues.Disconnected;

        #endregion

        #region Send Functions

        protected virtual void SendToPeer(Packet packet, T t) => TcpNetworkingClient.SendPacket(SignPacket(packet), t.EndPoint);

        protected void ShareWithAllClients(Packet packet)
        {
            ShareWithAllConnectingClients(packet);
            ShareWithAllConnectedClients(packet);
        }

        protected void ShareWithAllConnectingClients(Packet packet)
        {
            foreach (T t in Peers)
                if (t.Statues == ConnectionStatues.Connecting)
                    TcpNetworkingClient.SendPacket(SignPacket(packet), t.EndPoint);
        }

        protected void ShareWithAllConnectedClients(Packet packet)
        {
            foreach (T t in Peers)
                if (t.Statues == ConnectionStatues.Connected)
                    TcpNetworkingClient.SendPacket(SignPacket(packet), t.EndPoint);
        }

        /// <summary>
        /// Test a clients connection
        /// </summary>
        protected virtual void TestConnection(Client client)
        {
            client.ConnectionTryCount++;
            TcpNetworkingClient.SendPacket(SignPacket(new TestPacket()), client.EndPoint);
        }

        #endregion

        protected override void Dispose(bool isDisposing)
        {
            ShareWithAllClients(new ServerClosingPacket());
            base.Dispose(isDisposing);
        }
    }
}
