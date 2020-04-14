using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;
using static MD5;

public class API : MonoBehaviour
{
    [SerializeField]
    ButtonActions BA;

    [Serializable]
    public class API_Login_Set
    {
        public string login; // username
        public string password; // hashed password
    }

    [Serializable]
    public class API_Login_Get
    {
        public int id;
        public string firstName;
        public string lastName;
        public string error;
    }

    [Serializable]
    public class API_Register_Set
    {
        public string FirstName;
        public string LastName;
        public string Login; // username
        public string Password; // hashed password
        public string Email;
        public string Validation; // random MD5 hash
        public int ValidationStatus;
    }

    [Serializable]
    public class API_Register_Get
    {
        public string error;
    }

    [Serializable]
    public class API_AddContact_Set
    {
        public string contact; // contact name
        public int userId; // this.ID
        public string phoneNumber; // contact number
        public string email; // contact email;
        public string address; // contact address
        public string coordinates; // x,y,z coord
    }

    [Serializable]
    public class API_AddContact_Get
    {
        public string error;
    }

    [Serializable]
    public class API_EditContact_Set
    {
        public string contact; // contact name
        public int userId; // this.ID
        public string phoneNumber; // contact number
        public string email; // contact email;
        public string address; // contact address
        public string coordinates; // x,y,z coord
    }

    [Serializable]
    public class API_EditContact_Get
    {
        public string [] results; // ?
        public string error;
    }

    [Serializable]
    public class API_EditSearch_Set
    {
        public string id; //userID
    }

    [Serializable]
    public class API_EditSearch_Get
    {
        public string[] results; // ?
        public string error;
    }

    [Serializable]
    public class API_DeleteContact_Set
    {
        public string id; //userID
    }

    [Serializable]
    public class API_DeleteContact_Get
    {
       // nothing?
    }

    [Serializable]
    public class API_SearchContacts_Set
    {
        public string search;
        public int userId;
    }

    [Serializable]
    public class API_SearchContacts_Get
    {
        public string[] results; // undeliminated list
        public string error;
    }

    [Serializable]
    public class API_Reset_Set
    {
        public string ResetEmail;
    }

    [Serializable]
    public class API_Reset_Get
    {
        public string error;
    }

    UnityWebRequest CreateApiRequest(string url, string method, object body)
    {
        string bodyString = null;
        if (body is string)
        {
            bodyString = (string)body;
        }
        else if (body != null)
        {
            bodyString = JsonUtility.ToJson(body);
        }

        UnityWebRequest request = new UnityWebRequest();
        request.url = url;
        request.method = method;
        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : Encoding.UTF8.GetBytes(bodyString));
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 60;
        return request;
    }

    public UnityWebRequest CreateApiPostRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest("http://www.unitedplanet.ga/LAMPAPI" + actionUrl, UnityWebRequest.kHttpVerbPOST, body);
    }

    IEnumerator Post(UnityWebRequest request, System.Action<string> callback)
    {
       using (request)
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    public void Login(string username, string password)
    {
        StartCoroutine(Post(CreateApiPostRequest("/Login.php", new API_Login_Set { login = username, password = password }), Login));
    }

    public void Login(API_Login_Set login)
    {
        StartCoroutine(Post(CreateApiPostRequest("/Login.php", login), Login));
    }

    void Login(string reply)
    {
        API_Login_Get data = JsonUtility.FromJson<API_Login_Get>(reply);
        BA.LoggedIn(data);
    }

    public void Register(API_Register_Set set)
    {
        StartCoroutine(Post(CreateApiPostRequest("/Register.php", set), Register));
    }

    void Register(string reply)
    {
        API_Register_Get get = JsonUtility.FromJson<API_Register_Get>(reply);
    }

    public void AddContact(API_AddContact_Set set)
    {
        StartCoroutine(Post(CreateApiPostRequest("/AddContact.php", set), AddContact));
    }

    void AddContact(string reply)
    {
        Debug.Log(reply);
        API_AddContact_Get get = JsonUtility.FromJson<API_AddContact_Get>(reply);
    }

    public void EditContact(API_EditContact_Set set)
    {
        StartCoroutine(Post(CreateApiPostRequest("/EditContact.php", set), EditContact));
    }

    void EditContact(string reply)
    {
        API_EditContact_Get get = JsonUtility.FromJson<API_EditContact_Get>(reply);
    }

    public void EditSearch(API_EditSearch_Set set)
    {
        StartCoroutine(Post(CreateApiPostRequest("/EditSearch.php", set), EditSearch));
    }

    void EditSearch(string reply)
    {
        API_EditSearch_Get get = JsonUtility.FromJson<API_EditSearch_Get>(reply);
    }

    public void DeleteContact(API_DeleteContact_Set set)
    {
        StartCoroutine(Post(CreateApiPostRequest("/DeleteContact.php", set), DeleteContact));
    }

    void DeleteContact(string reply)
    {
        API_DeleteContact_Get get = JsonUtility.FromJson<API_DeleteContact_Get>(reply);
    }

    public void SearchContacts(API_SearchContacts_Set set)
    {
        StartCoroutine(Post(CreateApiPostRequest("/SearchContacts.php", set), SearchContacts));
    }

    void SearchContacts(string reply)
    {
        API_SearchContacts_Get get = JsonUtility.FromJson<API_SearchContacts_Get>(reply);
        BA.Searched(get);
    }

    public void Reset(API_Reset_Set set)
    {
        StartCoroutine(Post(CreateApiPostRequest("/Reset.php", set), Reset));
    }

    void Reset(string reply)
    {
        API_Reset_Get get = JsonUtility.FromJson<API_Reset_Get>(reply);
        BA.isReset(get);
    }
}
