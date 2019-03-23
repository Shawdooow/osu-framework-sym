#region usings

using System.Net;
using Sym.Networking.Packets;

#endregion

namespace Sym.Networking.NetworkingClients
{
    public class TcpNetworkingClient : NetworkingClient
    {
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public override int Available { get; }

        public TcpNetworkingClient(string address)
            : base(address)
        {
        }

        public override Packet GetPacket()
        {
            throw new System.NotImplementedException();
        }

        public override void SendBytes(byte[] bytes, IPEndPoint end)
        {
            throw new System.NotImplementedException();
        }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
