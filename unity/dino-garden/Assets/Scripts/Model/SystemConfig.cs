using System.Collections.Generic;

[System.Serializable]
public class SystemConfig : SystemInfoData
{   
    public int prod_version_code;
    public int dev_version_code;
    public SystemInfoData dev;
    public SystemInfoData prod;
    public string ip_url;
    public List<string> white_list_ip;
}

[System.Serializable]
public class SystemInfoData
{
    public string env;
    public bool maintain;
    public string maintainMessage;

}