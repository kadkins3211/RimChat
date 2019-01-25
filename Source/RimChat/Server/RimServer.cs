using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace RimChat.Server
{
    public class RimServer
    {
        WebSocketServer server;
        List<ServerClient> clients = new List<ServerClient>();

        public event Action<ServerClient> Connection;

        public delegate void MessageHandler(ServerClient client, byte[] data);
        public event MessageHandler Message;

        public event Action<ServerClient> Disconnection;

        public RimServer(IPAddress address, int port)
        {
            this.server = new WebSocketServer(address, port);
        }

        public void SendAll(string data)
        {
            foreach(ServerClient client in clients)
            {
                client.Send(data);
            }
        }

        public void Start()
        {
            this.server.Start();
            this.server.AddWebSocketService<ServerClient>("/", () =>
            {
                return new ServerClient(this);
            });
        }

        public IPAddress GetIPAddress()
        {
            return this.server.Address;
        }

        public int GetPort()
        {
            return this.server.Port;
        }

        internal void ConnectionCallback(ServerClient client)
        {
            this.clients.Add(client);

            this.Connection(client);
        }

        internal void MessageCallback(ServerClient client, byte[] data)
        {
            this.Message(client, data);
        }

        internal void CloseCallback(ServerClient client)
        {
            this.clients.Remove(client);

            this.Disconnection(client);
        }
    }
}
