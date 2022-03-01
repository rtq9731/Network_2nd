using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
public class NodeClass
{
    public string name;
    public string sprite;
    public float x;
    public float y;

    public List<NodeClass> data;
}

[ExecuteInEditMode]
public class EditManager : MonoBehaviour
{
    private Dictionary<string, GameObject> dictPrefabs;
    private Dictionary<string, Sprite> dictSprites;
    public List<GameObject> listPrefabs;
    public GameObject tilePrefab;
    private Sprite[] tileSprites;

    static EditManager instance;
    public static EditManager Instance => instance;

    

#if UNITY_EDITOR
    [MenuItem("Stage/Save/Save as Txt")]
    static void SaveAs()
    {
        Debug.Log("Saving current edit...");
        if (instance)
        {
            string[] filters = { "Text", "txt", "Json", "json", "All Files", "*" };
            var path = EditorUtility.SaveFilePanel(
                "Save Stage as txt",
                "",
                "stage0000.txt",
                "txt");

            if (path.Length != 0)
            {
                string str = instance.StringfyStage();
                File.WriteAllText(path, str);
            }
        }
    }

    public string StringfyStage()
    {
        string result = "";

        GameObject stage = GameObject.Find("Stage");
        result += StringfyNode(stage, 0);
        return result;
    }

    private string SetIndent(int indent)
    {
        string result = "";
        for (int j = 0; j < indent; j++)
        {
            result += "  ";
        }
        return result;
    }

    private string StringfyNode(GameObject node, int indent=1)
    {
        string result = SetIndent(indent) + "{";
        result += $"\"name\" :\"{node.transform.name}\", \"data\":[\n";

        for (int i = 0; i < node.transform.childCount; i++)
        {
            Transform tr = node.transform.GetChild(i);
            if (i > 0)
            {
                result += ",\n";
            }
            SpriteRenderer sr = tr.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                result += SetIndent(indent + 1);
                result += "{" + $"\"name\" : \"{tr.name}\", \"sprite\" : \"{sr.sprite.name}\", \"x\" : {tr.transform.position.x}, \"y\" : {tr.position.y}" + "}";
            }
            else
            {
                result += StringfyNode(tr.gameObject, indent+1);
            }
        }

        result += "\n" + SetIndent(indent) + "]}";
        return result;
    }
#endif

    private void Start()
    {
        instance = this;

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
    }


    [MenuItem("Stage/Load/Load Txt")]
    static void Load()
    {
        string path = EditorUtility.OpenFilePanel(
                "Opne Save File",
                "",
                "txt");

        if(path.Length != 0)
        {
            string data = File.ReadAllText(path);
            NodeClass stage = JsonUtility.FromJson<NodeClass>(data);
            instance.ParseNode(stage);
        }

        //instance.StringParseStage(data);
    }

    public static void LoadStage(string data)
    {
        if (data.Length != 0)
        {
            NodeClass stage = JsonUtility.FromJson<NodeClass>(data);
            instance.ParseNode(stage);
        }
    }

    private void ParseNode(NodeClass node, GameObject parent = null)
    {
        if(parent == null)
        {
            parent = new GameObject("New Stage");
        }

        if(node != null)
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
