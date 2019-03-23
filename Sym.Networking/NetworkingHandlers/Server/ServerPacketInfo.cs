#region usings

using Sym.Networking.Packets;

#endregion

namespace Sym.Networking.NetworkingHandlers.Server
{
    public class ServerPacketInfo : PacketInfo
    {
        public Client Client;
    }
}
