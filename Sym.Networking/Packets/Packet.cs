
#region usings

using System;

#endregion

namespace Sym.Networking.Packets
{
    [Serializable]
    public abstract class Packet
    {
        public virtual uint PacketSize => 8192;
    }
}
