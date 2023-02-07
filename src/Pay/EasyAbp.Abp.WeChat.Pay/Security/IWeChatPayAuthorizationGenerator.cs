using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace EasyAbp.Abp.WeChat.Pay.Security;

public interface IWeChatPayAuthorizationGenerator
{
    Task<string> GenerateAuthorizationAsync(HttpMethod method, string url, string body, [CanBeNull] string mchId = null);
}