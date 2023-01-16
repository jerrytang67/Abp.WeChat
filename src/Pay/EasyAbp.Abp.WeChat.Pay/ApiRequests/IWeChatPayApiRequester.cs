using System;
using System.Threading.Tasks;
using System.Xml;
using JetBrains.Annotations;

namespace EasyAbp.Abp.WeChat.Pay.ApiRequests
{
    /// <summary>
    /// 定义了微信支付接口的请求器。
    /// </summary>
    public interface IWeChatPayApiRequester
    {
        [Obsolete("This method will be deprecated.")]
        Task<XmlDocument> RequestAsync(string url, string body, string mchId);

        Task<TResponse> RequestAsync<TResponse>(string url, [NotNull] string body, [CanBeNull] string mchId);
    }
}