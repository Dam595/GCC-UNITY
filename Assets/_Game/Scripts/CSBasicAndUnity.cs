using System.Collections.Generic;
using UnityEngine;

public class CSBasicAndUnity : MonoBehaviour
{
    readonly string email = "uhuhh";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void KeyWord()
    {
        var HP = 1.0f;

        Dictionary<int, string> dict = new Dictionary<int, string>();
        foreach(var key in dict.Keys)
        {
            Debug.Log(key);
        }

    }
}
