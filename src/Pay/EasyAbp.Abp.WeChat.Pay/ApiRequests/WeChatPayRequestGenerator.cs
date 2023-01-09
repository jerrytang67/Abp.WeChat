using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyAbp.Abp.WeChat.Pay.ApiRequests;

public class WeChatPayRequestGenerator : IWeChatPayRequestGenerator
{
    public Task<WeChatPayApiRequest> GenerateRequestParametersAsync<TRequest>(string mchId,
        string url,
        HttpMethod method,
        TRequest requestObj)
    {
        throw new NotImplementedException();
    }

    private string GenerateSignature(string mchId, string url, HttpMethod method, string body)
    {
        throw new NotImplementedException();
    }
}