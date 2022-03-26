using SolrNet.Attributes;

namespace LegalDoc.Models
{

    public class Document
    {
        [SolrUniqueKey("id")]
        public int Id { get; set; }

        [SolrField("doctitle")]
        public string Title { get; set; }

        [SolrField("doctext")]
        public string DocText { get; set; }

        [SolrField("accepttime")]
        public DateTime AcceptTime { get; set; }

        [SolrField("author")]
        public string Author { get; set; }
    }
}
