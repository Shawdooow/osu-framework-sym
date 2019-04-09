#region usings

using System;
using System.Net;
using osu.Framework.Logging;
using Sym.Networking.Packets;

#endregion

// ReSharper disable InconsistentNaming

namespace Sym.Networking.NetworkingHandlers
{
    public abstract class Client
    {
        public readonly IPEndPoint EndPoint;

        /// <summary>
        /// Gets hit when we get a Packet
        /// </summary>
        public Action<Packet> OnPacketReceive;

        /// <summary>
        /// Last successful ping to this client
        /// </summary>
        public double Ping { get; set; }

        public virtual ConnectionStatues Statues
        {
            get => statues;
            set
            {
                if (statues != value)
                {
                    statues = value;
                    switch (statues)
                    {
                        case ConnectionStatues.Connecting:
                            OnConnecting?.Invoke();
                            break;
                        case ConnectionStatues.Connected:
                            OnConnected?.Invoke();
                            break;
                        case ConnectionStatues.Disconnected:
                            OnDisconnected?.Invoke();
                            break;
                    }
                }
            }
        }

        private ConnectionStatues statues;

        public event Action OnConnecting;

        public event Action OnConnected;

        public event Action OnDisconnected;

        public int ConnectionTryCount { get; set; }

        public double LastConnectionTime { get; set; }

        protected Client(IPEndPoint end) => EndPoint = end;
    }
}
