using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using Zero.Elasticsearch.Services;

namespace Zero.Elasticsearch.Extensions
{
    public static class ElasticsearchExtension
    {
        public static void AddElasticsearch(this IServiceCollection services, string uri, string defaultIndex)
        {
            var pool = new SingleNodeConnectionPool(new Uri(uri));
            var settings = new ConnectionSettings(
                pool,
                sourceSerializer: (builtin, setting) => new CustomJsonNetSerializer(builtin, setting));

            if (string.IsNullOrEmpty(defaultIndex) == false)
            {
                settings = settings.DefaultIndex(defaultIndex);
            }

            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);

            services.AddScoped<IElasticsearch, Services.Elasticsearch>();
        }
    }
}