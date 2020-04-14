using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInputManager : MonoBehaviour
{
    public GameObject pin;
    public Transform world;

    public string website;
    public string message;
    
    void Update()
    {
        float x, y, z;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.rigidbody != null)
            {
                x = hit.point.x;
                y = hit.point.y;
                z = hit.point.z;

                GameObject tempPin = Instantiate(pin, world);
                tempPin.transform.SetPositionAndRotation(new Vector3(x, y, z), Quaternion.identity);
                tempPin.transform.localScale = new Vector3(.01f, .01f, .01f);
            }
        }
    }
}
