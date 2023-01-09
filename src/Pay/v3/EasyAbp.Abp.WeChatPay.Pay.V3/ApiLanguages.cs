namespace EasyAbp.Abp.WeChat.Pay.V3
{
    public static class ApiLanguages
    {
        public const string English = "en_US";
        public const string SimplifiedChinese = "zh_CN";
        public const string TraditionalChinese = "zh_TW";
        public const string HongKongChinese = "zh_HK";

        public static string DefaultLanguage { get; set; } = English;
    }
}