﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketLibrary;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GuiTest1
{
    class NM
    {
        private static Socket sender;
        private static bool recieving;

        public static void Connect(string ip, int port)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 16000);

            sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(remoteEP);

            recieving = true;
            
        }

        public static void SendMessage(string message)
        {
            byte[] msg = Encoding.ASCII.GetBytes(message);

            int bytesSent = sender.Send(msg);
        }

        public delegate void UpdateListboxCallback(string message);

        private static void RecvLoop()
        {
            byte[] bytes = new byte[1024];
            while (recieving == true)
            {
                int bytesRec = sender.Receive(bytes);
                screenMain.pageMain.lbMainMessages.Dispatcher.Invoke(() => { screenMain.pageMain.lbMainMessages.Items.Add(Encoding.ASCII.GetString(bytes, 0, bytesRec)); });
                //screenMain.pageMain.lbMainMessages.Items.Add(Encoding.ASCII.GetString(bytes, 0, bytesRec));
            }
        }

        public static void RecieveMessage()
        {
            ThreadStart childref = new ThreadStart(RecvLoop);
            Thread childThread = new Thread(childref);
            childThread.Start();
        }

        public static void DC()
        {
            recieving = false;
        }
    }
}
