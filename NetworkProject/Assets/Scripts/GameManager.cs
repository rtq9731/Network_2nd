using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private SocketModule tcp;

    [SerializeField] ClientMove nickName;
    string myID;

    public GameObject prefabUnit;
    public GameObject mainChar;

    Dictionary<string, ClientMove> clientUnits;
    Queue<string> CommandQueue;

    public static GameManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private void Start()
    {
        tcp = GetComponent<SocketModule>();
        clientUnits = new Dictionary<string, ClientMove>();
        mainChar = FindObjectOfType<ClientMove>().gameObject;
    }

    private void Update()
    {
        ProcessQueue();

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector3 movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movePos.z = 0;

            mainChar.GetComponent<ClientMove>().Move(movePos);

            string data = "#Move#" + movePos.x + "," + movePos.y;
            SendCommand(data);
        }
    }

    private void ProcessQueue()
    {
        while (CommandQueue.Count > 0)
        {
            string nextCommand = CommandQueue.Dequeue();
            ProcessCommand(nextCommand);
        }
    }

    public void OnLogin()
    {
        string id = ""; //nickName ( ÀÎÇ²ÇÊµå ¸¸µå¼À )
        myID = id;

        if(id.Length > 0)
        {
            tcp.Login(id);
            mainChar.transform.position = Vector3.zero;
        }
    }

    public void OnLogout()
    {
        tcp.LogOut();
        
        foreach (var remotePair in clientUnits)
        {
            Destroy(remotePair.Value.gameObject);
        }

        clientUnits.Clear();
    }

    public ClientMove AddUnit(string id)
    {
        ClientMove cm = null;

        if(!clientUnits.ContainsKey(id))
        {
            GameObject newUnit = Instantiate(prefabUnit);
            cm = newUnit.GetComponent<ClientMove>();
            clientUnits.Add(id, cm);
        }

        return cm;
    }

    public void LeaveUnit(string id)
    {
        if(clientUnits.ContainsKey(id))
        {
            ClientMove uc = clientUnits[id];
            GameObject unit = uc.gameObject;
        }
    }

    public void SetMove(string id, string coordinates)
    {
        if(clientUnits.ContainsKey(id))
        {
            ClientMove cm = clientUnits[id];

            var strs = coordinates.Split(',');

            Vector3 pos = new Vector3(float.Parse(strs[0]), float.Parse(strs[1]), 0);

            cm.Move(pos);
        }
    }

    public void UserLeft(string id)
    {
        if(clientUnits.ContainsKey(id))
        {
            ClientMove cm = clientUnits[id];
            Destroy(cm.gameObject);
            clientUnits.Remove(id);
        }    
    }

    private void LoadHistory(string history)
    {
        var strs = history.Split(',');
        int max = strs.Length;

        for (int i = 0; i + 2 < max; i += 3)
        {
            string id = strs[i];
            if(myID.CompareTo(id) != 0)
            {
                ClientMove cm = AddUnit(id);
                if(cm != null)
                {
                    float x = float.Parse(strs[i + 1]);
                    float y = float.Parse(strs[i + 2]);
                    cm.transform.position = new Vector3(x, y, 0);
                }
            }
        }
    }

    private void SendCommand(string data)
    {

    }

    private void ProcessCommand(string nextCommand)
    {
        throw new NotImplementedException();
    }

}
