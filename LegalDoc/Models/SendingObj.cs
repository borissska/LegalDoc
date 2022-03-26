using Microsoft.EntityFrameworkCore;

namespace LegalDoc.Models
{
    [Keyless]
    public class SendingObj
    {
        public string Stroka { get; set; }
        public IEnumerable<Document> Document { get; set; }
    }
}
