using Gravitons.UI.Modal;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : FastSingleton<LoginController>
{
    [SerializeField] Button loginButton;
    [SerializeField] GameObject loginPanel;

    [SerializeField] GameObject Panel;
    [SerializeField] QRCodeDecodeController e_qrController;

    [SerializeField] GameObject scanLineObj;

    public static int GetVersionCode()
    {
#if UNITY_EDITOR
        return UnityEditor.PlayerSettings.Android.bundleVersionCode;
#else
        AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
        string packageName = context.Call<string>("getPackageName");
        AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
        return packageInfo.Get<int>("versionCode");
#endif
    }

    public int getSystemStatus(SystemInfoData systemConfig)
    {
        APIConfig.API_HOST = systemConfig.env;
        if (systemConfig.maintain)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private IEnumerator Start()
    {
        //StartCoroutine(GetIP("https://icanhazip.com/"));
        yield return new WaitForEndOfFrame();
        int systemStatus = 0;
        SystemConfig systemConfig = null;
        APIMng.Instance.APIGetsystem((isSuccess, response, errMsg, code) =>
        {
            if (!isSuccess)
            {
                ModalManager.Show("Oops!", "Sorry we cannot get system info.\nPlease check your internet connection or contact customer support.", new[] { new ModalButton() { Text = "OK", Callback = Application.Quit } });
            }
            else
            {
                int versionCode = GetVersionCode();
                Debug.Log("version code: " + versionCode);
                systemConfig = ((SystemConfig)response);
                if (systemConfig.dev_version_code == versionCode)
                {
                    systemStatus = getSystemStatus(systemConfig.dev);
                }
                else
                {
                    systemStatus = getSystemStatus(systemConfig);
                }
            }
        });

        yield return new WaitWhile(()=> systemStatus == 0);
        yield return new WaitForEndOfFrame();
        if(systemStatus == 2)
        {
            //Check whitelist maintain.
            string ExtenalIP = string.Empty;
            UnityWebRequest webRequest = UnityWebRequest.Get(systemConfig.ip_url);

            webRequest.SendWebRequest();
            yield return new WaitUntil(() => webRequest.isDone);
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                // Show results as text
                ExtenalIP = webRequest.downloadHandler.text.Trim().ToLower();
            }

            if (!string.IsNullOrEmpty(ExtenalIP))
            {
               if( systemConfig.white_list_ip.Contains(ExtenalIP))
                    systemStatus = 1;
            }
        }

        yield return new WaitForEndOfFrame();
        if (systemStatus == 1)
        {
            Debug.Log("Application Version : " + Application.version);
            APIMng.Instance.APICheckVersion(Application.version, (isSuccess, response, errMsg, code) => {
                if (!isSuccess)
                {
                    ModalManager.Show("Oops!", "Sorry we cannot get system info.\nPlease check your internet connection or contact customer support.", new[] { new ModalButton() { Text = "OK", Callback = Application.Quit } });
                }
                else if((bool) response)
                {
                    OnInitialized();
                }
                else
                {
                    ModalManager.Show("Update Available", "A new version of Dino Garden is available. Please update it to newest version!", new[] { new ModalButton() { Text = "OK", Callback = ShowDinolandHomepage } });
                }
            });
     
        }

        if (systemStatus == 2)
        {
            ModalManager.Show("Maintenance", "Sorry we are down for maintenance.\nPlease check back soon.",  new[] { new ModalButton() { Text = "OK", Callback = ShowDinolandHomepage } });
        }
    }

    public void ShowDinolandHomepage()
    {
        Application.OpenURL("https://dinoland.io/");
        Application.Quit();
    }


    public void OnInitialized()
    {
#if UNITY_EDITOR
        GetUserInfo(GotoNextScene);
#else
        if (PlayerPrefs.HasKey("loginData"))
        {
            GetUserInfo(GotoNextScene);
        }
        loginButton.onClick.AddListener(Play);
#endif
    }


    bool checkingToken = false;
    public void QrScanFinished(string dataText)
    {
        
        Debug.Log("QrScanFinished " + dataText);
        if (checkingToken)
        {
            return;
        }
        if (!IsValidToken(dataText))
        {
            return;
        }
        else
        {
            Stop();
            if (this.scanLineObj != null)
            {
                this.scanLineObj.SetActive(false);
            }
            checkingToken = true;
            APIMng.Instance.APIAuthQR(dataText, (isSuccess, response, errMsg, code) => {
                checkingToken = false; 
                if (isSuccess)
                {
                    GetUserInfo(GotoNextScene, ()=>{
                        ModalManager.Show("Fail to get user information.", "Your connection is interupted.\nYour Please try again later or contact to customer support.",
                       new[] { new ModalButton() { Text = "YES", Callback = ShowLogin } });
                    });
                    if (!string.IsNullOrEmpty(FirebaseManager.instance.PushToken))
                        APIMng.Instance.APIPushToken(FirebaseManager.instance.PushToken, null);
                    else
                        FirebaseManager.instance.shouldUploadToken = true;
                }
                else
                {
                    Debug.LogError("APIAuthQR fail");
                    ModalManager.Show("Invalid", "Your QR code is invalid.\nPlease retry.",
                    new[] { new ModalButton() { Text = "YES", Callback = Play } });
                }
            });
        }
    }

    public void GetUserInfo(Action successCallback, Action failCallback = null)
    {
        APIMng.Instance.APIGetUserInfor((isSuccess2, userInfor, errMsg2, code2) =>
        {
            if (isSuccess2)
            {
                UserInfor.SetUserInfor((UserInfor)userInfor);
                successCallback?.Invoke();
            }
            else
            {
                PlayerPrefs.DeleteAll();
                failCallback?.Invoke();
            }
        });
    }

    public bool IsValidToken(string token)
    {
        return true;
    }
    public void Reset()
    {
        if (this.e_qrController != null)
        {
            this.e_qrController.Reset();
        }

        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(true);
        }
    }
    public void ShowLogin()
    {
        this.loginPanel.SetActive(true);
        this.Panel.SetActive(false);
    }

    public void Play()
    {
        this.loginPanel.SetActive(false);
        this.Panel.SetActive(true);
        Reset();
        if (this.e_qrController != null)
        {
            this.e_qrController.StartWork();
        }
    }
    public void Stop()
    {
        if (this.e_qrController != null)
        {
            this.e_qrController.StopWork();
        }

        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(false);
        }
    }
    public void GotoNextScene()
    {
        if (this.e_qrController != null)
        {
            this.e_qrController.StopWork();
        }
        //Application.LoadLevel(scenename);
        SceneManager.LoadScene("MainScene");
    }
}
