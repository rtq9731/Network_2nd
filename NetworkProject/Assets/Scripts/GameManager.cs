using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GameManager : MonoBehaviour
{
    const char CHAR_TERMINATOR = ';';
    const char CHAR_COMMA = ',';
    const int DAMAGE_ATTACK = 30;

    private SocketModule tcp;

    [SerializeField]
    private InputField nickname;
    string myID;

    public GameObject prefabUnit;
    public GameObject mainChar;

    private UnitControl mainControl = null;

    Dictionary<string, UnitControl> remoteUnits;
    Queue<string> commandQueue;

    // Start is called before the first frame update
    void Start()
    {
        tcp = GetComponent<SocketModule>();
        remoteUnits = new Dictionary<string, UnitControl>();
        commandQueue = new Queue<string>();

        mainControl = mainChar.GetComponent<UnitControl>();
    }
    // Update is called once per frame
    void Update()
    {
        ProcessQueue();
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 targetPos;
                //orgPos = transform.position;
                targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //targetPos.z = orgPos.z;
                mainControl.SetTargetPos(targetPos);

                string data = "#Move#" + targetPos.x + CHAR_COMMA + targetPos.y;
                SendCommand(data);
                //SocketModule.GetInstance().SendData(data);
                //Debug.Log("sent: " + data);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                if(mainControl.GetHP() > 0)
                {
                    string data = "#Attack#";
                    mainChar.GetComponent<UnitControl>().StartFX();
                    SendCommand(data);
                }
            }
        }

    }
    void ProcessQueue()
    {
        while (commandQueue.Count > 0)
        {
            string nextCommand = commandQueue.Dequeue();
            ProcessCommand(nextCommand);
        }
    }
    public void OnLogin()
    {
        string id = nickname.text;
        myID = id;
        if (id.Length > 0)
        {
            tcp.Login(id);
            mainChar.transform.position = Vector3.zero;
        }
    }
    public void OnLogOut()
    {
        tcp.Logout();
        // delete all remote chars
        foreach (var remotePair in remoteUnits)
        {
            Destroy(remotePair.Value.gameObject);
        }
        remoteUnits.Clear();
    }
    public void OnRevive()
    {
        mainChar.GetComponent<UnitControl>().Revive();

        string data = "#Heal#";
        SendCommand(data);
    }
    public UnitControl AddUnit(string id)
    {
        UnitControl uc = null;
        Debug.Log(id);
    if (!remoteUnits.ContainsKey(id))
        {
            GameObject newUnit = Instantiate(prefabUnit);
            uc = newUnit.GetComponent<UnitControl>();
            remoteUnits.Add(id, uc);
        }
        return uc;
    }
    public void LeaveUnit(string id)
    {
        if (remoteUnits.ContainsKey(id))
        {
            UnitControl uc = remoteUnits[id];
            GameObject unit = uc.gameObject;
            remoteUnits.Remove(id);
            Destroy(unit);
        }
    }
    public void SetMove(string id, string coordinates)
    {

        Debug.Log(id);
        Debug.Log(remoteUnits.ContainsKey(id));
        if (remoteUnits.ContainsKey(id))
        {
            UnitControl uc = remoteUnits[id];
            var strs = coordinates.Split(',');
            Vector3 pos = new Vector3(float.Parse(strs[0]), float.Parse(strs[1]), 0);
            uc.SetTargetPos(pos);
        }
    }
    public void UserLeft(string id)
    {
        if (remoteUnits.ContainsKey(id))
        {
            UnitControl uc = remoteUnits[id];
            Destroy(uc.gameObject);
            remoteUnits.Remove(id);
        }
    }

    private void LoadHistory(string history)
    {
        var strs = history.Split(',');
        int max = strs.Length;
        for (int i = 0; i + 2 < max; i += 3)

        {
            string id = strs[i];
            if (myID.CompareTo(id) != 0)
            {
                UnitControl uc = AddUnit(id);
                if (uc != null)
                {
                    float x = float.Parse(strs[i + 1]);
                    float y = float.Parse(strs[i + 2]);
                    //uc.SetTargetPos();
                    uc.transform.position = new Vector3(x, y, 0);
                }
            }
        }
    }

    public void SendCommand(string cmd)
    {
        SocketModule.GetInstance().SendData(cmd);
        Debug.Log("cmd sent: " + cmd);
    }

    public void UserHeal(string id)
    {
        if(remoteUnits.ContainsKey(id))
        {
            UnitControl uc = remoteUnits[id];
            uc.Revive();
        }
    }

    public void UserAttack(string id)
    {
        if(remoteUnits.ContainsKey(id))
        {
            UnitControl uc = remoteUnits[id];
            uc.StartFX();
        }
    }

    private void TakeDamage(string remain)
    {
        var strs = remain.Split(CHAR_COMMA);
        for (int i = 0; i < strs.Length; i++)
        {
            if(remoteUnits.ContainsKey(strs[i]))
            {
                UnitControl uc = remoteUnits[strs[i]];
                if(uc != null)
                {
                    uc.DropHP(DAMAGE_ATTACK);
                }
            }
            else
            {
                if(myID.CompareTo(strs[i]) == 0)
                {
                    mainControl.DropHP(DAMAGE_ATTACK);
                }
            }
        }
    }

    public string GetID(string cmd)
    {
        int idx = cmd.IndexOf("$");
        string id = "";
    if (idx > 0)
        {
            id = cmd.Substring(0, idx);
        }
        return id;
    }

    public void QueueCommand(string cmd)
    {
        commandQueue.Enqueue(cmd);
    }

    public void ProcessCommand(string cmd)
    {
        bool bMore = true;
    while (bMore)
        {
            Debug.Log("process cmd = " + cmd);

            int idx = cmd.IndexOf("$");
            string id = "";
    if (idx > 0)
            {
                id = cmd.Substring(0, idx);
            }
            int idx2 = cmd.IndexOf("#");
            if (idx2 > idx)
            {
                // command is there
                int idx3 = cmd.IndexOf("#", idx2 + 1);
                if (idx3 > idx2)
                {
                    string command = cmd.Substring(idx2 + 1, idx3 - idx2 - 1);
                    string remain = "";
                    string nextCommand;
                    int idx4 = cmd.IndexOf(CHAR_TERMINATOR, idx3 + 1);
                    if(idx4 > idx3)
                    {
                        remain = cmd.Substring(idx3 + 1, idx4 - idx3 - 1);
                        nextCommand = cmd.Substring(idx4 + 1);
                    }
                    else
                    {
                        remain = cmd.Substring(idx3 + 1, cmd.Length - idx3 - 1);
                        nextCommand = cmd.Substring(idx3 + 1);
                    }
                    

                    Debug.Log("command= " + command + " id=" + id + " remain=" + remain + " next=" + nextCommand);
                    if (myID.CompareTo(id) != 0)
                    {
                        switch (command)
                        {
                            case "Enter":
                                AddUnit(id);
                                break;
                        case "Move":
                                SetMove(id, remain);
                                break;
                        case "Left":
                                UserLeft(id);
                                break;
                        case "History":
                                LoadHistory(remain);
                                break;
                        case "Heal":
                                UserHeal(id);
                                break;
                        case "Attack":
                                UserAttack(id);
                                break;
                        case "Damage":
                                TakeDamage(remain);
                                break;
                        }
                    }
                    else
                    {
                        Debug.Log("Ignore remote command");
                    }

                    cmd = nextCommand;
                    if (cmd.Length <= 0)
                    {
                        // No more data to process
                        bMore = false;
                    }
                }

                else
                {
                    // parsing error
                    bMore = false;
                }
            }
            else
            {
                // parsing error
                bMore = false;
            }
        }
    }
}