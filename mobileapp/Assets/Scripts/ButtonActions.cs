using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static API;

public class ButtonActions : MonoBehaviour
{
    public API api;
    MD5 md5 = new MD5();
    public GlobeManager GM;

    [SerializeField]
    GameObject welcomeTxt;
    [SerializeField]
    GameObject confirmLogInButton;
    [SerializeField]
    GameObject confirmAddContactButton;
    [SerializeField]
    GameObject confirmResetButton;
    [SerializeField]
    GameObject logInCanvas;

    string hashedPassword = null;
    string username = null;

    int userID = 48;

    public void SetLogInUsername(string st)
    {
        username = st;
        if (hashedPassword != null)
        {
            confirmLogInButton.SetActive(true);
        }
    }

    public void SetLoginPassword(string st)
    {
        if (st == null)
        {
            confirmLogInButton.SetActive(false);
            return;
        }

        hashedPassword = md5.Hash(st);

        if (username != null)
        {
            confirmLogInButton.SetActive(true);
        }
    }

    public void ConfirmLogin()
    {
        API_Login_Set input = new API_Login_Set { login = username, password = hashedPassword };
        api.Login(input);
    }

    // add contact
    string contact = null; // contact name
    string phoneNumber = null; // contact number
    string email = null; // contact email;
    string address = null; // contact address
    string coordinates = null; // x,y,z coord

    public void SetContactContact(string st)
    {
        contact = st;
        confirmAddContactButton.SetActive(true);
    }

    public void SetContactCoord(string st)
    {
        coordinates = st;
    }

    public void SetContactPhoneNumber(string st)
    {
        phoneNumber = st;
    }

    public void SetContactEmail(string st)
    {
        email = st;
    }

    public void SetContactAddress(string st)
    {
        address = st;
    }

    public void ConfirmAddContact()
    {
        API_AddContact_Set input = new API_AddContact_Set {contact = contact, userId = userID, address = address, email = email, coordinates = coordinates, phoneNumber = phoneNumber};
        api.AddContact(input);
    }

    private string ResetEmail;

    public void SetResetEmail(string st)
    {
        if (st == null)
        {
            confirmResetButton.SetActive(false);
            return;
        }

        if (st != null)
        {
            ResetEmail = st;
            confirmResetButton.SetActive(true);
        }
    }

    public void ConfirmReset()
    {
        API_Reset_Set input = new API_Reset_Set { ResetEmail =  ResetEmail};
        api.Reset(input);
    }

    [SerializeField]
    GameObject notificationCanvas;
    [SerializeField]
    GameObject errorCanvas;
    [SerializeField]
    TMP_Text error;
    [SerializeField]
    TMP_Text notification;
    public void DisplayError(string st)
    {
        errorCanvas.SetActive(true);
        error.text = st;
    }

    public void DisplayNotifiction(string st)
    {
        notificationCanvas.SetActive(true);
        notification.text = st;
    }

    public void LoggedIn(API_Login_Get instance)
    {
        if(instance.error == "")
        {
            Debug.Log("No Error");
        }
        else
        {
            DisplayError(instance.error);
            return;
        }

        userID = instance.id;

        logInCanvas.SetActive(false);
        welcomeTxt.GetComponent<Text>().text = "Welcome " + instance.firstName;

        API_SearchContacts_Set input = new API_SearchContacts_Set { search = "", userId = userID};
        api.SearchContacts(input);

        Destroy(welcomeTxt, 5f);
    }

    public void Searched(API_SearchContacts_Get instance)
    {
        if (instance.error == "")
        {
            
        }
        else
        {
            DisplayError(instance.error);
            return;
        }

        foreach (string contact in instance.results)
        {
            Debug.Log(contact);
        }

        
        
        List<API_EditContact_Set> contacts = new List<API_EditContact_Set>();

        foreach (string contact in instance.results)
        {
            int i = 0;
            string coord1 = "";
            string name1 = "";
            string phone1 = "";
            string email1 = "";
            string address1 = "";
            string tempS = "";

            while (contact[i] != '|') // Name
            {
                tempS += contact[i++];
            }

            //Debug.Log(tempS);
            name1 = tempS;
            tempS = "";
            i++;

            while (contact[i] != '|') // Phone
            {
                tempS += contact[i++];
            }

            phone1 = tempS;
            tempS = "";
            i++;

            while (contact[i] != '|') // Email
            { 
                tempS += contact[i++];
            }

            email1 = tempS;
            tempS = "";
            i++;

            while (contact[i] != '|') // Address
            {
                tempS += contact[i++];
            }

            address1 = tempS;
            tempS = "";
            i++;

            while (contact[i] != '|') // Coord
            {
                tempS += contact[i++];
            }

            Debug.Log(tempS);
            coord1 = tempS;

            API_EditContact_Set tempContact = new API_EditContact_Set
            {
                contact = name1,
                userId = userID,
                address = address1,
                email = email1,
                coordinates = coord1,
                phoneNumber = phone1
            };

            contacts.Add(tempContact);
        }

        GM.AddPins(contacts);
    }

    public void isReset(API_Reset_Get instance)
    {
        Debug.Log(instance);
        if (instance == null)
        {
            DisplayNotifiction("Reset Email Sent.\nPlease Check Your Email For A Reset Password Link");
        }
        else
        {
            DisplayError(instance.error);
        }
    }
}
