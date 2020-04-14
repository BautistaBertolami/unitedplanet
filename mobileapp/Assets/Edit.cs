using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static API;
using UnityEngine.UI;

public class Edit : MonoBehaviour
{
    private API_EditContact_Set contact;

    public GameObject confirmEditContactButton;
    public GameObject canvas;
    public Text name1;
    public Text phoneNumber;
    public Text email;
    public Text address;

    private API api;

    private void Start()
    {
        api = GameObject.Find("Manager").GetComponent<API>();
    }

    public void setContact(API_EditContact_Set instance)
    {
        contact = instance;
        UpdateFields();
        canvas.transform.SetParent(GameObject.Find("Canvases").transform);
    }

    public void SetContactContact(string st)
    {
        contact.contact = st;
        confirmEditContactButton.SetActive(true);

        UpdateFields();
    }

    public void SetContactCoord(string st)
    {
        contact.coordinates = st;
        UpdateFields();
    }

    public void SetContactPhoneNumber(string st)
    {
        contact.phoneNumber = st;
        UpdateFields();
    }

    public void SetContactEmail(string st)
    {
        contact.email = st;
        UpdateFields();
    }

    public void SetContactAddress(string st)
    {
        contact.address = st;
        UpdateFields();
    }

    public void ConfirmEditContact()
    {
        api.EditContact(contact);
    }

    void UpdateFields()
    {

        name1.text = contact.contact;
        phoneNumber.text = contact.phoneNumber;
        email.text = contact.email;
        address.text = contact.address;
    }
}
