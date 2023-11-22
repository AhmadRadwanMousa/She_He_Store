using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using She_He_Store.Models;
using She_He_Store.Services;
using System.Text;

namespace She_He_Store.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ModelContext _context;
        private readonly IMail _emailService;
        public CheckoutController(ModelContext context, IMail emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult AddOrder(OrderAndCardDetails orderCardDetails, int orderId)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (userId != 0)
            {
                if (orderCardDetails.card != null)
                {
                    var order = _context.Orders.SingleOrDefault(o => o.Orderid == orderId);
                    var card = orderCardDetails.card;
                    var existCard = _context.Cards.SingleOrDefault(c => c.Cardnumber == card.Cardnumber);

                    if (existCard != null)
                    {
                        card = existCard;
                    }
                    else
                    {
                        var newCard = new Card
                        {
                            Cardholdername = card.Cardholdername,
                            Cardnumber = card.Cardnumber,
                            Cvv = card.Cvv,
                            Expirtydate = card.Expirtydate,
                        };
                        _context.Cards.Add(newCard);
                        _context.SaveChanges();
                        card = newCard;
                    }

                    if (order.Totalamount > card.Cardamount)
                    {
                        TempData["NotSuccsesfullPayment"] = "true";
                        return RedirectToAction("CheckoutPage", "Home");
                    }
                    Payment payment = new Payment
                    {
                        Cardid = card.Cardid,
                        Paymentdate = DateTime.Now,
                        Paymentstatus = "Success",
                        Userid = userId,
                    };
                    _context.Payments.Add(payment);
                    _context.SaveChanges();

                    card.Cardamount = card.Cardamount - order.Totalamount;
                    order.Paymentid = payment.Paymentid;
                    order.Orderdate = DateTime.Now;
                    order.Shippingaddress = orderCardDetails.OrderNotes;
                    order.Orderstatus = "Pending";
                    _context.Cards.Update(card);
                    _context.Orders.Update(order);
                    _context.SaveChanges();

                    var user = _context.Users.SingleOrDefault(u => u.Userid == userId);
                    var userEmail = _context.Userlogins.SingleOrDefault(u => u.Userid == userId).Username;
                    var orderItems = _context.Orders.Where(o=>o.Orderid==order.Orderid).SelectMany(o=>o.Orderitems).Include(p=>p.Product).ToList();
                    string emailBody = GetEmailTemplate(orderItems,order.Orderid,Convert.ToDecimal(order.Totalamount));
                    MailData mailData = new MailData()
                    {
                        EmailTo = userEmail,
                        EmailToName = user.Fname + " " + user.Lname,
                        EmailSubject = "Your Order Has been placed!",
                        EmailBody = emailBody,
                       
                    };
                    _emailService.SendEmail(mailData);

                    TempData["PaymentSuccessfully"] = "true";
                    return RedirectToAction("Orders", "Home");
                }

            }
            return RedirectToAction("HomePage", "Home");

        }
        public string GetEmailTemplate(IEnumerable<Orderitem> orderItems,decimal orderId,decimal orderTotal)
        {
            StringBuilder emailBody = new StringBuilder();

            // Add the email header
            emailBody.AppendLine("<html>");
            emailBody.AppendLine("<body>");
            emailBody.AppendLine("<h2>Order Details</h2>");

            // Add the order information
            emailBody.AppendLine("<p><strong>Order ID:</strong> " + orderId + "</p>");

            // Add the table for order items
            emailBody.AppendLine("<table border='1'>");
            emailBody.AppendLine("<tr>");
            emailBody.AppendLine("<th>Product Name</th>");
            emailBody.AppendLine("<th>Quantity</th>");
            emailBody.AppendLine("<th>Unit Price</th>");
            emailBody.AppendLine("<th>Total Price</th>");
            emailBody.AppendLine("</tr>");

            // Add rows for each order item
            foreach (var orderItem in orderItems)
            {
                emailBody.AppendLine("<tr>");
                emailBody.AppendLine("<td>" + orderItem.Product.Productname + "</td>");
                emailBody.AppendLine("<td>" + orderItem.Quantity + "X </td>");
                emailBody.AppendLine("<td>" + ((orderItem.Product.Productprice) - (orderItem.Product.Sale / 100 * orderItem.Product.Productprice)) + "$   <del>" + orderItem.Product.Productprice + "$</del></td>");
                emailBody.AppendLine("<td>" + orderItem.Totalprice+ "$</td>");
                emailBody.AppendLine("</tr>");
            }

            // Add the order total row
            emailBody.AppendLine("<tr>");
            emailBody.AppendLine("<td colspan='3'><strong>Total</strong></td>");
            emailBody.AppendLine("<td>" + orderTotal + "</td>");
            emailBody.AppendLine("</tr>");
  
            emailBody.AppendLine("</table>");
            emailBody.AppendLine("<h3>Thank you for your order :)</h3>");
            emailBody.AppendLine("</body>");
            emailBody.AppendLine("</html>");
            // Return the HTML string
            return emailBody.ToString();
        }

    }
}
