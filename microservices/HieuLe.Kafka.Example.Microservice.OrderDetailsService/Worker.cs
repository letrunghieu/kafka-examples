using Confluent.Kafka;
using Confluent.SchemaRegistry;
using HieuLe.Kafka.Example.Microservice.Shared.Dtos;
using HieuLe.Kafka.Example.Microservice.Shared.Schemas;
using System.Net;

namespace HieuLe.Kafka.Example.Microservice.OrderDetailsService
{
    /// <summary>
    /// Validates the details of each order.
    /// - Is the quantity positive?
    /// - Is there a customerId
    /// - etc...
    /// This service could be built with Kafka Streams but we've used a Producer/Consumer pair
    /// including the integration with Kafka's Exactly Once feature (Transactions) to demonstrate
    /// this other style of building event driven services. Care needs to be taken with this approach
    /// as in the current release multi-node support is not provided for the transactional consumer
    /// (but it is supported inside Kafka Streams)
    /// </summary>
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;

        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            using var consumer = CreateConsumer("broker:29092", "http://schema-registry:8081");

            using var producer = CreateProducer("broker:29092", "http://schema-registry:8081");

            consumer.Subscribe(Topics.Orders.Name);
            producer.InitTransactions(TimeSpan.FromSeconds(10));
            logger.LogInformation("Kafka producer is initiated at: {time}", DateTimeOffset.Now);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var record = consumer.Consume(stoppingToken);
                    var order = record?.Message.Value;

                    var consumedOffsets = new List<TopicPartitionOffset>();
                    if (order != null && record != null)
                    {
                        producer.BeginTransaction();
                        try
                        {
                            if (order.state == OrderState.CREATED)
                            {
                                await producer.ProduceAsync(Topics.OrderValidations.Name,
                                    new Message<string, OrderValidation>
                                    {
                                        Key = order.id,
                                        Value = new OrderValidation()
                                        {
                                            orderId = order.id,
                                            checkType = OrderValidationType.ORDER_DETAILS_CHECK,
                                            validationResult = (IsOrderValid(order)
                                                ? OrderValidationResult.PASS
                                                : OrderValidationResult.FAIL)
                                        }
                                    });

                                //consumedOffsets.Add(
                                //    new TopicPartitionOffset(record.TopicPartition, consumer.Position(record.TopicPartition))
                                //    );
                            }

                            producer.SendOffsetsToTransaction(
                                consumer.Assignment.Select(a => new TopicPartitionOffset(a, consumer.Position(a))),
                                consumer.ConsumerGroupMetadata,
                                TimeSpan.FromSeconds(10)
                            );
                            producer.CommitTransaction();
                        }
                        catch (Exception)
                        {
                            producer.AbortTransaction();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            finally
            {
                consumer.Close();

                logger.LogInformation("Stopped at: {time}", DateTimeOffset.Now);
            }
        }

        private static bool IsOrderValid(Order order)
        {
            if (order.quantity < 0)
            {
                return false;
            }

            if (order.price < 0)
            {
                return false;
            }

            return true;
        }

        private IProducer<string, OrderValidation> CreateProducer(string bootstrapServers, string schemaRegistryUrl)
        {
            var clientId = Dns.GetHostName();

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = clientId,
                // The TransactionalId identifies this instance of the map words processor.
                // If you start another instance with the same transactional id, the existing
                // instance will be fenced.
                TransactionalId = $"{typeof(Worker).FullName}-{clientId}",
                EnableIdempotence = true,
            };

            var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig
            {
                Url = schemaRegistryUrl
            });

            return new ProducerBuilder<string, OrderValidation>(producerConfig)
                .SetKeySerializer(Topics.OrderValidations.KeySerDesFactory.MakeSerDes(schemaRegistry).Serializer)
                .SetValueSerializer(Topics.OrderValidations.ValueSerDesFactory.MakeSerDes(schemaRegistry).Serializer)
                .Build();
        }

        private IConsumer<string, Order> CreateConsumer(string bootstrapServers, string schemaRegistryUrl)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = typeof(Worker).FullName,
                GroupInstanceId = Dns.GetHostName(),
                // Offsets are committed using the producer as part of the
                // transaction - not the consumer. When using transactions,
                // you must turn off auto commit on the consumer, which is
                // enabled by default!
                EnableAutoCommit = false,
                // Enable incremental rebalancing by using the CooperativeSticky
                // assignor (avoid stop-the-world rebalances).
                PartitionAssignmentStrategy = PartitionAssignmentStrategy.CooperativeSticky,
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig
            {
                Url = schemaRegistryUrl
            });

            return new ConsumerBuilder<string, Order>(consumerConfig)
                .SetKeyDeserializer(Topics.Orders.KeySerDesFactory.MakeSerDes(schemaRegistry).Deserializer)
                .SetValueDeserializer(Topics.Orders.ValueSerDesFactory.MakeSerDes(schemaRegistry).Deserializer)
                .Build();
        }
    }
}