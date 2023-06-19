using System.Collections.Generic;

namespace Zero.Elasticsearch.Models
{
    public class HighlightResult<TDocument> where TDocument : class
    {
        public IReadOnlyCollection<HighlightDocument<TDocument>> Documents { get; }

        public long TotalDocuments { get; }

        public HighlightResult(IReadOnlyCollection<HighlightDocument<TDocument>> documents, long total)
        {
            Documents = documents;
            TotalDocuments = total;
        }
    }

    public class HighlightDocument<T> where T : class
    {
        public T Source { get; set; }
        public string[] Highlight { get; set; }
    }
}