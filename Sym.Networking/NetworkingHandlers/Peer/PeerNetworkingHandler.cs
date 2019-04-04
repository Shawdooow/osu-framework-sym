#region usings

using System;
using System.Collections.Generic;
using osu.Framework.Logging;
using Sym.Networking.NetworkingClients;
using Sym.Networking.Packets;

#endregion

namespace Sym.Networking.NetworkingHandlers.Peer
{
    public class PeerNetworkingHandler : NetworkingHandler
    {
        #region Fields

        /// <summary>
        /// Call this when we connect to a Host
        /// </summary>
        public Action<Host> OnConnectedToHost;

        public ConnectionStatues ConnectionStatues { get; protected set; }

        #endregion

        public PeerNetworkingHandler()
        {
            OnAddressChange += (ip, port) =>
            {
                UdpNetworkingClient = new UdpNetworkingClient(ip + ":" + port);

                if (Tcp)
                    TcpNetworkingClient = new TcpNetworkingClient(ip + ":" + port);
            };
            OnTcpChange += value =>
            {
                if (value)
                {
                    TcpNetworkingClient = new TcpNetworkingClient(IP + ":" + Port);
                    SendToServer(new TcpPacket());
                }
                else
                {
                    TcpNetworkingClient.Dispose();
                    TcpNetworkingClient = null;
                }
            };
        }

        #region Update Loop

        /// <summary>
        /// Handle any packets we got before sending them to OnPackerReceive
        /// </summary>
        /// <param name="info"></param>
        protected override void HandlePackets(PacketInfo info)
        {
            Logger.Log($"Recieved a Packet from {UdpNetworkingClient.EndPoint}", LoggingTarget.Network, LogLevel.Debug);

            switch (info.Packet)
            {
                case ConnectedPacket _:
                    ConnectionStatues = ConnectionStatues.Connected;
                    Logger.Log("Connected to server!", LoggingTarget.Network);
                    OnConnectedToHost?.Invoke(new Host());
                    break;
                case DisconnectPacket _:
                    Logger.Log("Server shutting down!", LoggingTarget.Network);
                    break;
                case TestPacket _:
                    SendToServer(new TestPacket());
                    break;
            }

            OnPacketReceive?.Invoke(info);
        }

        #endregion

        #region Packet and Client Helper Functions

        protected override List<PacketInfo> ReceivePackets()
        {
            List<PacketInfo> packets = new List<PacketInfo>();
            for (int i = 0; i < UdpNetworkingClient?.Available; i++)
                packets.Add(new PeerPacketInfo
                {
                    Packet = UdpNetworkingClient.GetPacket()
                });
            return packets;
        }

        protected override Packet SignPacket(Packet packet)
        {
            if (packet is ConnectPacket c)
                c.Gamekey = Gamekey;
            return packet;
        }

        #endregion

        #region Send Functions

        public virtual void SendToServer(Packet packet)
        {
            UdpNetworkingClient.SendPacket(SignPacket(packet));
        }

        #endregion

        #region Network Actions

        /// <summary>
        /// Starts the connection proccess to Host / Server
        /// </summary>
        public virtual void Connect()
        {
            // ReSharper disable once InvertIf
            if (true)//ConnectionStatues <= ConnectionStatues.Disconnected)
            {
                Logger.Log($"Attempting to connect to {UdpNetworkingClient.Address}", LoggingTarget.Network);
                SendToServer(new ConnectPacket());
                ConnectionStatues = ConnectionStatues.Connecting;
            }
            //else
                //Logger.Log("We are already connecting, please wait for us to fail before retrying!", LoggingTarget.Network);
        }

        public virtual void Disconnect()
        {
            if (ConnectionStatues >= ConnectionStatues.Connecting)
                SendToServer(new DisconnectPacket());
            else
                Logger.Log("We are not connected!", LoggingTarget.Network);
        }

        #endregion

        protected override void Dispose(bool isDisposing)
        {
            SendToServer(new DisconnectPacket());

            base.Dispose(isDisposing);
        }
    }
}
