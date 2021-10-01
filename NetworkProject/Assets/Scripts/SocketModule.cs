using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SocketModule : MonoBehaviour
{
    static SocketModule Instance = null;

    private TcpClient clientSocket;
    private GameManager gm;

    private NetworkStream serverStream = default(NetworkStream);

    private Queue<string> msgQueue;
    private string nickName;

    bool bRunning = false;

    public static SocketModule GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        if (Instance = null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        msgQueue = new Queue<string>();
        gm = GetComponent<GameManager>();
    }

    private void Update()
    {
        
    }

    public void Login(string id)
    {
        if(!bRunning)
        {
            clientSocket = new TcpClient();
            clientSocket.Connect("localhost", 8888);
            serverStream = clientSocket.GetStream();

            byte[] outStream = Encoding.ASCII.GetBytes(id + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(GetMessage);
            ctThread.Start();
            bRunning = true;
            nickName = id;
        }
    }

    public void SendData(string str)
    {
        if(bRunning && serverStream != null)
        {
            byte[] outStream = Encoding.ASCII.GetBytes("$" + str);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
    }

    private void StopThread()
    {
        bRunning = false;
    }

    public void LogOut()
    {
        if(bRunning)
        {
            StopThread();
            msgQueue.Clear();
            nickName = "";
        }

        if (serverStream != null)
        {
            serverStream.Close();
            serverStream = null;
        }

        clientSocket.Close();
    }

    public bool IsOnline()
    {
        return bRunning;
    }

    public string GetNextData()
    {
        if(msgQueue.Count > 0)
        {
            string nextMsg = msgQueue.Dequeue();
            return nextMsg;
        }

        return null;
    }

    private void GetMessage()
    {
        byte[] inStream = new byte[1024];
        string returnData = "";

        try
        {
            while(bRunning)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                buffSize = clientSocket.ReceiveBufferSize;
                int numBytesRead;

                if(serverStream.DataAvailable)
                {
                    returnData = "";
                    while(serverStream.DataAvailable)
                    {
                        numBytesRead = serverStream.Read(inStream, 0, inStream.Length);
                        returnData += Encoding.ASCII.GetString(inStream, 0, numBytesRead);
                    }

                    gm.QueueCommand(returnData);
                    Debug.Log(returnData);
                }
            }
        }
        catch
        {
            StopThread();
        }
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

}
