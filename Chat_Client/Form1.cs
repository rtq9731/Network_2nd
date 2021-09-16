﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Chat_Client
{
    public partial class Form1 : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;
        bool stopRunning = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            readData = "Conected to Chat Server ...";
            msg();
            clientSocket.Connect("localhost", 8888);
            serverStream = clientSocket.GetStream();

            byte[] outStream = Encoding.ASCII.GetBytes(NameInputBox.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(getMessage);
            ctThread.Start();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            byte[] outStream = Encoding.ASCII.GetBytes(NameInputBox.Text + "$" + InputMsgBox.Text);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();   
        }

         private void getMessage()
        {
            byte[] inStream = new byte[1024];
            string returndata = "";

            try
            {
                while (!stopRunning)
                {
                    serverStream = clientSocket.GetStream();
                    int buffSize = 0;
                    buffSize = clientSocket.ReceiveBufferSize;
                    int numBytesRead;

                    if (serverStream.DataAvailable)
                    {
                        returndata = "";
                        while (serverStream.DataAvailable)
                        {
                            numBytesRead = serverStream.Read(inStream, 0, inStream.Length);
                            returndata += Encoding.ASCII.GetString(inStream, 0, numBytesRead);
                            Array.Clear(inStream, 0, inStream.Length);
                        }

                        readData = "" + returndata;
                        msg();
                    }
                }
            }
            catch (Exception ex)
            {
                stopRunning = true;
            }
        }

        private void msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
                OutputMsgBox.Text = OutputMsgBox.Text + Environment.NewLine + " >> " + readData;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopRunning = true;

            if (serverStream != default(NetworkStream))
            {
                serverStream.Close();
            }

            clientSocket.Close();
        }
    }
}
