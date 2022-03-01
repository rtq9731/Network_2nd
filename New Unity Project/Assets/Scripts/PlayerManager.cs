using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    List<PlayerBase> playerList = new List<PlayerBase>();

    public PlayerHero hero { get; set; } = null;

    private static PlayerManager s_Instance = null;
    public static PlayerManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<PlayerManager>() as PlayerManager;
            }
            return s_Instance;
        }
    }

    private void Awake()
    {
        if (s_Instance != null) return;
        s_Instance = this;
    }

    private void Update()
    {
        foreach (PlayerBase item in playerList)
        {
            item.UpdatePlayer();
        }
    }

    public PlayerBase FindPlayer(string uid)
    {
        return playerList.Find(x => x.UID == uid);
    }

    public void RemovePlayer(PlayerBase player)
    {
        if(playerList.Contains(player))
        {
            playerList.Remove(player);
        }
    }



    public PlayerBase AddPlayer(string prefab, string uid, string type)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>(prefab));
        Debug.Log(go);
        PlayerBase player = null;
        switch (type)
        {
            case "Hero":
                player = go.GetComponent<PlayerHero>();
                player.InitPlayer(uid);
                hero = player as PlayerHero;
                break;
            case "Enemy":
                player = go.GetComponent<PlayerEnemy>();
                player.InitPlayer(uid);
                break;
            default:
                player = go.GetComponent<PlayerBase>();
                player.InitPlayer(uid);
                break;
        }
        playerList.Add(player);
        return player;
    }
}
