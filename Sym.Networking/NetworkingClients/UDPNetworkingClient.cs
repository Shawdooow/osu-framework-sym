#region usings

using System;
using System.Net;
using System.Net.Sockets;
using osu.Framework.Logging;

#endregion

// ReSharper disable VirtualMemberCallInConstructor

namespace Sym.Networking.NetworkingClients
{
    public class UdpNetworkingClient : NetworkingClient
    {
        protected readonly UdpClient UdpClient;

        public override int Available => UdpClient?.Available ?? 0;

        public UdpNetworkingClient(string address)
            : base(address)
        {
            try
            {
                UdpClient = new UdpClient();
                UdpClient.Connect(EndPoint);
                Logger.Log($"No exceptions while creating peer UdpClient with address {address}!", LoggingTarget.Runtime, LogLevel.Debug);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error while setting up a new Peer UdpClient!");
                Dispose();
            }
        }

        public UdpNetworkingClient(int port)
            : base(port)
        {
            try
            {
                UdpClient = new UdpClient(EndPoint);
                Logger.Log($"No exceptions while updating server UdpClient with port {port}", LoggingTarget.Runtime, LogLevel.Debug);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error while setting up a new Server UdpClient!");
                Dispose();
            }
        }

        public override void SendBytes(byte[] bytes, IPEndPoint end)
        {
            if (end == null && !UdpClient.Client.Connected)
            {
                Logger.Log($"UdpClient is not connected to {UdpClient.Client.RemoteEndPoint}!", LoggingTarget.Network, LogLevel.Error);
                return;
            }

            try
            {
                if (end == null)
                {
                    UdpClient.Send(bytes, bytes.Length);
                    Logger.Log($"No exceptions while sending bytes to {UdpClient.Client.RemoteEndPoint}", LoggingTarget.Runtime, LogLevel.Debug);
                }
                else
                {
                    UdpClient.Send(bytes, bytes.Length, end);
                    Logger.Log($"No exceptions while sending bytes to {end}", LoggingTarget.Runtime, LogLevel.Debug);
                }
            }
            catch (Exception e) { Logger.Error(e, "Error sending bytes!"); }                
        }

        public override byte[] GetBytes() => Available > 0 ? UdpClient.Receive(ref EndPoint) : null;

        public override void Dispose()
        {
            UdpClient?.Close();
            UdpClient?.Dispose();
        }
    }
}
