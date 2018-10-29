using System;

namespace Symcol.Networking.Packets
{
    [Serializable]
    public class ServerClosingPacket : Packet
    {
        public override int PacketSize => 256;
    }
}
