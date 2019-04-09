#region usings

using System.Runtime.Serialization;

#endregion

namespace Sym.Networking.Packets
{
    public interface IPacket : ISerializable
    {
        uint PacketSize { get; }
    }
}
