using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace SocketServer
{

    public partial class Form1 : Form
    {

        private Socket listeningSocket;
        private bool listening;
        private Thread serverThread;
        

        public Form1()
        {
            InitializeComponent();
            listening = false;
        }

        // Delegate method to update GUI from a different thread
        public delegate void logCallback(string logLine);
        public void log(string logLine)
        {
            if (InvokeRequired) this.Invoke(new logCallback(log), new object[] { logLine });
            else tbLog.AppendText(logLine + Environment.NewLine);
        }

        // Delegate method to update temperature 
        public delegate void setTempCallback(string temperature);
        public void setTemp(string temperature)
        {
            if (InvokeRequired) this.Invoke(new setTempCallback(setTemp), new object[] { temperature });
            else
            {                
                float floatTemp;

                // Check if received string is a correct value
                if (float.TryParse(temperature, out floatTemp) && floatTemp >= 0 && floatTemp <= 300)
                {

                    // Update textbox and progress bar
                    tbTemp.Text = temperature;
                    pbTemp.Value = (int)(floatTemp * 10);
                }
                else log("Invalid temperature value received");                
            }
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            // Start listening...
            if (!listening)
            {
                // Check if tbPort.Text is a correct number
                int listeningPort;
                if (int.TryParse(tbPort.Text, out listeningPort))
                {
                    if (listeningPort > 0 && listeningPort < 65768)
                    {
                        // Create listening socket and bind it to Any:PORT
                        IPAddress listeningIp = IPAddress.Any;
                        IPEndPoint listeningEndPoint = new IPEndPoint(listeningIp, listeningPort);
                        listeningSocket = new Socket(listeningIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        listeningSocket.Bind(listeningEndPoint);
                        listeningSocket.Listen(10);
                        
                        // Start thread to handle incoming connections
                        SocketHandler socketHandler = new SocketHandler(listeningSocket, this);
                        serverThread = new Thread(new ThreadStart(socketHandler.startListening));
                        serverThread.Start();

                        log("Listening on port " + listeningPort);
                        btStart.Text = "Stop listening";
                        tbPort.ReadOnly = true;
                        listening = true;
                    }
                }
                else MessageBox.Show("Invalid port number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Stop listening...
            else 
            {
                listeningSocket.Close();
                serverThread.Abort();
                log("Listening stopped");

                btStart.Text = "Start listening";
                tbPort.ReadOnly = false;
                listening = false;
            }
        }
    }
}
