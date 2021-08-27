using System;
using System.Threading;
using Confluent.Kafka;
using Serilog;
using Serilog.Core;

namespace ConsumerKafka
{
    class Program
    {
        static Logger logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

        static void Main(string[] args)
        {
            logger.Information("Testando o consumo de mensagens com Kafka");

            if (IsInValidArgs(args)) return;

            string bootServer = args[0];
            string topicName = args[1];

            logger.Information($"BootstrapServers = {bootServer}");
            logger.Information($"Topic = {topicName}");

            var config = new ConsumerConfig
            {
                BootstrapServers = bootServer,
                GroupId = $"{topicName}-group-0",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(topicName);

                    try
                    {
                        while (true)
                        {
                            var cr = consumer.Consume(cts.Token);
                            logger.Information($"Mensagem lida: {cr.Message.Value}");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumer.Close();
                        logger.Warning("Cancelada a execução do Consumer...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Exceção: {ex.GetType().FullName} | " +
                             $"Mensagem: {ex.Message}");
            }
        }


        static bool IsInValidArgs(string[] args)
        {
            if (args.Length != 2)
            {
                logger.Error("Informe dois argumentos: No primeiro o \"IP:porta\" " +
                    "para testes com o Kafka.  No segundo o Topico a ser consumido. " +
                    "Sugestão: \n" +
                    "dotnet run \"localhost:9092\" \"topic-test\"");             

                return true;
            }

            return false;
        }
    }
}
