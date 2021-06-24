using System;
using System.Collections.Generic;

namespace NetService {
  public interface IHttpRequest {
    IHttpRequest SetHeader(string key, string value);
    IHttpRequest SetHeaders(IEnumerable<KeyValuePair<string, string>> headers);
    IHttpRequest SetTimeout(int duration);
    IHttpRequest SetRedirectLimit(int redirectLimit);
    IHttpRequest RemoveSuperHeaders();
    bool RemoveHeader(string key);
    void OnUploadProgress(Action<float> onProgress);
    void OnDownloadProgress(Action<float> onProgress);
    void OnSuccess(Action<HttpResponse> onSuccess);
    void OnError(Action<HttpResponse> onError);
    void OnNetworkError(Action<HttpResponse> onNetworkError);
    void Send();
    void Abort();
  }
}