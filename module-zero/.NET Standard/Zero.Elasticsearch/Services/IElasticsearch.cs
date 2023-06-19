using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zero.Elasticsearch.Models;

namespace Zero.Elasticsearch.Services
{
    public interface IElasticsearch
    {
        IElasticClient Client { get; }

        Task<IndexResult> CreateIndex<T>(string indexName) where T : class;

        Task<IndexResult> DeleteIndex<T>(string indexName) where T : class;

        Task<IndexResult> IndexDocument<T>(T document, string indexName = null) where T : class;

        Task<bool> ExistDocument<T>(string docId, string indexName = null) where T : class;

        Task<IndexResult> UpdateDocument<T>(string docId, T document, string indexName = null) where T : class;

        Task<DeleteResponse> DeleteDocument<T>(string documentId, string indexName = null) where T : class;

        /// <summary>
        /// Manually compose search-descriptor and search
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchDescriptor"></param>
        /// <returns></returns>
        Task<ElasticsearchResult<T>> Search<T>(SearchDescriptor<T> searchDescriptor) where T : class;

        /// <summary>
        /// Search all searchable fields for multiple words best found in the same field with fuzziness logic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyword"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<ElasticsearchResult<T>> Search<T>(string keyword, ElasticsearchOption option = null) where T : class;

        /// <summary>
        /// Search on a target field for specific word found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyword"></param>
        /// <param name="field"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<ElasticsearchResult<T>> SearchField<T>(string keyword, Expression<Func<T, object>> field, ElasticsearchOption option = null) where T : class;

        /// <summary>
        /// Search on a target field with wildcard keyword.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wildcard">eg. "*keyword")</param>
        /// <param name="field"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<ElasticsearchResult<T>> SearchWildcard<T>(string wildcard, Expression<Func<T, object>> field, ElasticsearchOption option = null) where T : class;

        /// <summary>
        /// Search on a target field with highlight result on words found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyword"></param>
        /// <param name="field"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<HighlightResult<T>> SearchHighlightText<T>(string keyword, Expression<Func<T, object>> field, ElasticsearchOption option = null) where T : class;

        /// <summary>
        /// Search on a target field with fragmented highlight result on words found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyword"></param>
        /// <param name="field"></param>
        /// <param name="fragmentSize"></param>
        /// <param name="numberOfFragments"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<HighlightResult<T>> SearchHighlightFragment<T>(
            string keyword, Expression<Func<T, object>> field, int fragmentSize = 150, int numberOfFragments = 5,
            ElasticsearchOption option = null) where T : class;

        /// <summary>
        /// Suggest auto-complete text on typing for Completion data-type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyword"></param>
        /// <param name="field"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<List<string>> SuggestAutoComplete<T>(string keyword, Expression<Func<T, object>> field,
            ElasticsearchOption option = null) where T : class;

        /// <summary>
        /// Suggest auto-complete text on typing for SearchAsYouType data-type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyword"></param>
        /// <param name="field"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<ElasticsearchResult<T>> SearchAsYouType<T>(string keyword, Expression<Func<T, object>> field, ElasticsearchOption option = null)
            where T : class;
    }
}