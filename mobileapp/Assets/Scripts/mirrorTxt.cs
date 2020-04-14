using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mirrorTxt : MonoBehaviour
{
    [SerializeField]
    Text target;

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Text>().text = target.text;
    }
}
