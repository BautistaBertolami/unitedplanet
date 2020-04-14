using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static API;

public class GlobeManager : MonoBehaviour
{
    public float scaleMultiplier = 0.1f;
    public float rotateMultiplier = 0.1f;

    public float initialScale = 5;

    public float rotationSpeed = 50;
    public bool isRotating = true;

    [SerializeField]
    GameObject Pin;

    List<GameObject> Pins = new List<GameObject>();

    private void Awake()
    {
        Recenter();
    }

    public void SetScaleMultiplier(string st)
    {
        scaleMultiplier = float.Parse(st);
    }

    public void SetRotateMultiplier(string st)
    {
        rotateMultiplier = float.Parse(st);
    }

    public void Rotate(Vector3 rotation)
    {
        transform.Rotate(rotation * rotateMultiplier, Space.World);
    }

    public void Scale(float scale)
    {
        transform.localScale += new Vector3(scaleMultiplier * scale, scaleMultiplier * scale, scaleMultiplier * scale);
    }

    public void SetRotation(bool b)
    {
        isRotating = b;
    }

    public void Recenter()
    {
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
        transform.rotation = Quaternion.Euler(0, 90, -30);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public GameObject AddPin(float pinX, float pinY, float pinZ, GameObject pin)
    {
        GameObject tempPin;

        if (pin == null) tempPin = Instantiate(Pin, transform);
        else tempPin = pin;
            
        tempPin.transform.SetPositionAndRotation(new Vector3(pinX, pinY, pinZ), Quaternion.identity);
        tempPin.transform.localScale = new Vector3(2f, 2f, 2f);

        return tempPin;
    }

    public void AddPins(List<API_EditContact_Set> contacts)
    {
        if(Pins.Count != 0)
        {
            foreach (GameObject pin in Pins)
            {
                Destroy(pin);
            }
        }

        foreach (API_EditContact_Set contact in contacts)
        {
            GameObject tempPin = AddPin(contact.coordinates);
            Edit edit = tempPin.GetComponent<Edit>();
            edit.setContact(contact);
        }
    }

    GameObject AddPin(string coordinates)
    {
        string temp = "";
        float x;
        float y;
        float z;

        string pin = coordinates;

        int i = 0;
        while (pin[i] != ',')
        {
            temp += pin[i++];
        }

        //Debug.Log("X = " + temp);
        x = float.Parse(temp);
        temp = "";
        i++;

        while (pin[i] != ',')
        {
            temp += pin[i++];
        }

       // Debug.Log("Y = " + temp);
        y = float.Parse(temp);
        temp = "";
        i++;

        for (; i < pin.Length; i++)
        {
            temp += pin[i];
        }

        //Debug.Log("Z = " + temp);
        z = float.Parse(temp);
        temp = "";
        i++;

        return AddPin(x, y, z, null);
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        }
    }
}
