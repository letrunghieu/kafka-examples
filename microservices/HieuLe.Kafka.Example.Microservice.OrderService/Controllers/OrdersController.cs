using System.Net;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using HieuLe.Kafka.Example.Microservice.OrderService.Domains.Dtos.Response;
using HieuLe.Kafka.Example.Microservice.Shared.Dtos;
using HieuLe.Kafka.Example.Microservice.Shared.Schemas;
using Microsoft.AspNetCore.Mvc;
using PG.BDS.MessageBus;

namespace HieuLe.Kafka.Example.Microservice.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IMessagePublisher _messagePublisher;

        public OrdersController(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public string GetOrder(string id)
        {
            return id;
        }

        // POST api/<OrdersController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<OrderResponse>> Post([FromBody] Order order)
        {
            //var config = new ProducerConfig
            //{
            //    BootstrapServers = "broker:29092",
            //    ClientId = Dns.GetHostName(),
            //};

            //using var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig
            //{
            //    Url = "http://schema-registry:8081"
            //});

            //using var producer = new ProducerBuilder<string, Order>(config)
            //    .SetKeySerializer(Topics.Orders.KeySerDesFactory.MakeSerDes(schemaRegistry).Serializer)
            //    .SetValueSerializer(Topics.Orders.ValueSerDesFactory.MakeSerDes(schemaRegistry).Serializer)
            //    .Build();

            //await producer.ProduceAsync(Topics.Orders.Name, new Message<string, Order> { Key = order.id, Value = order });
            await _messagePublisher.PublishAsync(Topics.Orders.Name, order.id, new Shared.ProtoMessages.Order
            {
                Id = order.id,
                CustomerId = order.customerId,
                Price = order.price,
                Product = Shared.ProtoMessages.Order.Types.Product.Jumpers,
                Quantity = order.quantity,
                State = Shared.ProtoMessages.Order.Types.OrderState.Created
            });
            return CreatedAtAction(nameof(GetOrder), new { id = order.id }, new OrderResponse(Url.Action(nameof(GetOrder), values: new { id = order.id }) ?? string.Empty));
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
