using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AuthQRRequest
{
    public string signInToken;
}

[System.Serializable]
public class AuthQRResponse
{
    public string accessToken;
    public int landId;
}
