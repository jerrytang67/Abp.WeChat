using EasyAbp.Abp.WeChat.Common;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.WeChat.Pay.V3
{
    [DependsOn(typeof(AbpWeChatCommonAbstractionsModule))]
    public class AbpWeChatPayAbstractionsModule : AbpModule
    {
    }
}