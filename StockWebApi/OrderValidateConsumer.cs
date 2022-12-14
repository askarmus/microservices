using MassTransit;
using MessageModel;

namespace StockWebApi
{
    public class OrderCardNumberValidateConsumer : IConsumer<OrderMessage>
    {
        public async Task Consume(ConsumeContext<OrderMessage> context)
        {
            var data = context.Message;
            if (data.OrderId != "ass")
            {
                // invalid
            }
        }
    }
}
