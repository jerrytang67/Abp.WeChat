using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Options;
using EasyAbp.Abp.WeChat.Pay.Security;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.WeChat.Pay.ApiRequests
{
    [Dependency(TryRegister = true)]
    public class DefaultWeChatPayApiRequester : IWeChatPayApiRequester, ITransientDependency
    {
        private readonly IAbpWeChatPayHttpClientFactory _httpClientFactory;
        private readonly IWeChatPayAuthorizationGenerator _authorizationGenerator;
        private readonly IAbpWeChatPayOptionsProvider _optionsProvider;

        public DefaultWeChatPayApiRequester(IAbpWeChatPayHttpClientFactory httpClientFactory,
            IAbpWeChatPayOptionsProvider optionsProvider,
            IWeChatPayAuthorizationGenerator authorizationGenerator)
        {
            _httpClientFactory = httpClientFactory;
            _optionsProvider = optionsProvider;
            _authorizationGenerator = authorizationGenerator;
        }

        public Task<string> RequestAsync(string url, string body, string mchId = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TResponse> RequestAsync<TResponse>(string url, string body, string mchId = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            // Setting the request header for the http client.
            var options = await _optionsProvider.GetAsync(mchId);
            var language = options.AcceptLanguage ?? ApiLanguages.DefaultLanguage;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue("EasyAbp.Abp.WeChat.Pay", "1.0.0"));

            // Sending the request.
            var client = await _httpClientFactory.CreateAsync(mchId);
            var response = await client.SendAsync(request);

            throw new System.NotImplementedException();
        }

        public Task<string> RequestAsync(string url, object body, string mchId = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<TResponse> RequestAsync<TResponse>(string url, object body, string mchId = null)
        {
            throw new System.NotImplementedException();
        }
    }
}