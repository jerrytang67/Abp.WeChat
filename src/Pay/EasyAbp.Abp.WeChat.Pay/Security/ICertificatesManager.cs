using System.Threading.Tasks;

namespace EasyAbp.Abp.WeChat.Pay.Security;

public interface ICertificatesManager
{
    Task<WeChatPayCertificate> GetCertificateAsync(string mchId);
}