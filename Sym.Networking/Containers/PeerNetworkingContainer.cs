using osu.Framework.Graphics.Containers;
using Sym.Networking.NetworkingHandlers.Peer;
using Sym.Networking.Packets;

namespace Sym.Networking.Containers
{
    public class PeerNetworkingContainer : Container
    {
        public readonly PeerNetworkingHandler PeerNetworkingHandler;

        public PeerNetworkingContainer(PeerNetworkingHandler handler)
        {
            PeerNetworkingHandler = handler;
            PeerNetworkingHandler.OnPacketReceive += OnPacketRecieve;
        }

        protected virtual void SendPacket(Packet packet) => PeerNetworkingHandler.SendToServer(packet);

        protected virtual void OnPacketRecieve(PacketInfo info)
        {
        }

        protected override void Dispose(bool isDisposing)
        {
            PeerNetworkingHandler.OnPacketReceive -= OnPacketRecieve;
            base.Dispose(isDisposing);
        }
    }
}
