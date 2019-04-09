#region usings

using Sym.Networking.NetworkingHandlers;

#endregion

namespace Sym.Networking.Packets
{
    public class PacketInfo<T>
        where T : Client
    {
        public readonly T Client;

        public readonly Packet Packet;

        public PacketInfo(T client, Packet packet)
        {
            Client = client;
            Packet = packet;
        }
    }
}
