using System.Collections.Generic;
using System.Net;
using osu.Framework.Logging;
using Symcol.Networking.NetworkingClients;
using Symcol.Networking.Packets;

namespace Symcol.Networking.NetworkingHandlers.Server
{
    public class ServerNetworkingHandler: NetworkingHandler
    {
        #region Fields

        //30 Seconds by default
        protected virtual double TimeOutTime => 60000;

        /// <summary>
        /// All Connecting clients / clients losing connection
        /// </summary>
        public readonly List<Client> Clients = new List<Client>();

        protected virtual Client CreateConnectingClient(ConnectPacket connectPacket)
        {
            Client c = new Client
            {
                EndPoint = new IPEndPoint(IPAddress.Parse(NetworkingClient.EndPoint.Address.ToString()), NetworkingClient.EndPoint.Port),
                LastConnectionTime = Time.Current,
                Statues = ConnectionStatues.Connecting
            };
            c.OnDisconnected += () => Clients.Remove(c);

            return c;
        }

        #endregion

        public ServerNetworkingHandler() => OnAddressChange += (ip, port) => { NetworkingClient = new UdpNetworkingClient(port); };

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
        protected override void HandlePackets(PacketInfo info)
        {
            base.HandlePackets(info);

            ServerPacketInfo serverInfo = (ServerPacketInfo)info;

            switch (info.Packet)
            {
                case ConnectPacket connectPacket:
                    serverInfo.Client = CreateConnectingClient(connectPacket);
                    Clients.Add(serverInfo.Client);
                    SendToClient(new ConnectedPacket());
                    break;
                case DisconnectPacket disconnectPacket:
                    ClientDisconnecting(serverInfo);
                    break;
                case TestPacket testPacket:
                    if (serverInfo.Client != null)
                    {
                        serverInfo.Client.LastConnectionTime = Time.Current;
                        serverInfo.Client.ConnectionTryCount = 0;
                        serverInfo.Client.Statues = ConnectionStatues.Connected;
                        break;
                    }
                    Logger.Log("Recieved a test packet from a client we have never seen!", LoggingTarget.Network, LogLevel.Error);
                    break;
            }
        }

        protected virtual void CheckClients()
        {
            foreach (Client client in Clients)
            {
                if (client.LastConnectionTime + TimeOutTime / 6 <= Time.Current && client.ConnectionTryCount == 0)
                    TestConnection(client);

                if (client.LastConnectionTime + TimeOutTime / 3 <= Time.Current && client.ConnectionTryCount == 1)
                    TestConnection(client);

                if (client.LastConnectionTime + TimeOutTime / 2 <= Time.Current && client.ConnectionTryCount == 2)
                    TestConnection(client);

                if (client.LastConnectionTime + TimeOutTime <= Time.Current)
                {
                    client.Statues = ConnectionStatues.Disconnected;
                    break;
                }
            }
        }

        #endregion

        #region Packet and Client Helper Functions

        protected override List<PacketInfo> ReceivePackets()
        {
            List<PacketInfo> packets = new List<PacketInfo>();
            for (int i = 0; i < NetworkingClient?.Available; i++)
            {
                packets.Add(new ServerPacketInfo
                {
                    Packet = NetworkingClient.GetPacket(),
                    Client = GetClient(),
                });
            }

            return packets;
        }

        /// <summary>
        /// Get a matching client from currently connecting/connected clients
        /// </summary>
        /// <returns></returns>
        protected Client GetClient()
        {
            foreach (Client client in Clients)
                if (client.EndPoint.ToString() == NetworkingClient.EndPoint.ToString())
                    return client;
            return null;
        }

        protected override Packet SignPacket(Packet packet)
        { 
            if (packet is ConnectPacket c)
                c.Gamekey = Gamekey;
            return packet;
        }

        /// <summary>
        /// Called to remove a client that is disconnecting
        /// </summary>
        /// <param name="info"></param>
        protected void ClientDisconnecting(ServerPacketInfo info) => info.Client.Statues = ConnectionStatues.Disconnected;

        protected virtual bool HandlePacket(Packet packet)
        {
            if (GetClient() != null)
                return true;

            if (packet is ConnectPacket c && c.Gamekey == Gamekey)
                return true;

            Logger.Log("This is not a packet we should handle!", LoggingTarget.Network, LogLevel.Debug);
            return false;
        }

        #endregion

        #region Send Functions

        protected void ShareWithAllClients(Packet packet)
        {
            ShareWithAllConnectingClients(packet);
            ShareWithAllConnectedClients(packet);
        }

        protected void ShareWithAllConnectingClients(Packet packet)
        {
            foreach (Client client in Clients)
                if (client.Statues == ConnectionStatues.Connecting)
                    NetworkingClient.SendPacket(SignPacket(packet), client.EndPoint);
        }

        protected void ShareWithAllConnectedClients(Packet packet)
        {
            foreach (Client client in Clients)
                if (client.Statues == ConnectionStatues.Connected)
                    NetworkingClient.SendPacket(SignPacket(packet), client.EndPoint);
        }

        protected void SendToClient(Packet packet) => NetworkingClient.SendPacket(SignPacket(packet), GetClient().EndPoint);

        /// <summary>
        /// Test a clients connection
        /// </summary>
        protected virtual void TestConnection(Client client)
        {
            client.ConnectionTryCount++;
            NetworkingClient.SendPacket(SignPacket(new TestPacket()), client.EndPoint);
        }

        #endregion

        #region Network Actions

        public virtual void Close()
        {
            if (NetworkingClient is UdpNetworkingClient udp)
                udp.UdpClient.Close();
        }

        #endregion

        protected override void Dispose(bool isDisposing)
        {
            ShareWithAllClients(new ServerClosingPacket());
            Close();
            base.Dispose(isDisposing);
        }
    }
}
