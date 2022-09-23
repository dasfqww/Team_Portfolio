using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers:MonoBehaviour
{
    static Managers s_Instance;
    static Managers Instance{ get{ Init(); return s_Instance; } }

    DataManager _data = new DataManager();
    SoundManager _sound = new SoundManager();

    public static DataManager Data { get { return Instance._data; } }
    public static SoundManager Sound { get { return Instance._sound; } }

    // Start is called before the first frame update
    void Start()
    {
        Init();      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void Init()
    {
        if (s_Instance = null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go==null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();

            s_Instance._data.Init();
        }        
    }
}
