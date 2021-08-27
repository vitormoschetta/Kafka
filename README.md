# Instalação do Apache Kafka em Docker Container

### Executar os containers:
```
docker-compose up -d --build
```

<br>

### Executar o Producer:
```
dotnet run "localhost:9092" "topic-test" "Mensagem 01" "Mensagem 02"
```

<br>

### Executar o Consumer:
```
dotnet run "localhost:9092" "topic-test"
```


<br>
<br>


# Instalação Binários no Host

### Baixar o Apache Kafka:

<https://kafka.apache.org/downloads>

Baixar os binários (.tgz) e descompactar.

Obs: Necessário ter o JDK do Java instalado.

<br>

### Executar Apache ZooKeeper:
Dentro da pasta onde foi descompactado o Kafka, executar:
```
bin/zookeeper-server-start.sh config/zookeeper.properties
```

<br>

### Executar Apache Kafka:
Dentro da pasta onde foi descompactado o Kafka, executar:
```
bin/kafka-server-start.sh config/server.properties
```


<br>

## Executar Apache Kafka:
Dentro da pasta onde foi descompactado o Kafka, executar:
```
bin/kafka-server-start.sh config/server.properties
```

<br>


# Operação

## Criar um Novo Tópico:
```
bin/kafka-topics.sh --create --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1 --topic LOJA_NOVO_PEDIDO
```

<br>

## Listar Tópicos Cadastrados:
```
bin/kafka-topics.sh --list --bootstrap-server localhost:9092
```

<br>

## Criar um Producer (Produtor de Mensagens):
```
bin/kafka-console-producer.sh --broker-list localhost:9092 --topic LOJA_NOVO_PEDIDO
```
A linha de comando então permitirá inserir mensagens.


<br>

## Criar um Consumer (Consumidor de Mensagens):
```
bin/kafka-console-consumer.sh --bootstrap-server localhost:9092 --topic LOJA_NOVO_PEDIDO --from-beginning
```
Aqui todas as mensagens armazenadas serão mostradas. Se uma nova mensagem for adicionada no terminal do Producer, aparecerá no Consumer automaticamente.