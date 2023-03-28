using System.Threading.Tasks;
using JetBrains.Annotations;

namespace EasyAbp.Abp.WeChat.Pay.ApiRequests
{
    /// <summary>
    /// 定义了微信支付接口的请求器。
    /// </summary>
    public interface IWeChatPayApiRequester
    {
        Task<string> RequestAsync(string url, [NotNull] string body, [CanBeNull] string mchId = null);

        Task<TResponse> RequestAsync<TResponse>(string url, [NotNull] string body, [CanBeNull] string mchId = null);

        Task<string> RequestAsync(string url, [NotNull] object body, [CanBeNull] string mchId = null);

        Task<TResponse> RequestAsync<TResponse>(string url, [NotNull] object body, [CanBeNull] string mchId = null);
    }
}