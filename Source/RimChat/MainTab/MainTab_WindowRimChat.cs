using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using RimChat.Core;
using System.Net;

// TODO: Break up GUI into subclasses and make the chat window scrollable
// figure out why the enter key isn't being captured and sedning the message
namespace RimChat.MainTab
{
    internal class MainTab_WindowRimChat : MainTabWindow
    {
        public static ChatManager Manager;

        // Scroll position for main chat content
        private Vector2 contentScrollPosition = Vector2.zero;

        public MainTab_WindowRimChat()
        {
            if (Manager == null)
                Manager = new ChatManager();
            // Don't close the window on enter
            this.closeOnAccept = false;
        }

        // This actually draws the window content
        public override void DoWindowContents(Rect canvas)
        {
            Text.Font = GameFont.Small;
            var headerRect = new Rect(canvas.x + 5, canvas.y + 5, canvas.width - 10, canvas.height - 10);

            var contentRect = new Rect(canvas.x + 10, canvas.y + 40, canvas.width - 20, canvas.height - 100);
            var joinButtonRect = new Rect(canvas.width - 310, canvas.y + 5, 100, 30);
            var joinHostRect = new Rect(canvas.width - 210, canvas.y + 5, 100, 30);
            var joingPortRect = new Rect(canvas.width - 110, canvas.y + 5, 100, 30);
            var hostButtonRect = new Rect(canvas.width - 410, canvas.y + 5, 100, 30);

            var sendButtonRect = new Rect(canvas.width - 110, canvas.height - 30, 100, 30);
            var textInputRect = new Rect(canvas.x + 10,
                canvas.height - 30, 
                canvas.width - sendButtonRect.width - 10,
                sendButtonRect.height);

            Widgets.DrawMenuSection(contentRect);
            Widgets.Label(headerRect, "RimChat");
            // join button
            if(Widgets.ButtonText(joinButtonRect, "RimChat.Tab.Page.Button.Join".Translate(), true, false, true))
            {
                //Try to connect to the entered ip:port
                Manager.TryConnect();
            }

            // host and port inputs
            Manager.InputHost = Widgets.TextArea(joinHostRect, Manager.InputHost);
            Manager.InputPort = Widgets.TextArea(joingPortRect, Manager.InputPort);

            // Start a server
            if(Widgets.ButtonText(hostButtonRect, "RimChat.Tab.Page.Button.Host".Translate(), true, false, true))
            {
                Manager.StartServer(IPAddress.Any, 11000);
            }

            // Draw Messages to contentRect
            float curX = 5f,curY = 0f, height = 30f, yMargin = 0f;
            GUI.BeginGroup(contentRect);
            Widgets.BeginScrollView(contentRect,ref contentScrollPosition,contentRect.AtZero());
            foreach(var m in Manager.Messages)
            {
                Rect messageRect = new Rect(curX, curY, contentRect.width, height);
                Widgets.Label(messageRect, m);
                curY += height + yMargin;
            }
            Widgets.EndScrollView();
            GUI.EndGroup();

            // Chat Input          
            Manager.InputText = Widgets.TextArea(textInputRect, Manager.InputText);

            // Send button 
            if(Widgets.ButtonText(sendButtonRect, "RimChat.Tab.Page.Button.Send".Translate(), true, false, true))
            {
                UpdateMessageList();
            }
            // Capture enter key and update messages
            if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
            {
                Manager.Messages.Add("Enter key detected..");
                UpdateMessageList();
                Event.current.Use();
            }
        }

        // Updates message list with input text and sends to the server if connected
        private void UpdateMessageList()
        {
            Manager.Messages.Add(Manager.InputText);           
            if (Manager.InputText == "clear")
            {
                Manager.Messages.Clear();
            }
            else
            {
                Manager.client.Send(Manager.InputText);
            }
            Manager.InputText = "";
        }
    }
}
