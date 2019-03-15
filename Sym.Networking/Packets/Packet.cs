using System;

namespace Sym.Networking.Packets
{
    [Serializable]
    public abstract class Packet
    {
        /// <summary>
        /// Specify starting size of a packet (bytes) for efficiency
        /// </summary>
        public virtual uint PacketSize => 512;
    }
}
