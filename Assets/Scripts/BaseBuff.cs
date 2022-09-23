using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuff : MonoBehaviour
{
    public string type;
    public float percentage;
    public float duration;
    public float currentTime;
    // Start is called before the first frame update
    
    public void Init()
    {

    }

    public void Excute()
    {

    }

    IEnumerator Activatinon()
    {
        yield return null;
    }

    public void DeActivation()
    {

    }
}
