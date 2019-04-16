#region usings

#endregion

#region usings

using System.Net;
using System.Net.Sockets;
using osu.Framework.Logging;

#endregion

namespace Sym.Networking.NetworkingHandlers.Server
{
    public class Peer : Client
    {
        public override ConnectionStatues Statues
        {
            get => base.Statues;
            set
            {
                if (base.Statues != value)
                {
                    base.Statues = value;
                    switch (base.Statues)
                    {
                        case ConnectionStatues.Connecting:
                            Logger.Log($"Peer {EndPoint.Address} is connecting!", LoggingTarget.Network);
                            break;
                        case ConnectionStatues.Connected:
                            Logger.Log($"Peer {EndPoint.Address} is connected!", LoggingTarget.Network);
                            break;
                        case ConnectionStatues.Disconnected:
                            Logger.Log($"Peer {EndPoint.Address} has lost connection!", LoggingTarget.Network);
                            break;
                    }
                }
            }
        }

        protected internal TcpClient TcpClient { get; internal set; }

        protected internal int NextPacketSize { get; internal set; }

        public Peer(IPEndPoint end)
            : base(end)
        {
        }
    }
}
