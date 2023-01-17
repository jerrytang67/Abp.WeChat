using System.Security.Cryptography.X509Certificates;
using EasyAbp.Abp.WeChat.Common.Extensions;

namespace EasyAbp.Abp.WeChat.Pay.Security;

public sealed class WeChatPayCertificate
{
    public string MchId { get; set; }

    public X509Certificate2 X509Certificate { get; set; }

    public byte[] CertificateHashCode { get; set; }

    public WeChatPayCertificate(string mchId, byte[] certificateBytes, string password)
    {
        MchId = mchId;
        X509Certificate = new X509Certificate2(certificateBytes,
            password,
            X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
        CertificateHashCode = certificateBytes.Sha256();
    }
}