using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ClientAuthentication
{
    //ref: 
    //1. https://developer.okta.com/blog/2018/02/01/secure-aspnetcore-webapi-token-auth#create-the-token-service
    //2. https://www.codemag.com/Article/2105051/Implementing-JWT-Authentication-in-ASP.NET-Core-5
    
        public interface ITokenService
        {
             Task<string> GetToken();
        }
        
  public class CommonTokenService : ITokenService
  {
    private CommonToken token = new();
    private string _client_id;
    private string _client_secret;
    private string _token_url;
    public static ITokenService Instance;
    public CommonTokenService(string client_id, string client_secret, string token_url)
    {
          this._client_id = client_id;
          this._client_secret = client_secret;
          this._token_url=token_url;
          Instance = this;
    }

    public async Task<string> GetToken()
    {
      if (!this.token.IsValidAndNotExpiring)
      {
        this.token = await this.GetNewAccessToken();
      }
      return token.AccessToken;
    }

    private async Task<CommonToken> GetNewAccessToken()
    {
      var token = new CommonToken();
      var client = new HttpClient();
      var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{this._client_id}:{this._client_secret}");
      client.DefaultRequestHeaders.Authorization =
      	new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCreds));

      var postMessage = new Dictionary<string, string>();
      postMessage.Add("grant_type", "client_credentials");
      postMessage.Add("scope", "access_token");
      var request = new HttpRequestMessage(HttpMethod.Post, this._token_url)
      {
        Content = new FormUrlEncodedContent(postMessage)
      };

      var response = await client.SendAsync(request);
      if(response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        token = JsonConvert.DeserializeObject<CommonToken>(json);
        token.ExpiresAt = DateTime.UtcNow.AddSeconds(this.token.ExpiresIn);
      }
      else
      {
        throw new ApplicationException("Unable to retrieve access token from Okta");
      }

      return token;
    }

    private class CommonToken
    {
      [JsonProperty(PropertyName = "access_token")]
      public string AccessToken { get; set; }

      [JsonProperty(PropertyName = "expires_in")]
      public int ExpiresIn { get; set; }

      public DateTime ExpiresAt { get; set; }

      public string Scope { get; set; }

      [JsonProperty(PropertyName = "token_type")]
      public string TokenType { get; set; }

      public bool IsValidAndNotExpiring
      {
        get
        {
          return !String.IsNullOrEmpty(this.AccessToken) && this.ExpiresAt > DateTime.UtcNow.AddSeconds(30);
        }
      }
    }
  }
}
