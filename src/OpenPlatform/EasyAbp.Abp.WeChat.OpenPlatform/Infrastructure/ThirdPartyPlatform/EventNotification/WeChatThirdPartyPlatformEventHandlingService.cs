using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Common.EventHandling;
using EasyAbp.Abp.WeChat.Common.Infrastructure.Encryption;
using EasyAbp.Abp.WeChat.Common.Models;
using EasyAbp.Abp.WeChat.OpenPlatform.EventHandling;
using EasyAbp.Abp.WeChat.OpenPlatform.Infrastructure.Models.ThirdPartyPlatform;
using EasyAbp.Abp.WeChat.OpenPlatform.Infrastructure.ThirdPartyPlatform.Options;
using EasyAbp.Abp.WeChat.OpenPlatform.Infrastructure.ThirdPartyPlatform.Options.OptionsResolving.Contributors;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.Abp.WeChat.OpenPlatform.Infrastructure.ThirdPartyPlatform.EventNotification;

public class WeChatThirdPartyPlatformEventHandlingService :
    IWeChatThirdPartyPlatformEventHandlingService, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IWeChatThirdPartyPlatformAsyncLocal _asyncLocal;
    private readonly IWeChatThirdPartyPlatformOptionsProvider _optionsProvider;
    private readonly IWeChatNotificationEncryptor _weChatNotificationEncryptor;

    public WeChatThirdPartyPlatformEventHandlingService(
        IServiceProvider serviceProvider,
        IWeChatThirdPartyPlatformAsyncLocal asyncLocal,
        IWeChatThirdPartyPlatformOptionsProvider optionsProvider,
        IWeChatNotificationEncryptor weChatNotificationEncryptor)
    {
        _serviceProvider = serviceProvider;
        _asyncLocal = asyncLocal;
        _optionsProvider = optionsProvider;
        _weChatNotificationEncryptor = weChatNotificationEncryptor;
    }

    /// <summary>
    /// 授权事件通知接口，开发人员需要实现 <see cref="IWeChatThirdPartyPlatformAuthEventHandler"/> 处理器来处理回调请求。
    /// https://developers.weixin.qq.com/doc/oplatform/Third-party_Platforms/2.0/api/ThirdParty/token/component_verify_ticket.html
    /// https://developers.weixin.qq.com/doc/oplatform/Third-party_Platforms/2.0/api/Before_Develop/authorize_event.html#infotype-%E8%AF%B4%E6%98%8E
    /// </summary>
    public virtual async Task<WeChatEventHandlingResult> NotifyAuthAsync(string componentAppId,
        WeChatEventNotificationRequestModel request)
    {
        var options = await _optionsProvider.GetAsync(componentAppId);

        using var changeOptions = _asyncLocal.Change(options);

        var model = await DecryptMsgAsync<AuthNotificationModel>(options, request);

        var handlers = _serviceProvider.GetService<IEnumerable<IWeChatThirdPartyPlatformAuthEventHandler>>();

        foreach (var handler in handlers.Where(x => x.InfoType == model.InfoType))
        {
            var result = await handler.HandleAsync(model);

            if (!result.Success)
            {
                return result;
            }
        }

        return new WeChatEventHandlingResult(true);
    }

    /// <summary>
    /// 微信应用事件通知接口，开发人员需要实现 <see cref="IWeChatThirdPartyPlatformAppEventHandler"/> 处理器来处理回调请求。
    /// https://developers.weixin.qq.com/doc/oplatform/Third-party_Platforms/2.0/operation/thirdparty/prepare.html
    /// </summary>
    public virtual async Task<WeChatEventHandlingResult> NotifyAppAsync(string componentAppId,
        string authorizerAppId, WeChatEventNotificationRequestModel request)
    {
        var options = await _optionsProvider.GetAsync(componentAppId);

        using var changeOptions = _asyncLocal.Change(options);

        var model = await DecryptMsgAsync<WeChatAppNotificationModel>(options, request);

        var handlers = _serviceProvider.GetService<IEnumerable<IWeChatThirdPartyPlatformAppEventHandler>>();

        foreach (var handler in handlers.Where(x => x.Event == model.Event))
        {
            var result = await handler.HandleAsync(options.AppId, authorizerAppId, model);

            if (!result.Success)
            {
                return result;
            }
        }

        return new WeChatEventHandlingResult(true);
    }

    protected virtual async Task<T> DecryptMsgAsync<T>(IWeChatThirdPartyPlatformOptions options,
        WeChatEventNotificationRequestModel request) where T : ExtensibleObject, new()
    {
        return await _weChatNotificationEncryptor.DecryptPostDataAsync<T>(
            options.Token,
            options.EncodingAesKey,
            options.AppId,
            request.MsgSignature,
            request.Timestamp,
            request.Notice,
            request.PostData);
    }
}