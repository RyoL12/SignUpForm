using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[Serializable]
public class SignUpData
{
    public string email;
    public string password;
    public string createDate;

    public SignUpData(string email, string password)
    {
        this.email = email;
        this.password = password;
        this.createDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}

public class SignUpForm : MonoBehaviour
{
    public InputField emailInput;
    public InputField passwordInput;
    public InputField rePasswordInput;
    public Button submitButton;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string rePassword = rePasswordInput.text;

        if (password != rePassword)
        {
            Debug.LogError("Passwords do not match!");
            return;
        }

        SignUpData data = new SignUpData(email, password);
        string json = JsonUtility.ToJson(data);
        Debug.Log("Generated JSON: " + json);

        StartCoroutine(SendToDatabase(json));
    }

    private IEnumerator SendToDatabase(string json)
    {
        string url = "https://binusgat.rf.gd/unity-api-test/api/auth/signup.php";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Sign-up successful!");
        }
        else
        {
            Debug.LogError("Sign-up failed: " + request.error);
        }
    }
} 
