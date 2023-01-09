using System.Net.Http;
using System.Threading.Tasks;

namespace EasyAbp.Abp.WeChat.Pay.ApiRequests;

public interface IWeChatPayRequestGenerator
{
    Task<WeChatPayApiRequest> GenerateRequestParametersAsync<TRequest>(string mchId,
        string url,
        HttpMethod method,
        TRequest requestObj);
}