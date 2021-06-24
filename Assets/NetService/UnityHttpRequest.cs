using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace NetService {
  public class UnityHttpRequest : IHttpRequest {
    internal UnityWebRequest UnityWebRequest => unityWebRequest;
    private readonly UnityWebRequest unityWebRequest;
    private readonly Dictionary<string, string> headers;
    public Action<float> onUploadProgress;
    public Action<float> onDownloadProgress;
    public Action<HttpResponse> onSuccess;
    public Action<HttpResponse> onError;
    public Action<HttpResponse> onNetworkError;

    public UnityHttpRequest(UnityWebRequest unityWebRequest) {
      this.unityWebRequest = unityWebRequest;
      headers = new Dictionary<string, string>(HttpManager.GetSuperHeaders());
    }

    public IHttpRequest RemoveSuperHeaders() {
      foreach (var kvp in HttpManager.GetSuperHeaders()) {
        headers.Remove(kvp.Key);
      }
      return this;
    }

    public IHttpRequest SetHeader(string key, string value) {
      headers[key] = value;
      return this;
    }

    public IHttpRequest SetHeaders(IEnumerable<KeyValuePair<string, string>> headers) {
      foreach (var kvp in headers) {
        SetHeader(kvp.Key, kvp.Value);
      }
      return this;
    }

    public void OnUploadProgress(Action<float> onProgress) {
      onUploadProgress += onProgress;
    }

    public void OnDownloadProgress(Action<float> onProgress) {
      onDownloadProgress += onProgress;
    }

    public void OnSuccess(Action<HttpResponse> onSuccess) {
      this.onSuccess += onSuccess;
    }

    public void OnError(Action<HttpResponse> onError) {
      this.onError += onError;
    }

    public void OnNetworkError(Action<HttpResponse> onNetworkError) {
      this.onNetworkError += onNetworkError;
    }

    public bool RemoveHeader(string key) {
      return headers.Remove(key);
    }

    public IHttpRequest SetTimeout(int duration) {
      unityWebRequest.timeout = duration;
      return this;
    }
    public IHttpRequest SetRedirectLimit(int redirectLimit) {
      UnityWebRequest.redirectLimit = redirectLimit;
      return this;
    }

    public void Send() {
      foreach (var header in headers) {
        unityWebRequest.SetRequestHeader(header.Key, header.Value);
      }
      HttpManager.Instance.Send(this);
    }

    public void Abort() {
      HttpManager.Instance.Abort(this);
    }
  }
}