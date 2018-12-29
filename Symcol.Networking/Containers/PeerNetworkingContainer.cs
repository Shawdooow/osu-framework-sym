using System.Collections.Generic;
using Symcol.Base.Graphics.Containers;
using Symcol.Networking.NetworkingHandlers.Peer;
using Symcol.Networking.Packets;

namespace Symcol.Networking.Containers
{
    public class PeerNetworkingContainer : SymcolContainer
    {
        public readonly PeerNetworkingHandler PeerNetworkingHandler;

        private readonly List<PacketInfo> que = new List<PacketInfo>();

        private bool loaded;

        public PeerNetworkingContainer(PeerNetworkingHandler handler)
        {
            PeerNetworkingHandler = handler;
            PeerNetworkingHandler.OnPacketReceive += onPacketReceive;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            loaded = true;
            foreach (PacketInfo info in que)
                onPacketReceive(info);
        }

        protected virtual void SendPacket(Packet packet) => PeerNetworkingHandler.SendToServer(packet);

        private void onPacketReceive(PacketInfo info)
        {
            if (loaded)
                OnPacketReceive(info);
            else
                que.Add(info);
        }

        protected virtual void OnPacketReceive(PacketInfo info)
        {
        }

        protected override void Dispose(bool isDisposing)
        {
            PeerNetworkingHandler.OnPacketReceive -= onPacketReceive;
            base.Dispose(isDisposing);
        }
    }
}
