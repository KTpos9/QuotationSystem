namespace Zero.Elasticsearch.Models
{
    public class ElasticsearchOption
    {
        /// <summary>
        /// Index name to search
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// Start from result number (zero-base index)
        /// </summary>
        public int? From { get; set; }

        /// <summary>
        /// Number of result
        /// </summary>
        public int? Size { get; set; }
    }
}