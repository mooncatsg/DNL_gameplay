using System.Collections;
using UnityEngine;
using SimpleHTTP;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;

public class APIBase : MonoBehaviour
{
    #region base
#if UNITY_EDITOR
    public string accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYXlsb2FkIjp7ImlkIjoxOCwid2FsbGV0QWRkcmVzcyI6IjB4ZjBlNDhmNmMyY2EyZDM4MmM4NTExYTEwZDhjODQ5MmIxOWI5YmYyYSJ9LCJpYXQiOjE2NjA1NTc5MzUsImV4cCI6MTE2NjA1NTc5MzV9.Ef0bEdnUtQwGTwKwZs1n_VfZikL6IK08DwZMm2Pvzs0";
    public int landid = 57;
    public string deviceId { 
        get {
            return "b5161bf7675984fc173ad71f3c2de9fd";

            //return SystemInfo.deviceUniqueIdentifier; 
        } 
    }
#else
    public string accessToken;
    public int landid;
        public string deviceId { 
        get {
            return SystemInfo.deviceUniqueIdentifier; 
        } 
    }
#endif
    public delegate void BaseCallback(bool isOK,ResponseBase rp);

    protected void GetFullURL(string url, Action<bool, string> cb)
    {
        StartCoroutine(IEGetFullURL(url, cb));
    }
    IEnumerator IEGetFullURL(string url, Action<bool, string> cb)
    {
        Request request = new Request(url);
        Client http = new Client();
        LogRequest(request);
        yield return http.Send(request);
        LogResponse(http);
        if (cb != null)
        {
            cb(http.Response().IsOK(), http.Response().Body());
        }
    }

    protected void Get(string path, BaseCallback cb)
    {
        StartCoroutine(IEGet(path, cb));
    }
    IEnumerator IEGet(string path, BaseCallback cb)
    {
        Request request = new Request(APIConfig.API_HOST + path);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        request.AddHeader("landid", landid.ToString());
        request.AddHeader("deviceId",deviceId);

        Client http = new Client();
        LogRequest(request);
        yield return http.Send(request);
        LogResponse(http);
        if (cb != null)
        {
            var response = ProcessResult(http);
            cb.Invoke(http.Response().IsOK() && response.code == 1, response);
        }
    }

    protected void Post(string path, RequestBody body, BaseCallback cb)
    {
        StartCoroutine(IEPost(path, body, cb));
    }
    IEnumerator IEPost(string path, RequestBody body, BaseCallback cb)
    {
        Request request = new Request(APIConfig.API_HOST + path);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        request.AddHeader("landid", landid.ToString());
        request.AddHeader("deviceId", deviceId);
        request.Post(body);

        Client http = new Client();
        LogRequest(request);
        yield return http.Send(request);
        LogResponse(http);
        if (cb != null)
        {
            var response = ProcessResult(http);
            cb.Invoke(http.Response().IsOK() && response.code == 1, response);
        }
    }
    protected void PostRaw(string path, string data, BaseCallback cb, bool useToken = true)
    {
        StartCoroutine(IEPostRaw(path, data, cb, useToken));
    }
    IEnumerator IEPostRaw(string path, string data, BaseCallback cb, bool useToken = true)
    {
        Request request = new Request(APIConfig.API_HOST + path);
        request.Post(RequestBody.From(data));
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("landid", landid.ToString());
        request.AddHeader("deviceId", deviceId);
        if (useToken)
        {
            request.AddHeader("Authorization", "Bearer " + accessToken);
        }

        Client http = new Client();
        LogRequest(request);
        yield return http.Send(request);
        LogResponse(http);
        if (cb != null)
        {
            var response = ProcessResult(http);
            cb.Invoke(http.Response().IsOK() && response.code == 1, response);
        }
    }

    protected void PostForm(string path, FormData formData, BaseCallback cb)
    {
        StartCoroutine(IEPostForm(path, formData, cb));

    }
    IEnumerator IEPostForm(string path, FormData formData, BaseCallback cb)
    {

        // Create the request object and use the helper function `RequestBody` to create a body from FormData
        Request request = new Request(APIConfig.API_HOST + path);
        request.AddHeader("Authorization", "Bearer " + accessToken);
        request.AddHeader("landid", landid.ToString());
        request.AddHeader("deviceId", deviceId);
        request.Post(RequestBody.From(formData));

        // Instantiate the client
        Client http = new Client();
        // Send the request
        LogRequest(request);
        yield return http.Send(request);
        LogResponse(http);
        if (cb != null)
        {
            var response = ProcessResult(http);
            cb.Invoke(http.Response().IsOK() && response.code == 1, response);
        }
    }
    protected void PutRaw(string path, string data, BaseCallback cb)
    {
        StartCoroutine(IEPutRaw(path, data, cb));
    }
    IEnumerator IEPutRaw(string path, string data, BaseCallback cb)
    {
        Request request = new Request(APIConfig.API_HOST + path);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        request.AddHeader("landid", landid.ToString());
        request.AddHeader("deviceId", deviceId);
        request.Put(RequestBody.From(data));

        Client http = new Client();
        LogRequest(request);
        yield return http.Send(request);
        LogResponse(http);
        if (cb != null)
        {
            var response = ProcessResult(http);
            cb.Invoke(http.Response().IsOK() && response.code == 1, response);
        }
    }
    //NOTE: This put function not tested yet 
    protected void Put(string path, RequestBody body, BaseCallback cb)
    {
        StartCoroutine(IEPut(path, body, cb));
    }
    IEnumerator IEPut(string path, RequestBody body, BaseCallback cb)
    {
        Request request = new Request(APIConfig.API_HOST + path);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        request.AddHeader("landid", landid.ToString());
        request.AddHeader("deviceId", deviceId);
        request.Put(body);

        Client http = new Client();
        LogRequest(request);
        yield return http.Send(request);
        LogResponse(http);
        if (cb != null)
        {
            var response = ProcessResult(http);
            cb.Invoke(http.Response().IsOK() && response.code == 1, response);
        }
    }

    //NOTE: This DELETE function not tested yet 
    protected void Delete(string path, BaseCallback cb)
    {
        StartCoroutine(IEDelete(path, cb));
    }
    IEnumerator IEDelete(string path, BaseCallback cb)
    {
        Request request = new Request(APIConfig.API_HOST + path);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        request.AddHeader("landid", landid.ToString());
        request.AddHeader("deviceId", deviceId);
        request.Delete();

        Client http = new Client();
        LogRequest(request);
        yield return http.Send(request);
        LogResponse(http);
        if (cb != null)
        {
            var response = ProcessResult(http);
            cb.Invoke(http.Response().IsOK() && response.code == 1, response);
        }
    }

    private ResponseBase ProcessResult(Client http)
    {
        //HTTP Success
        if (http.IsSuccessful())
        {
            try
            {
                Response resp = http.Response();
                ResponseBase rp = JsonConvert.DeserializeObject<ResponseBase>(resp.Body());
                if ((rp.code == 401) || (rp.error != null && rp.error.statusCode == 401))
                {
                    StartCoroutine(DelayLogout());
                }
                if (!string.IsNullOrEmpty(rp.message))
                    if (rp.message.StartsWith("BadRequestError:"))
                        rp.message = rp.message.Remove(0, 16);
                return rp;
            }
            catch
            {
                return this.BuildNetworkError(null);
            }

        }
        else
        {
            return this.BuildNetworkError(http);
        }
    }

    IEnumerator DelayLogout()
    {
        yield return new WaitForEndOfFrame();
        PlayerPrefs.DeleteAll();
        if(SceneManager.GetActiveScene().buildIndex != 0)
            SceneManager.LoadScene(0);
    }


    private ResponseBase BuildNetworkError(Client http = null)
    {
        string msg = "Network connection error, please try later. ";
        if (http != null && http.Error() != null)
        {
            msg += http.Error();
        }
        return new ResponseBase()
        {
            code = -1,
            message = msg,
            data = null
        };
    }


    public void LogRequest(Request req)
    {
//#if UNITY_EDITOR
        Debug.Log(string.Format("<color=yellow>Send {0} to</color><color=white> {1}</color>\nWith headers: \n {2} \nAnd params: \n {3}", req.Method(), req.Url(), JsonConvert.SerializeObject(req.Headers()), JsonConvert.SerializeObject(req.Body())));
//#endif
    }
    public void LogResponse(Client client)
    {
//#if UNITY_EDITOR
        if (client.Response().IsOK())
            Debug.Log(string.Format("<color=green>Response {0} from</color><color=white> {1}</color>\nWith data: \n {2}", client.Request().Method(), client.Request().Url(), client.Response().Body()));
        else
            Debug.Log(string.Format("<color=red>Response {0} Error from</color><color=white> {1}</color>\nWith data: \n {2}", client.Request().Method(), client.Request().Url(), client.Response().Body()));
//#endif
    }
#endregion
}
