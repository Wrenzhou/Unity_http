using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetService {
  public class UnityHttpService : IHttpService {

    public IHttpRequest Get(string uri) {
      return new UnityHttpRequest(UnityWebRequest.Get(uri));
    }

    public IHttpRequest Post(string uri, string postData) {
      return new UnityHttpRequest(UnityWebRequest.Post(uri, postData));
    }

    public IHttpRequest Post(string uri, WWWForm formData) {
      return new UnityHttpRequest(UnityWebRequest.Post(uri, formData));
    }

    public IHttpRequest Post(string uri, Dictionary<string, string> formData) {
      return new UnityHttpRequest(UnityWebRequest.Post(uri, formData));
    }

    public IHttpRequest Post(string uri, List<IMultipartFormSection> multipartForm) {
      return new UnityHttpRequest(UnityWebRequest.Post(uri, multipartForm));
    }

    public IHttpRequest Post(string uri, byte[] bytes, string contentType) {
      var unityWebRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST) {
        uploadHandler = new UploadHandlerRaw(bytes) {
          contentType = contentType
        },
        downloadHandler = new DownloadHandlerBuffer()
      };
      return new UnityHttpRequest(unityWebRequest);
    }

    public IHttpRequest Put(string uri, byte[] bodyData) {
      return new UnityHttpRequest(UnityWebRequest.Put(uri, bodyData));
    }

    public IHttpRequest Put(string uri, string bodyData) {
      return new UnityHttpRequest(UnityWebRequest.Put(uri, bodyData));
    }

    public IHttpRequest Delete(string uri) {
      return new UnityHttpRequest(UnityWebRequest.Delete(uri));
    }

    public IHttpRequest Head(string uri) {
      return new UnityHttpRequest(UnityWebRequest.Head(uri));
    }

    public IEnumerator Send(IHttpRequest request) {
      var unityHttpRequest = (UnityHttpRequest)request;
      var unityWebRequest = unityHttpRequest.UnityWebRequest;
      yield return unityWebRequest.SendWebRequest();
      var response = CreateResponse(unityWebRequest);
      unityHttpRequest.onDownloadProgress?.Invoke(unityWebRequest.downloadProgress);
      unityHttpRequest.onUploadProgress?.Invoke(unityWebRequest.uploadProgress);
      if (unityWebRequest.isNetworkError) {
        unityHttpRequest.onNetworkError?.Invoke(response);
      } else if (unityWebRequest.isHttpError) {
        unityHttpRequest.onError?.Invoke(response);
      } else {
        unityHttpRequest.onSuccess?.Invoke(response);
      }
    }

    public void Abort(IHttpRequest request) {
      var unityHttpRequest = request as UnityHttpRequest;
      if (unityHttpRequest?.UnityWebRequest != null && !unityHttpRequest.UnityWebRequest.isDone) {
        unityHttpRequest.UnityWebRequest.Abort();
      }
    }

    private static HttpResponse CreateResponse(UnityWebRequest unityWebRequest) {
      return new HttpResponse {
        Url = unityWebRequest.url,
        Bytes = unityWebRequest.downloadHandler?.data,
        Text = unityWebRequest.downloadHandler?.text,
        IsSuccessful = !unityWebRequest.isHttpError && !unityWebRequest.isNetworkError,
        IsHttpError = unityWebRequest.isHttpError,
        IsNetworkError = unityWebRequest.isNetworkError,
        Error = unityWebRequest.error,
        StatusCode = unityWebRequest.responseCode,
        ResponseHeaders = unityWebRequest.GetResponseHeaders(),
        Texture = (unityWebRequest.downloadHandler as DownloadHandlerTexture)?.texture
      };
    }
  }
}