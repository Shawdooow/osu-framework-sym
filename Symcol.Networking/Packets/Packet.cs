using System;

namespace Symcol.Networking.Packets
{
    [Serializable]
    public abstract class Packet
    {
        /// <summary>
        /// Specify starting size of a packet (bytes) for efficiency
        /// </summary>
        public virtual int PacketSize => 512;
    }
}
