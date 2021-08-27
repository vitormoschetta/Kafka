using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Serilog;
using Serilog.Core;

namespace ProducerKafka
{
    class Program
    {
        static Logger logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

        static async Task Main(string[] args)
        {
            logger.Information("Testando o envio de mensagens com Kafka");

            if (IsInValidArgs(args)) return;

            string bootServer = args[0];
            string topicName = args[1];

            logger.Information($"BootstrapServers = {bootServer}");
            logger.Information($"Topic = {topicName}");

            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = bootServer
                };

                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    // pegar do terceiro argumento em diante, que são as mensagens...
                    for (int i = 2; i < args.Length; i++)
                    {
                        var message = new Message<Null, string> { Value = args[i] };

                        var result = await producer.ProduceAsync(topicName, message);

                        logger.Information(
                            $"Mensagem: {args[i]} | " +
                            $"Status: { result.Status.ToString()}");
                    }
                }

                logger.Information("Concluído o envio de mensagens");
            }
            catch (Exception ex)
            {
                logger.Error($"Exceção: {ex.GetType().FullName} | " +
                             $"Mensagem: {ex.Message}");
            }
        }


        static bool IsInValidArgs(string[] args)
        {
            if (args.Length < 3)
            {
                logger.Error("Informe, nos argumentos, ao menos 3 parâmetros:" +
                    "No primeiro o \"IP:porta\" para testes com o Kafka. " +
                    "No segundo o Topico que receberá as mensagens. " +
                    "E do terceiro em diante as mensagens a serem enviadas. " +
                    "Sugestão: \n" +
                    "dotnet run \"localhost:9092\" \"topic-test\" \"Mensagem 01\" \"Mensagem 02\"");

                return true;
            }

            return false;
        }
    }
}
