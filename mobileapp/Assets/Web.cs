using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static API;

public class Web : MonoBehaviour
{
    public Text text;
    public API api;


    void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
                            UnityEngine.WebGLInput.captureAllKeyboardInput = false; // or true
#endif

        ID(4);
    }

    public void ID(int ID)
    {
        ID = 4;
        text.text = ID.ToString();
    }

    public void Fuck()
    {
        API_SearchContacts_Set input = new API_SearchContacts_Set { search = "", userId = 48 };
        api.SearchContacts(input);
    }
}
