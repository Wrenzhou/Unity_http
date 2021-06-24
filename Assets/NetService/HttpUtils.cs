using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class HttpUtils {
  public static string Createurl(string uri, Dictionary<string, string> parameters) {
    if (parameters == null || parameters.Count == 0) {
      return uri;
    }
    var stringBuilder = new StringBuilder(uri);
    for (var i = 0; i < parameters.Count; i++) {
      var element = parameters.ElementAt(i);
      stringBuilder.Append(i == 0 ? "?" : "&");
      stringBuilder.Append(element.Key);
      stringBuilder.Append("=");
      stringBuilder.Append(element.Value);
    }
    return stringBuilder.ToString();
  }

  public static string Createurl(string host, string query, Dictionary<string, string> formData) {
    var builder = new StringBuilder();
    builder.Append(host);
    if (builder[builder.Length - 1] != '/') builder.Append("/");
    builder.Append(query);
    if (formData != null) {
      builder.Append("?");
      foreach (var iterator in formData) {
        builder.AppendFormat("{0}={1}", iterator.Key, iterator.Value);
        builder.Append("&");
      }
      builder.Remove(builder.Length - 1, 1);
    }
    return builder.ToString();
  }
}
