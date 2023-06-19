using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zero.Elasticsearch.Models;

namespace Zero.Elasticsearch.Services
{
    public class Elasticsearch : IElasticsearch
    {
        private readonly IElasticClient client;

        public IElasticClient Client => client;

        public Elasticsearch(IElasticClient client)
        {
            this.client = client;
        }

        public async Task<IndexResult> CreateIndex<T>(string indexName) where T : class
        {
            bool isExist = client.Indices.Exists(indexName).Exists;
            if (isExist == false)
            {
                var indexResponse = await client.Indices.CreateAsync(indexName, c => c.Map<T>(m => m.AutoMap()));

                return new IndexResult { IsSuccess = indexResponse.IsValid, ErrorMessage = indexResponse.OriginalException != null ? indexResponse.OriginalException.Message : null };
            }

            return new IndexResult { IsSuccess = false, ErrorMessage = "Index Already Exists." };
        }

        public async Task<IndexResult> DeleteIndex<T>(string indexName) where T : class
        {
            bool isExist = client.Indices.Exists(indexName).Exists;
            if (isExist)
            {
                var indexResponse = await client.Indices.DeleteAsync(indexName);

                return new IndexResult { IsSuccess = indexResponse.IsValid, ErrorMessage = indexResponse.OriginalException != null ? indexResponse.OriginalException.Message : null };
            }

            return new IndexResult { IsSuccess = false, ErrorMessage = "Index Not Exists." };
        }

        public async Task<IndexResult> IndexDocument<T>(T document, string indexName = null) where T : class
        {
            var response = new IndexResponse();
            if (string.IsNullOrEmpty(indexName) == false)
            {
                response = await client.IndexAsync(document, x => x.Index(indexName));
            }
            else
            {
                response = await client.IndexDocumentAsync(document);
            }
            return new IndexResult { IsSuccess = response.IsValid, ErrorMessage = response.OriginalException != null ? response.OriginalException.Message : null };
        }

        public async Task<bool> ExistDocument<T>(string docId, string indexName = null) where T : class
        {
            var respnose = new ExistsResponse();
            if (string.IsNullOrEmpty(indexName) == false)
            {
                respnose = await client.DocumentExistsAsync<T>(docId, x => x.Index(indexName));
            }
            else
            {
                respnose = await client.DocumentExistsAsync<T>(docId);
            }
            return respnose.Exists;
        }

        public async Task<IndexResult> UpdateDocument<T>(string docId, T document, string indexName = null) where T : class
        {
            var isExist = await ExistDocument<T>(docId, indexName);
            if (isExist == false)
            {
                return await IndexDocument<T>(document, indexName);
            }

            UpdateResponse<T> response;
            if (string.IsNullOrEmpty(indexName) == false)
            {
                response = await client.UpdateAsync<T>(docId, u => u.Index(indexName).Doc(document));
            }
            else
            {
                response = await client.UpdateAsync<T>(docId, u => u.Doc(document));
            }
            return new IndexResult { IsSuccess = response.IsValid, ErrorMessage = response.OriginalException != null ? response.OriginalException.Message : null };
        }

        public async Task<DeleteResponse> DeleteDocument<T>(string documentId, string indexName = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(indexName) == false)
            {
                return await client.DeleteAsync<T>(documentId, descriptor => descriptor.Index(indexName));
            }

            return await client.DeleteAsync<T>(documentId);
        }

        public async Task<ElasticsearchResult<T>> Search<T>(SearchDescriptor<T> searchDescriptor) where T : class
        {
            var result = await client.SearchAsync<T>(searchDescriptor);
            return ResponseResult(result);
        }

        public async Task<ElasticsearchResult<T>> Search<T>(string keyword, ElasticsearchOption option = null) where T : class
        {
            var searchDescriptor = CreateSearchDescriptor<T>(option);
            searchDescriptor = searchDescriptor
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(keyword)
                        .Fuzziness(Fuzziness.Auto)
                    )
                );

            return await Search(searchDescriptor);
        }

        public async Task<ElasticsearchResult<T>> SearchField<T>(string keyword, Expression<Func<T, object>> field, ElasticsearchOption option = null) where T : class
        {
            var searchDescriptor = CreateSearchDescriptor<T>(option);
            searchDescriptor = searchDescriptor
                .Query(q => q
                    .Term(field, keyword)
                );

            return await Search(searchDescriptor);
        }

        public async Task<ElasticsearchResult<T>> SearchWildcard<T>(string wildcard, Expression<Func<T, object>> field, ElasticsearchOption option = null) where T : class
        {
            var searchDescriptor = CreateSearchDescriptor<T>(option);
            searchDescriptor = searchDescriptor
                .Query(q => q
                    .Wildcard(p => p
                        .Field(field)
                        .Value(wildcard)
                    )
                );

            return await Search(searchDescriptor);
        }

        public async Task<HighlightResult<T>> SearchHighlightText<T>(
            string keyword, Expression<Func<T, object>> field, ElasticsearchOption option = null) where T : class
        {
            var searchDescriptor = CreateSearchDescriptor<T>(option);
            searchDescriptor = searchDescriptor.Highlight(highlight => highlight
                    .Fields(
                        fields => fields
                            .Fragmenter(HighlighterFragmenter.Span)
                            .PreTags("<span class='search-highlight'>")
                            .PostTags("</span>")
                            .Field(field)
                    )
                )
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(keyword)
                        .Fuzziness(Fuzziness.Auto)
                        .Fields(fields => fields.Field(field))
                    )
                );

            return await SearchHighlight(searchDescriptor);
        }

        public async Task<HighlightResult<T>> SearchHighlightFragment<T>(
            string keyword, Expression<Func<T, object>> field, int fragmentSize = 150, int numberOfFragments = 5, ElasticsearchOption option = null) where T : class
        {
            var searchDescriptor = CreateSearchDescriptor<T>(option);
            searchDescriptor = searchDescriptor.Highlight(highlight => highlight
                    .Fields(
                        fields => fields
                            .Fragmenter(HighlighterFragmenter.Span)
                            .PreTags("<span class='search-highlight'>")
                            .PostTags("</span>")
                            .FragmentSize(fragmentSize)
                            .NoMatchSize(fragmentSize)
                            .NumberOfFragments(numberOfFragments)
                            .Field(field)
                    )
                )
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(keyword)
                        .Fuzziness(Fuzziness.Auto)
                        .Fields(fields => fields.Field(field))
                    )
                );

            return await SearchHighlight(searchDescriptor);
        }

        public async Task<List<string>> SuggestAutoComplete<T>(string keyword, Expression<Func<T, object>> field, ElasticsearchOption option = null) where T : class
        {
            var searchDescriptor = CreateSearchDescriptor<T>(option);
            searchDescriptor = searchDescriptor
                .Suggest(s => s.Completion("suggest", descriptor => descriptor
                    .Prefix(keyword)
                    .Fuzzy(fuzzinessDescriptor => fuzzinessDescriptor.Fuzziness(Fuzziness.Auto))
                    .Field(field)
                )
            );

            var result = await client.SearchAsync<T>(searchDescriptor);
            var options = result.Suggest.Values.SelectMany(x => x).SelectMany(x => x.Options).Select(x => x.Text).ToList();
            return options;
        }

        public async Task<ElasticsearchResult<T>> SearchAsYouType<T>(string keyword, Expression<Func<T, object>> field, ElasticsearchOption option = null) where T : class
        {
            var searchDescriptor = CreateSearchDescriptor<T>(option);
            searchDescriptor = searchDescriptor
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(keyword)
                        .Type(TextQueryType.BoolPrefix)
                        .Fields(fields => fields
                            .Field(field)
                        )
                    )
                );

            return await Search(searchDescriptor);
        }

        private SearchDescriptor<T> CreateSearchDescriptor<T>(ElasticsearchOption option) where T : class
        {
            var searchDescriptor = new SearchDescriptor<T>();
            if (string.IsNullOrWhiteSpace(option?.IndexName) == false)
            {
                searchDescriptor = searchDescriptor.Index(option.IndexName);
            }

            if (option?.From != null)
            {
                searchDescriptor = searchDescriptor.From(option.From);
            }

            if (option?.Size != null)
            {
                searchDescriptor = searchDescriptor.Size(option.Size);
            }

            return searchDescriptor;
        }

        private ElasticsearchResult<T> ResponseResult<T>(ISearchResponse<T> result) where T : class
        {
            if (result.IsValid == false)
            {
                throw new ElasticsearchClientException("Elasticsearch request failed.");
            }
            return new ElasticsearchResult<T>(result?.Documents, result.Total);
        }

        private async Task<HighlightResult<T>> SearchHighlight<T>(SearchDescriptor<T> searchDescriptor) where T : class
        {
            var result = await client.SearchAsync<T>(searchDescriptor);
            var highlights = result?.Hits.Select(x => new HighlightDocument<T>
            {
                Source = x.Source,
                Highlight = x.Highlight.SelectMany(h => h.Value).ToArray()
            }).ToList();

            return new HighlightResult<T>(highlights, result.Total);
        }
    }
}