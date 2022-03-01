using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private void Start()
    {
        PlayerManager.Instance.AddPlayer("Player_Hero", "12sfweq", "Hero").transform.position = Vector3.zero;
        PlayerManager.Instance.AddPlayer("Player_Enemy", "12sfasdfq", "Enemy").transform.position = new Vector3(1, 0, 1);
        PlayerManager.Instance.AddPlayer("Player_Enemy", "12sf23", "Enemy").transform.position = new Vector3(1, 0, 3);
    }
}
