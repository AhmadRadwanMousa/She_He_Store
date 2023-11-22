using System.Collections;

namespace She_He_Store.Models
{
	public class OrderAndCardDetails
	{
        public IEnumerable<Orderitem> ?orderItems{ get; set;}
        public Card ?card { get; set; }
        public string? OrderNotes { get; set; }
    }
}
