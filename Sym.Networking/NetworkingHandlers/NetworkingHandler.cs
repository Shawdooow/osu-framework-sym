#region usings

using System;
using System.Collections.Generic;
using System.Net;
using osu.Framework.Graphics.Containers;
using osu.Framework.Timing;
using Sym.Networking.NetworkingClients;
using Sym.Networking.Packets;

#endregion

// ReSharper disable InconsistentNaming

namespace Sym.Networking.NetworkingHandlers
{
    public abstract class NetworkingHandler<T> : Container
        where T : Client
    {
        #region Fields

        protected virtual string Gamekey => null;

        protected abstract T GetClient(IPEndPoint end);

        protected TcpNetworkingClient TcpNetworkingClient { get; set; }

        protected UdpNetworkingClient UdpNetworkingClient { get; set; }

        /// <summary>
        /// Called when the address is changed
        /// </summary>
        public event Action<string, int> OnAddressChange;

        /// <summary>
        /// Our IP:Port
        /// </summary>
        public string Address
        {
            get => IP + ":" + Port;
            set
            {
                string[] split = value.Split(':');

                string i = split[0];
                int p = int.Parse(split[1]);

                if (IP + Port != value)
                {
                    IP = i;
                    Port = p;
                    OnAddressChange?.Invoke(i, p);
                }
            }
        }

        /// <summary>
        /// Called when the ip is changed
        /// </summary>
        public event Action<string> OnIPChange;

        /// <summary>
        /// Our IP
        /// </summary>
        public string IP
        {
            get => ip;
            private set
            {
                if (ip != value)
                {
                    ip = value;
                    OnIPChange?.Invoke(ip);
                }
            }
        }

        private string ip;

        /// <summary>
        /// Called when the port is changed
        /// </summary>
        public event Action<int> OnPortChange;

        /// <summary>
        /// Our Port
        /// </summary>
        public int Port
        {
            get => port;
            private set
            {
                if (port != value)
                {
                    port = value;
                    OnPortChange?.Invoke(port);
                }
            }
        }

        private int port;

        /// <summary>
        /// Called when the Udp option is changed
        /// </summary>
        public event Action<bool> OnUdpChange;

        /// <summary>
        /// Enables Udp
        /// </summary>
        public bool Udp
        {
            get => udp;
            set
            {
                if (value != udp)
                {
                    udp = value;
                    OnUdpChange?.Invoke(value);
                }
            }
        }

        private bool udp;

        #endregion

        protected NetworkingHandler()
        {
            AlwaysPresent = true;

            DecoupleableInterpolatingFramedClock clock = new DecoupleableInterpolatingFramedClock { IsCoupled = false };
            Clock = clock;
            clock.Start();
        }

        #region Update Loop

        protected override void Update()
        {
            base.Update();

            foreach (PacketInfo<T> p in GetPackets())
                PacketReceived(p);
        }

        /// <summary>
        /// Handle any packets we got before sending them to OnPackerReceive
        /// </summary>
        /// <param name="packet"></param>
        protected abstract void PacketReceived(PacketInfo<T> packet);

        #endregion

        #region Packet and Client Helper Functions

        /// <summary>
        /// returns a list of all avalable packets
        /// </summary>
        /// <returns></returns>
        protected abstract List<PacketInfo<T>> GetPackets();

        /// <summary>
        /// Signs this packet so everyone knows where it came from
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        protected virtual Packet SignPacket(Packet packet)
        {
            if (packet is ConnectPacket c)
                c.Gamekey = Gamekey;
            return packet;
        }

        #endregion

        protected override void Dispose(bool isDisposing)
        {
            TcpNetworkingClient?.Dispose();
            UdpNetworkingClient?.Dispose();
            base.Dispose(isDisposing);
        }
    }

    public enum ConnectionStatues
    {
        Disconnected,
        Connecting,
        Connected
    }
}
