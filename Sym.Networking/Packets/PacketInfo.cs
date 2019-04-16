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

        public readonly bool UDP;

        public PacketInfo(T client, Packet packet, bool udp = false)
        {
            Client = client;
            Packet = packet;
            UDP = udp;
        }
    }
}
