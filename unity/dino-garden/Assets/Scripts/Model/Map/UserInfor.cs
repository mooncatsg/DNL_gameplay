using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserInfor
{
    private static UserInfor singleton;
    public static UserInfor Instance { get { return singleton; } }
    public static void SetUserInfor(UserInfor userInfor) { singleton = userInfor;  }

    public int id;
    public string loginId;
    public string nonce;
    public string walletAddress;
    public string name;
    public int status;
    public string avatarUrl;
    public string money;
    public string dngBalance;
    public int claimAbleToken;
    public int claimableAt;
    public string desc;
    public string createdAt;
    public string updatedAt;
    public string machineLoan;
}


[System.Serializable]
public class PushToken
{
    public string tokenNotification;
}
[System.Serializable]
public class CheckVersion
{
    public string version;
}
