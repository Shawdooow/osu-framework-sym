#region usings

#region usings

using System;
using System.Net;
using System.Net.Sockets;
using osu.Framework.Logging;

#endregion

#pragma warning disable 618

#endregion

namespace Sym.Networking.NetworkingClients
{
    public class TcpNetworkingClient : NetworkingClient
    {
        public const int BUFFER_SIZE = 8192 * 256;

        public const int TIMEOUT = 30000;

        public TcpClient TcpClient { get; protected set; }

        public readonly TcpListener TcpListener;

        public virtual NetworkStream NetworkStream => TcpClient.GetStream();

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
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                Logger.Log("No Network Connection Detected!", LoggingTarget.Network, LogLevel.Error);

            try
            {
                TcpListener = new TcpListener(port);
                TcpListener.Start();
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
            TcpClient = result.Result;
            TcpClient.SendBufferSize = BUFFER_SIZE;
            TcpClient.SendTimeout = TIMEOUT;
            Logger.Log("TcpClient Connected!", LoggingTarget.Network);
        });

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
            TcpListener?.Stop();
        }
    }
}
