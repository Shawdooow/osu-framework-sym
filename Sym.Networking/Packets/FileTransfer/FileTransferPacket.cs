#region usings

using System;
using System.Runtime.Serialization;

#endregion

namespace Sym.Networking.Packets.FileTransfer
{
    [Serializable]
    public class FileTransferPacket : SpecializedPacket
    {
        public readonly byte[] Data;

        public FileTransferPacket(string name, byte[] data)
            : base(name)
        {
            Data = data;
        }

        public FileTransferPacket(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Data = (byte[])info.GetValue("d", typeof(byte[]));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("d", Data, typeof(byte[]));
        }
    }
}
