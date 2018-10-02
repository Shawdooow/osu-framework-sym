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

        public ServerNetworkingHandler()
        {
            OnAddressChange += (ip, port) => { NetworkingClient = new UdpNetworkingClient(port); };
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
        /// <param name="packet"></param>
        protected override void HandlePackets(Packet packet)
        {
            base.HandlePackets(packet);

            switch (packet)
            {
                case ConnectPacket connectPacket:
                    Clients.Add(CreateConnectingClient(connectPacket));
                    SendToClient(new ConnectedPacket(), connectPacket);
                    break;
                case DisconnectPacket disconnectPacket:
                    ClientDisconnecting(disconnectPacket);
                    break;
                case TestPacket testPacket:
                    Client client = GetClient(testPacket);
                    if (client != null)
                    {
                        client.LastConnectionTime = Time.Current;
                        client.ConnectionTryCount = 0;
                        client.Statues = ConnectionStatues.Connected;
                    }
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
                    Clients.Remove(client);
                    Logger.Log("Connection to a connected client lost! - " + client.EndPoint, LoggingTarget.Network, LogLevel.Error);
                    break;
                }
            }
        }

        #endregion

        #region Packet and Client Helper Functions

        protected override Packet SignPacket(Packet packet)
        { 
            if (packet is ConnectPacket c)
                c.Gamekey = Gamekey;
            return packet;
        }

        /// <summary>
        /// Get a matching client from currently connecting/connected clients
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        protected Client GetClient(Packet packet)
        {
            foreach (Client client in Clients)
                if (client.EndPoint.ToString() == NetworkingClient.EndPoint.ToString())
                    return client;
            return null;
        }

        /// <summary>
        /// Called to remove a client that is disconnecting
        /// </summary>
        /// <param name="packet"></param>
        protected void ClientDisconnecting(DisconnectPacket packet)
        {
            Client client = GetClient(packet);
            if (client != null)
                client.Statues = ConnectionStatues.Disconnected;
        }

        protected virtual bool HandlePacket(Packet packet)
        {
            if (GetClient(packet) != null)
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

        protected void SendToClient(Packet packet, Packet recievedPacket)
        {
            NetworkingClient.SendPacket(SignPacket(packet), GetClient(recievedPacket).EndPoint);
        }

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

        }

        #endregion

        protected override void Dispose(bool isDisposing)
        {
            ShareWithAllClients(new ServerClosingPacket());
            base.Dispose(isDisposing);
        }
    }
}
