using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Options;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.ParametersModel;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.WeChat.Pay.Services.BasicPayment;

public abstract class BasicPaymentService : WeChatPayServiceBase, IBasicPaymentService
{
    public BasicPaymentService(AbpWeChatPayOptions options,
        IAbpLazyServiceProvider lazyServiceProvider) : base(options,
        lazyServiceProvider)
    {
    }

    public abstract Task CreateOrderAsync<T>(T input);
}