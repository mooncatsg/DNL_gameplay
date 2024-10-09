using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : FastSingleton<FirebaseManager>
{

    Firebase.FirebaseApp app;

    public string PushToken { get; private set;}
    public bool shouldUploadToken = false;
    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        GameObject go = new GameObject("FirebaseManager");
        go.AddComponent<FirebaseManager>();
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(FirebaseManager.instance);
    }
    void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        UnityEngine.Debug.LogError("FirebaseManager.Start");
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventAppOpen);
    }
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
        PushToken = token.Token;
        if(shouldUploadToken)
        {
            APIMng.Instance.APIPushToken(FirebaseManager.instance.PushToken, null);
            shouldUploadToken = false;
        }    
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }

}
