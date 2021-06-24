using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetService {
  public sealed class HttpManager : MonoBehaviour {
    public static HttpManager Instance {
      get {
        if (instance != null) return instance;
        Init(new UnityHttpService());
        return instance;
      }
    }

    private static HttpManager instance;

    private IHttpService service;
    private Dictionary<string, string> superHeaders;
    private Dictionary<IHttpRequest, Coroutine> httpRequests;

    public static void Init(IHttpService service) {
      if (instance) return;

      instance = new GameObject(typeof(HttpManager).Name).AddComponent<HttpManager>();
      instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
      instance.superHeaders = new Dictionary<string, string>();
      instance.httpRequests = new Dictionary<IHttpRequest, Coroutine>();
      instance.service = service;
      DontDestroyOnLoad(instance.gameObject);
    }


    public static Dictionary<string, string> GetSuperHeaders() {
      return new Dictionary<string, string>(Instance.superHeaders);
    }

    public static void SetSuperHeader(string key, string value) {
      if (string.IsNullOrEmpty(key)) {
        throw new ArgumentException("Key cannot be null or empty.");
      }

      if (string.IsNullOrEmpty(value)) {
        throw new ArgumentException("Value cannot be null or empty, if you are intending to remove the value, use the RemoveSuperHeader() method.");
      }

      Instance.superHeaders[key] = value;
    }

    public static bool RemoveSuperHeader(string key) {
      if (string.IsNullOrEmpty(key)) {
        throw new ArgumentException("Key cannot be null or empty.");
      }
      return Instance.superHeaders.Remove(key);
    }

    public static IHttpRequest Get(string uri) {
      return Instance.service.Get(uri);
    }

    public static IHttpRequest Post(string uri, string postData) {
      return Instance.service.Post(uri, postData);
    }

    public static IHttpRequest Post(string uri, WWWForm formData) {
      return Instance.service.Post(uri, formData);
    }

    public static IHttpRequest Post(string uri, Dictionary<string, string> formData) {
      return Instance.service.Post(uri, formData);
    }

    public static IHttpRequest Post(string uri, List<IMultipartFormSection> multipartForm) {
      return Instance.service.Post(uri, multipartForm);
    }

    public static IHttpRequest Post(string uri, byte[] bytes, string contentType) {
      return Instance.service.Post(uri, bytes, contentType);
    }

    public static IHttpRequest Put(string uri, byte[] bodyData) {
      return Instance.service.Put(uri, bodyData);
    }

    public static IHttpRequest Put(string uri, string bodyData) {
      return Instance.service.Put(uri, bodyData);
    }

    public static IHttpRequest Delete(string uri) {
      return Instance.service.Delete(uri);
    }

    public static IHttpRequest Head(string uri) {
      return Instance.service.Head(uri);
    }


    internal void Send(IHttpRequest request) {
      var enumerator = CreateCoroutineRequest(request);
      var coroutine = StartCoroutine(enumerator);
      httpRequests.Add(request, coroutine);
    }

    private IEnumerator CreateCoroutineRequest(IHttpRequest request) {
      yield return service.Send(request);
      Instance.httpRequests.Remove(request);
    }

    internal void Abort(IHttpRequest request) {
      Instance.service.Abort(request);

      if (httpRequests.ContainsKey(request)) {
        StopCoroutine(httpRequests[request]);
      }
      Instance.httpRequests.Remove(request);
    }
  }
}