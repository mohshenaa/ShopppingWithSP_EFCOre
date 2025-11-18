using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopppingWithSP.Models
{
    public class Details
    {
        [Key]
        public int ItemCode { get; set; }


        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public string Item { get; set; } = default!;

        public string Size {  get; set; } = default!;

        public decimal Price { get; set; }

        public string? Description { get; set; } = default!;

        [DataType(DataType.ImageUrl), ScaffoldColumn(false)]
        public string? ImageUrl { get; set; }

        [NotMapped, DisplayName("Image")]
        public IFormFile? ImageFile { get; set; }

        public Order? Order { get; set; } = default!;

    }
}
