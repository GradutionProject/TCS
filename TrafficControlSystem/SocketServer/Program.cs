using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;

namespace SocketServer
{

    public class SocketHandler
    {

        private Socket listeningSocket;
        private Form1 myForm;


        public SocketHandler(Socket listeningSocket, Form1 myForm)
        {            
            this.listeningSocket = listeningSocket;
            this.myForm = myForm;
        }

        public void startListening()
        {

            Socket handlerSocket = null;
            while (true)
            {
                try
                {
                    // Wait for incoming connection
                    handlerSocket = listeningSocket.Accept();
                    handlerSocket.ReceiveTimeout = 5000;
                    IPAddress clientAddress = ((IPEndPoint)handlerSocket.RemoteEndPoint).Address;
                    myForm.log("Client connected: " + clientAddress);

                    string message = null;

                    while (true)
                    {
                        // Save received bytes
                        byte[] buffer = new byte[1024];
                        int bytesRec = handlerSocket.Receive(buffer);
                        message += Encoding.ASCII.GetString(buffer, 0, bytesRec);

                        // Message completed? Parse it...
                        if (message.Contains("\r"))
                        {
                            myForm.setTemp(message.Replace('.', ','));
                            myForm.log(" message received: " + message);
                            break;
                        }
                    }

                    // Close connection
                    handlerSocket.Close();
                    myForm.log("Client disconnected");
                }
            
                catch (Exception)
                {
                    // If we're in error (timeout, thread stopped...) close socket and return
                    if (handlerSocket != null)
                    {
                        if(handlerSocket.Connected) handlerSocket.Disconnect(false);
                        handlerSocket.Close();
                        myForm.log("Client disconnected");
                    }                
                }
            }
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
