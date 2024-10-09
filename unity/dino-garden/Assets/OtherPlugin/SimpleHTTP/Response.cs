using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace SimpleHTTP {
	public class Response {
		private int status;
		private string body;
		private byte[] rawBody;
		private string error;

		public Response(int status, string body, byte[] rawBody, string error) {
			this.status = status;
			this.body = body;
			this.rawBody = rawBody;
			this.error = error;
		}

		public T To<T>() {
			return JsonUtility.FromJson<T> (body);
		}

		public int Status() {
			return status;
		}

		public string Body() {
			return body;
		}

		public byte[] RawBody() {
			return rawBody;
		}

		public bool IsOK() {
			return status >= 200 && status < 300;
		}
		public string Error()
		{
			return error;
		}

		public string ToString() {
			return "status: " + status.ToString () + " - response: " + body.ToString ();
		}

		public static Response From(UnityWebRequest www) {
			return new Response ((int)www.responseCode, www.downloadHandler.text, www.downloadHandler.data, www.error);
		}
	}
}
