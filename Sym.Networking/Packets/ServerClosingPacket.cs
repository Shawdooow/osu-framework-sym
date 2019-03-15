using System;

namespace Sym.Networking.Packets
{
    [Serializable]
    public class ServerClosingPacket : Packet
    {
        public override uint PacketSize => 256;
    }
}
