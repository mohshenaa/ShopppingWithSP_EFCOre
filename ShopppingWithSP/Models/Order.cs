using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopppingWithSP.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }


        [Required]
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; } = default!;


        [Required]
        //[Phone,RegularExpression(@"^(\+8801[3 - 9]\d{8}|01[3 - 9]\d{8})$")]
        [DisplayName("Phone Number")]
        public string CustomerContact { get; set; } = default!;


        [DisplayName("Email")]
        [EmailAddress]
        public string CustomerEmail { get; set; } = default!;



        [Required]
        [DisplayName("Shipping Address"), DataType(DataType.MultilineText)]
        public string CustomerAddress { get; set; } = default!;


        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }=DateTime.Now;


        [DisplayName("Order Status")]
        public Status OrderStatus { get; set; }

        public List<Details> Details { get; set; } = new();
    }
    public enum Status
    {
        Delivered,
        Pending,
        Confirm,
        Cancel
    }

}
