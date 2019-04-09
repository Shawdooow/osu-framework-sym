#region usings

using System;
using System.Runtime.Serialization;

#endregion

namespace Sym.Networking.Packets
{
    [Serializable]
    public abstract class SpecializedPacket : Packet, IPacket
    {
        public readonly string Name;

        protected SpecializedPacket(string name) => Name = name;

        protected SpecializedPacket(SerializationInfo info, StreamingContext context) => Name = info.GetString("n");

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue("n", Name, typeof(string));
    }
}
