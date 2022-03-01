using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStage : MonoBehaviour
{
    private FirebaseManager fm;

    private Dictionary<string, GameObject> dictPrefabs;
    private Dictionary<string, Sprite> dictSprites;
    public List<GameObject> listPrefabs;
    public GameObject tilePrefab;
    private Sprite[] tileSprites;

    private void Start()
    {
        dictPrefabs = new Dictionary<string, GameObject>();
        foreach (var item in listPrefabs)
        {
            dictPrefabs[item.name] = item;
        }

        tileSprites = Resources.LoadAll<Sprite>("Platforms");
        dictSprites = new Dictionary<string, Sprite>();
        foreach (var item in tileSprites)
        {
            dictSprites.Add(item.name, item);
        }
        fm = GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>();
        LoadStageData(fm.stageData);
    }

    public void LoadStageData(string data)
    {
        if (data.Length != 0)
        {
            NodeClass stage = JsonUtility.FromJson<NodeClass>(data);
            ParseNode(stage);
        }
    }

    private void ParseNode(NodeClass node, GameObject parent = null)
    {
        if (parent == null)
        {
            parent = new GameObject("New Stage");
        }

        if (node != null)
        {
            if (node.data != null && node.data.Count > 0)
            {
                GameObject currentObj = MakeNode(node, parent);

                foreach (var child in node.data)
                {
                    ParseNode(child as NodeClass, currentObj);
                }
            }
            else
            {
                MakeNode(node, parent);
            }
        }
    }

    private GameObject MakeNode(NodeClass node, GameObject parent)
    {
        GameObject obj = null;

        if (parent != null)
        {
            
            if (dictPrefabs.ContainsKey(node.name))
            {
                obj = Instantiate(dictPrefabs[node.name]);
                obj.transform.position = new Vector2(node.x, node.y);
                obj.transform.SetParent(parent.transform);
            }
            else
            {
                if (node.sprite != null)
                {
                    obj = MakeSpriteNode(node, parent);
                }
                else
                {
                    obj = new GameObject(node.name);
                    obj.transform.SetParent(parent.transform);
                }
            }
        }

        return obj;
    }

    private GameObject MakeSpriteNode(NodeClass node, GameObject parent)
    {
        GameObject obj = null;

        if (parent != null)
        {
            obj = Instantiate(tilePrefab);

            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            obj.name = node.name;
            if (dictSprites.ContainsKey(node.sprite))
            {
                sr.sprite = dictSprites[node.sprite];
            }
            else
            {
                Debug.Log("Sprite not found" + node.sprite);
            }
            obj.transform.SetParent(parent.transform);
            obj.transform.position = new Vector2(node.x, node.y);
        }

        return obj;
    }
}
