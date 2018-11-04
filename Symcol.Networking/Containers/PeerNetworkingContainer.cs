using Symcol.Base.Graphics.Containers;
using Symcol.Networking.NetworkingHandlers.Peer;
using Symcol.Networking.Packets;

namespace Symcol.Networking.Containers
{
    public class PeerNetworkingContainer : SymcolContainer
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
