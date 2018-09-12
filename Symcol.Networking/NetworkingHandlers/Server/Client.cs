using System;
using System.Net;
// ReSharper disable InconsistentNaming

namespace Symcol.Networking.NetworkingHandlers.Server
{
    public class Client
    {
        public IPEndPoint EndPoint { get; set; }

        public int ID { get; set; }

        /// <summary>
        /// Last successful ping to this client
        /// </summary>
        public double Ping { get; set; }

        public ConnectionStatues Statues
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

        public event Action OnConnecting;

        public event Action OnConnected;

        public event Action OnDisconnected;

        private ConnectionStatues statues;

        public int ConnectionTryCount { get; set; }

        public double LastConnectionTime { get; set; }
    }
}
