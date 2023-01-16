using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using EasyAbp.Abp.WeChat.Common.Extensions;
using EasyAbp.Abp.WeChat.Pay.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.WeChat.Pay.ApiRequests
{
    [Dependency(TryRegister = true)]
    public class DefaultWeChatPayApiRequester : IWeChatPayApiRequester, ITransientDependency
    {
        private readonly IAbpWeChatPayHttpClientFactory _httpClientFactory;
        private readonly IAbpWeChatPayOptionsProvider _optionsProvider;

        public DefaultWeChatPayApiRequester(IAbpWeChatPayHttpClientFactory httpClientFactory,
            IAbpWeChatPayOptionsProvider optionsProvider)
        {
            _httpClientFactory = httpClientFactory;
            _optionsProvider = optionsProvider;
        }

        public virtual async Task<XmlDocument> RequestAsync(string url, string body, string mchId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/xml")
            };

            var client = await _httpClientFactory.CreateAsync(mchId);
            var responseMessage = await client.SendAsync(request);
            var readAsString = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"微信支付接口请求失败。\n错误码: {responseMessage.StatusCode}，\n响应内容: {readAsString}");
            }

            var newXmlDocument = new XmlDocument();
            try
            {
                newXmlDocument.LoadXml(readAsString);
            }
            catch (XmlException)
            {
                throw new HttpRequestException($"请求接口失败，返回的不是一个标准的 XML 文档。\n响应内容: {readAsString}");
            }

            return newXmlDocument;
        }

        public async Task<TResponse> RequestAsync<TResponse>(string url, string body, string mchId)
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

        private WeChatPayApiRequestModel BuildRequestModel(HttpMethod method, string url, string body, string mchId)
        {
            return new WeChatPayApiRequestModel(method, url, body, mchId, RandomStringHelper.GetRandomString());
        }
    }
}