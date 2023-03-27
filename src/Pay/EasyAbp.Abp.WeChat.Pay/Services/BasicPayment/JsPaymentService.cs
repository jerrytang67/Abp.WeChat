﻿using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Options;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.WeChat.Pay.Services.BasicPayment;

public class JsPaymentService : BasicPaymentService
{
    public JsPaymentService(AbpWeChatPayOptions options,
        IAbpLazyServiceProvider lazyServiceProvider) : base(options,
        lazyServiceProvider)
    {
    }

    public override Task CreateOrderAsync<T>(T input)
    {
        throw new System.NotImplementedException();
    }
}