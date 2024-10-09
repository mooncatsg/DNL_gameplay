using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeeplinkManager : MonoBehaviour
{
    public static DeeplinkManager Instance { get; private set; }
    public string deeplinkURL;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                onDeepLinkActivated(Application.absoluteURL);
            }
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void onDeepLinkActivated(string url)
    {
        PlayerPrefs.DeleteAll();
        string token = url.Split('/').Last();
        StartCoroutine(LoadSceneStart(token));
    }

    IEnumerator LoadSceneStart(string token)
    {
        var aync = SceneManager.LoadSceneAsync("Start");
        while(!aync.isDone)
        {
            yield return new WaitForEndOfFrame();
        }    
        APIMng.Instance.APIAuthQR(token, (isSuccess, response, errMsg, code) => {
            if (isSuccess)
            {
                GetUserInfo();
                if (!string.IsNullOrEmpty(FirebaseManager.instance.PushToken))
                    APIMng.Instance.APIPushToken(FirebaseManager.instance.PushToken, null);
                else
                    FirebaseManager.instance.shouldUploadToken = true;
            }
            else
            {

            }
        });
    }

    public void GetUserInfo()
    {
        APIMng.Instance.APIGetUserInfor((isSuccess2, userInfor, errMsg2, code2) =>
        {
            if (isSuccess2)
            {
                UserInfor.SetUserInfor((UserInfor)userInfor);
                GotoNextScene();
            }
        });
    }

    public void GotoNextScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
