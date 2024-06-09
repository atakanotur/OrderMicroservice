using Quartz;
using Business.RabbitMQ.Consumer;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework;
using System.Net.Mail;
using System.Net;
using System.Text;

public class DailyJob : IJob
{
    OrderMicroserviceDbContext _dbContext = new OrderMicroserviceDbContext();

    public async Task Execute(IJobExecutionContext context)
    {
        var orders = await RabbitMQConsumer.ReadMessages();
        List<OrdersLog> ordersLogList = new List<OrdersLog>();
        if (orders.Any())
        {
            foreach (var order in orders)
            {
                var ordersLog = new OrdersLog
                {
                    ProductId = order.Id,
                    CustomerId = order.CustomerId,
                    Price = order.Price,
                    Quantity = order.Quantity,
                    Status = order.Status,
                    CreatedAt = DateTime.Now,
                };
                ordersLogList.Add(ordersLog);
                await SaveOrderToDatabase(ordersLog);
            }
            await SendEmailWithOrdersLog(ordersLogList);
        }
    }

    private async Task SaveOrderToDatabase(OrdersLog ordersLog)
    {
        _dbContext.OrdersLog.Add(ordersLog);
        await _dbContext.SaveChangesAsync();
    }

    private async Task SendEmailWithOrdersLog(List<OrdersLog> ordersLogList)
    {
        var customerGroups = ordersLogList.GroupBy(o => o.CustomerId);

        foreach (var group in customerGroups)
        {
            var customerId = group.Key;
            var customerOrders = group.ToList();
            var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer != null)
            {
                string customerEmail = customer.Email;

                var emailBody = new StringBuilder();
                emailBody.AppendLine("Dear Customer,");
                emailBody.AppendLine("Here are your order logs:");
                foreach (var orderLog in customerOrders)
                {
                    emailBody.AppendLine($"ProductId: {orderLog.ProductId}, Price: {orderLog.Price}, Quantity: {orderLog.Quantity}, Status: {orderLog.Status}, CreatedAt: {orderLog.CreatedAt}");
                }
                emailBody.AppendLine("Best regards,");
                emailBody.AppendLine("Your Company");

                await SendEmailAsync(customerEmail, "Your Order Logs", emailBody.ToString());
            }
        }
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using (var message = new MailMessage())
        {
            message.From = new MailAddress("twofatask@gmail.com");
            message.To.Add(new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = false;

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("twofatask@gmail.com", "vxim djoj snqg hmrg");
                client.EnableSsl = true;
                await client.SendMailAsync(message);
            }
        }
    }
}
