using Confluent.Kafka;

namespace WebApplicationTicketsCRUD.Kafka;

public class TestKafka
{
    private IProducer<Null, string> _producer;

    public TestKafka(ProducerConfig producerConfig)
    {
        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    }

    public void SendNotifyToLocalConsole(String str)
    {
        _producer.Produce("quickstart", new Message<Null, string> { Value = str });
        _producer.Flush(TimeSpan.FromSeconds(3));
    }
}