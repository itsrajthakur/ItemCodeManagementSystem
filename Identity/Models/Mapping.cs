using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class Mapping
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserPrice { get; set; }
        public string UserItemCode { get; set; }
    }
}
