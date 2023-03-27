using System.Threading.Tasks;

namespace EasyAbp.Abp.WeChat.Pay.Services.BasicPayment;

public interface IBasicPaymentService : IAbpWeChatPayService
{
    Task CreateOrderAsync<T>(T input);
}