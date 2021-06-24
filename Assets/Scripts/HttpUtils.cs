using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class HttpUtils
{
  /// <summary>
  /// Format and append parameters to a uri
  /// </summary>
  /// <param name="uri">The uri to append the properties to.</param>
  /// <param name="parameters">A dictionary of parameters to append to the uri.</param>
  /// <returns>The uri with the appended parameters.</returns>
  public static string ConstructUriWithParameters(string uri, Dictionary<string, string> parameters)
  {
    if (parameters == null || parameters.Count == 0)
    {
      return uri;
    }

    var stringBuilder = new StringBuilder(uri);

    for (var i = 0; i < parameters.Count; i++)
    {
      var element = parameters.ElementAt(i);
      stringBuilder.Append(i == 0 ? "?" : "&");
      stringBuilder.Append(element.Key);
      stringBuilder.Append("=");
      stringBuilder.Append(element.Value);
    }
    return stringBuilder.ToString();
  }

  public static Uri Create(string host, string query, Dictionary<string, string> formData)
  {
    var builder = new StringBuilder();

    builder.Append(host);
    if (builder[builder.Length - 1] != '/') builder.Append("/");
    builder.Append(query);
    if (formData != null)
    {
      builder.Append("?");
      foreach (var iterator in formData)
      {
        builder.AppendFormat("{0}={1}", iterator.Key, iterator.Value);
        builder.Append("&");
      }
      builder.Remove(builder.Length - 1, 1);
    }
    return new Uri(builder.ToString());
  }
}
