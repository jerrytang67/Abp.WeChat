using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Common.Extensions;
using EasyAbp.Abp.WeChat.Pay.ApiRequests;
using EasyAbp.Abp.WeChat.Pay.Options;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;

namespace EasyAbp.Abp.WeChat.Pay.Security;

public class WeChatPayAuthorizationGenerator : IWeChatPayAuthorizationGenerator
{
    private readonly IAbpWeChatPayOptionsProvider _weChatPayOptionsProvider;

    public WeChatPayAuthorizationGenerator(IAbpWeChatPayOptionsProvider weChatPayOptionsProvider)
    {
        _weChatPayOptionsProvider = weChatPayOptionsProvider;
    }

    public async Task<string> GenerateAuthorizationAsync(HttpMethod method, string url, string body, string mchId)
    {
        var options = await _weChatPayOptionsProvider.GetAsync(mchId);
        var timeStamp = DateTimeHelper.GetNowTimeStamp().ToString();
        var nonceStr = RandomStringHelper.GetRandomString();

        var requestModel = new WeChatPayApiRequestModel(method, url, body, timeStamp, nonceStr);
        var pendingSignature = requestModel.GetPendingSignatureString();
        var signString = RsaSign(pendingSignature, options.ApiKey);

        return
            $"WECHATPAY2-SHA256-RSA2048 mchid=\"{mchId}\",nonce_str=\"{nonceStr}\",timestamp=\"{timeStamp}\",serial_no=\"{options.CertificateSerialNumber},signature=\"{signString}";
    }

    private string RsaSign(string pendingSignature, string privateKey)
    {
        var bytesToSign = Encoding.UTF8.GetBytes(pendingSignature);
        var engine = new Pkcs1Encoding(new RsaEngine());
        using (var txtReader = new StringReader(privateKey))
        {
            var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtReader).ReadObject();
            engine.Init(true, keyPair.Private);
        }

        return Convert.ToBase64String(engine.ProcessBlock(bytesToSign, 0, bytesToSign.Length));
    }
}