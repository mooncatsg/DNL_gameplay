using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResponseBase
{
    public int code { get; set; }
    public string message { get; set; }
    public JToken data; 
    public Error error;


}

[System.Serializable]
public class Error
{
    public int statusCode { get; set; }
    public string message { get; set; }
    public string name { get; set; }
}
