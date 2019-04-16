
#region usings

using System;

#endregion

namespace Sym.Networking.Packets
{
    [Serializable]
    public abstract class Packet
    {
        public virtual int PacketSize => 8192;
    }
}
