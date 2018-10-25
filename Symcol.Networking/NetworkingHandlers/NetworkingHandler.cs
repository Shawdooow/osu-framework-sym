﻿using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using osu.Framework.Timing;
using Symcol.Base.Graphics.Containers;
using Symcol.Networking.NetworkingClients;
using Symcol.Networking.Packets;

// ReSharper disable InconsistentNaming

namespace Symcol.Networking.NetworkingHandlers
{
    public abstract class NetworkingHandler : SymcolContainer
    {
        #region Fields

        protected virtual string Gamekey => null;

        protected NetworkingClient NetworkingClient { get; set; }

        /// <summary>
        /// Gets hit when we get + send a Packet
        /// </summary>
        public Action<PacketInfo> OnPacketReceive;

        /// <summary>
        /// TODO: Implement TCP connections
        /// </summary>
        public bool Tcp
        {
            get => tcp;
            set
            {
                // ReSharper disable once RedundantCheckBeforeAssignment
                if (value != tcp)
                {
                    tcp = value;
                }
            }
        }

        private bool tcp;

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

            foreach (PacketInfo p in ReceivePackets())
                HandlePackets(p);
        }

        /// <summary>
        /// Handle any packets we got before sending them to OnPackerReceive
        /// </summary>
        /// <param name="packet"></param>
        protected virtual void HandlePackets(PacketInfo packet) => OnPacketReceive?.Invoke(packet);

        #endregion

        #region Packet and Client Helper Functions

        /// <summary>
        /// returns a list of all avalable packets
        /// </summary>
        /// <returns></returns>
        protected abstract List<PacketInfo> ReceivePackets();

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
    }

    public enum ConnectionStatues
    {
        Disconnected,
        Connecting,
        Connected
    }
}
