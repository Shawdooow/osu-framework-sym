using System;
using System.Runtime.Serialization;

namespace Sym.Networking.Packets.FileTransfer
{
    [Serializable]
    public class StartFileTransferPacket : SpecializedPacket
    {
        public readonly long Size;

        public StartFileTransferPacket(string name, long size)
            : base(name)
        {
            Size = size;
        }

        public StartFileTransferPacket(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Size = info.GetInt64("s");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("s", Size, typeof(long));
        }
    }
}
