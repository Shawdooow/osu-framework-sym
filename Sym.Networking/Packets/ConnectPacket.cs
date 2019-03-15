using System;

namespace Sym.Networking.Packets
{
    [Serializable]
    public class ConnectPacket : Packet
    {
        public override uint PacketSize => 256;

        public string Gamekey;
    }
}
