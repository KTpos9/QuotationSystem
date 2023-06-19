using System.Collections.Generic;

namespace Zero.Elasticsearch.Models
{
    public class ElasticsearchResult<TDocument> where TDocument : class
    {
        public IReadOnlyCollection<TDocument> Documents { get; }
        public long TotalDocuments { get; }

        public ElasticsearchResult(IReadOnlyCollection<TDocument> documents, long total)
        {
            Documents = documents;
            TotalDocuments = total;
        }
    }
}