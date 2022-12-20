using System.Net.Http;
using Newtonsoft.Json;

namespace EasyAbp.Abp.WeChat.Official.Models
{
    public abstract class OfficialCommonRequest : IOfficialRequest
    {
        public virtual StringContent ToStringContent()
        {
            return new StringContent(JsonConvert.SerializeObject(this,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}