using Elasticsearch.Net;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;

namespace Zero.Elasticsearch.Extensions
{
    public class CustomJsonNetSerializer : ConnectionSettingsAwareSerializerBase
    {
        public CustomJsonNetSerializer(IElasticsearchSerializer builtinSerializer, IConnectionConfigurationValues connectionSettings)
            : base(builtinSerializer, (IConnectionSettingsValues)connectionSettings) { }

        protected override JsonSerializerSettings CreateJsonSerializerSettings() =>
        new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };
    }
}