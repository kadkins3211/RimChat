using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using RimChat.Server;
using RimChat.Client;

namespace RimChat.Core
{
    public class ChatManager
    {
        // Server
        RimServer Server;
        private object lockProcessPacket = new object();

        // Client
        public RimClient client;
        const string serverAddress = "127.0.0.1";
        const int port = 11000;

        // Messages
        public List<string> Messages;
        public string InputText;
        public string InputHost;
        public string InputPort;

        public ChatManager()
        {
            Messages = new List<string>();
            InputText = "";
            InputHost = "127.0.0.1";
            InputPort = "11000";
            Messages.Add(DateTime.UtcNow + " Welcome to RimChat...");
        }

        #region Server
        // Start the server 
        public void StartServer(IPAddress ipAddress, int port)
        {
            if(this.Server != null)
            {
                Messages.Add(String.Format("Server already running on {0}:{1}", 
                    this.Server.GetIPAddress().ToString(), this.Server.GetPort()));
                return;
            }
            Messages.Add(String.Format("Starting server on {0}:{1}", ipAddress.ToString(), port));
            this.Server = new RimServer(ipAddress, port);
            this.Server.Start();
            Messages.Add(String.Format("Server started on {0}:{1}", 
                this.Server.GetIPAddress().ToString(), this.Server.GetPort()));
            this.Server.Connection += this.ServerConnectionCallback;
            this.Server.Message += this.ServerMessageCallback;
            this.Server.Disconnection += this.ServerDisconnectionCallback;
        }
        
        // Callbacks registered with RimServer
        private void ServerConnectionCallback(ServerClient client)
        {
            Messages.Add("SERVER: Connection from " + client.ID);
        }

        private void ServerMessageCallback(ServerClient client, byte[] data)
        {
            // lock while processing
            lock (lockProcessPacket)
            {
                Messages.Add(String.Format("SERVER: {0} -> Server: {1}",client.ID,Encoding.Default.GetString(data)));
                // Add deserliazer
                // Send packet to clients
                this.Server.SendAll(String.Format("{0} {1}: {2}", DateTime.UtcNow, client.ID, Encoding.Default.GetString(data)));
            }             
        }

        private void ServerDisconnectionCallback(ServerClient client)
        {
            Messages.Add("SERVER: Disconnect from " + client.ID);
        }
        #endregion

        #region Client
        public void TryConnect()
        {
            TryConnect(InputHost, InputPort);
        }

        public void TryConnect(string inputHost, string inputPort)
        {
            Messages.Add(String.Format("CLIENT: trying to connect to {0}:{1}", inputHost, Convert.ToInt32(inputPort)));
            if (this.client != null)
            {
                Messages.Add(String.Format("CLIENT: existing found, disconnecting before trying new connection"));
                this.client.Disconnect();
                this.client = null;
            }
            this.client = new RimClient(inputHost, Convert.ToInt32(inputPort));
            try
            {
                client.Connect();
                Messages.Add(client.state.ToString());
                this.client.Message += this.ClientMessageCallback;
            }
            catch (Exception ex)
            {
                Messages.Add(String.Format("CLIENT: exception while trying to connect:\n {0}", ex.Message));
            }
        }

        private void ClientMessageCallback(byte[] data)
        {
            Messages.Add(Encoding.Default.GetString(data));
        }

        public bool IsConnected()
        {
            return this.client != null && this.client.state == WebSocketSharp.WebSocketState.Open;
        }
        #endregion
    }
}
