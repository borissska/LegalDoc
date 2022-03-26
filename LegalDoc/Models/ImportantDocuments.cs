namespace LegalDoc.Models
{
    public class ImportantDocuments
    {
        public int Id { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int DocumentId { get; set; }
        public Document Document { get; set; }
    }
}
