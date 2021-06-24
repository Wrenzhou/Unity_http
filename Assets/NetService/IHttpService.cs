using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetService {
  public interface IHttpService {
    IHttpRequest Get(string uri);
    IHttpRequest Head(string uri);
    IHttpRequest Post(string uri, string postData);
    IHttpRequest Post(string uri, WWWForm formData);
    IHttpRequest Post(string uri, Dictionary<string, string> formData);
    IHttpRequest Post(string uri, List<IMultipartFormSection> multipartForm);
    IHttpRequest Post(string uri, byte[] bytes, string contentType);
    IHttpRequest Put(string uri, byte[] bodyData);
    IHttpRequest Put(string uri, string bodyData);
    IHttpRequest Delete(string uri);
    IEnumerator Send(IHttpRequest request);
    void Abort(IHttpRequest request);
  }
}