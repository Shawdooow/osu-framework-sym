#region usings

using System;
using System.Net;
using System.Net.Sockets;
using osu.Framework.Logging;
using Sym.Networking.Packets;

#endregion

namespace Sym.Networking.NetworkingClients
{
    public class TcpNetworkingClient : NetworkingClient
    {
        public readonly TcpClient TcpClient;

        public readonly NetworkStream NetworkStream;

        public override int Available => TcpClient?.Available ?? 0;

        public TcpNetworkingClient(string address)
            : base(address)
        {
            try
            {
                TcpClient = new TcpClient();
                TcpClient.Connect(EndPoint);
                NetworkStream = TcpClient.GetStream();
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
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                Logger.Log("No Network Connection Detected!", LoggingTarget.Network, LogLevel.Error);

            try
            {
                TcpClient = new TcpClient(EndPoint);
                NetworkStream = TcpClient.GetStream();
                Logger.Log($"No exceptions while updating server TcpClient with port {port}", LoggingTarget.Runtime, LogLevel.Debug);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error while setting up a new Server TcpClient!");
                Dispose();
            }
        }

        public override void SendBytes(byte[] bytes, IPEndPoint end)
        {
            if (end != null) throw new InvalidOperationException("Blame TCP for sucking");

            if (!TcpClient.Client.Connected)
            {
                Logger.Log($"TcpClient is not connected to {TcpClient.Client.RemoteEndPoint}!", LoggingTarget.Network, LogLevel.Error);
                return;
            }

            try
            {
                NetworkStream.Write(bytes, 0, bytes.Length);
                Logger.Log($"No exceptions while sending bytes to {TcpClient.Client.RemoteEndPoint}", LoggingTarget.Runtime, LogLevel.Debug);
            }
            catch (Exception e) { Logger.Error(e, "Error sending bytes!"); }
        }

        public override byte[] GetBytes() => null;//Available > 0 ? NetworkStream.Read() : null;

        public override void Dispose()
        {
            TcpClient?.Close();
            TcpClient?.Dispose();
        }
    }
}
