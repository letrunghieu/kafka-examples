using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using HieuLe.Kafka.Example.Microservice.OrderService.Domains.Dtos;
using HieuLe.Kafka.Example.Microservice.OrderService.Domains.Dtos.Response;
using Microsoft.AspNetCore.Mvc;

namespace HieuLe.Kafka.Example.Microservice.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
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
            var config = new ProducerConfig
            {
                BootstrapServers = "broker:29092",
            };

            using var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig
            {
                Url = "http://schema-registry:8081"
            });

            using var producer = new ProducerBuilder<string, Order>(config)
                .SetKeySerializer(new AvroSerializer<string>(schemaRegistry).AsSyncOverAsync())
                .SetValueSerializer(new AvroSerializer<Order>(schemaRegistry).AsSyncOverAsync())
                .Build();

            await producer.ProduceAsync("orders", new Message<string, Order> { Key = order.id, Value = order });
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
