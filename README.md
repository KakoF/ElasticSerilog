# Documentação de Configuraçãoo do Serilog
Visão Geral
Este projeto utiliza o Serilog para logging estruturado com múltiplos sinks (destinos) incluindo Console, Debug e Elasticsearch.

### Configuração
#### Configuraçãoo do Ambiente

```csharp
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
```
- Define o ambiente de execução (Development, Staging, Production)

- Controla qual arquivo de configuração será carregado

#### Carregamento de Configurações
```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .Build();
```
- appsettings.json: Configuração base (obrigatória)

- appsettings.{ambiente}.json: Configuração específica por ambiente (opcional)

- reloadOnChange: true permite atualização em tempo real

#### Configuração do Serilog
Enriquecimento de Logs
```csharp
.Enrich.FromLogContext()          // Adiciona propriedades do contexto
.Enrich.WithExceptionDetails()    // Detalhes estruturados de exceçães
.Enrich.WithProperty("Environment", environment)
.Enrich.WithProperty("HostName", System.Net.Dns.GetHostName())
```
- Contexto completo do log

- Detalhamento de exceçães

- Propriedades customizadas: Ambiente e Nome do Host

#### Destinos (Sinks)
```csharp
//.WriteTo.Debug() (DEV)                  // Saída para Debug window
.WriteTo.Console()                // Saída para console
.WriteTo.Elasticsearch(...)       // Envio para Elasticsearch
```
#### Configuração do Elasticsearch
```csharp
public static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
    {
        AutoRegisterTemplate = true,  // Cria template automaticamente
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
        NumberOfReplicas = 1,        // 1 réplica para alta disponibilidade
        NumberOfShards = 2,          // 2 shards para distribuição
    };
}
```
- Formato do índice: nomedoprojeto-ambiente-ano-mes

#### Configuração do Host
```csharp
builder.Host.UseSerilog();
Integra Serilog com o sistema de logging do ASP.NET Core

4. Configuração de Níveis de Log (appsettings.json)
json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```
- Default: Nível Information para logs da aplicação

- Microsoft.AspNetCore: Nível Warning para reduzir logs do framework

Pré-requisitos
Pacotes NuGet
```text
Serilog.AspNetCore
Serilog.Sinks.Console
Serilog.Sinks.Debug
Serilog.Sinks.Elasticsearch
Serilog.Enrichers.Environment
Serilog.Exceptions
Variáveis de Ambiente
ASPNETCORE_ENVIRONMENT: Define o ambiente (Development/Staging/Production)
```
#### Configuração do Elasticsearch (appsettings.json)
```json
"ElasticConfiguration": {
  "Uri": "http://localhost:9200"
}
```
### Como Usar
#### Injeção do Logger
```csharp
private readonly ILogger<SeuController> _logger;

public SeuController(ILogger<SeuController> logger)
{
    _logger = logger;
}

```
#### Exemplos de Uso
```csharp
_logger.LogInformation("Operação iniciada");
_logger.LogWarning("Atenção: {Mensagem}", mensagem);
_logger.LogError(ex, "Erro na operação: {Detalhes}", detalhes);
_logger.LogDebug("Detalhes: {@Objeto}", objetoComplexo);
```
#### Enriquecimento Contextual
```csharp
using (LogContext.PushProperty("TransactionId", Guid.NewGuid()))
{
    _logger.LogInformation("Operação com ID transacional");
}
```
### Benefícios
- Logs Estruturados: Facilita análise e busca

- Múltiplos Destinos: Console, Debug e Elasticsearch simultaneamente

- Separação por Ambiente: Configuraçães específicas por ambiente

- Enriquecimento Automático: Contexto, exceçães, host, ambiente

- Performance: Configuração otimizada para produção

### Troubleshooting
Logs não aparecem no Elasticsearch
- Verifique a conexão com o Elasticsearch

- Confirme a variável ASPNETCORE_ENVIRONMENT

- Verifique as configuraçães em ElasticConfiguration:Uri

### Níveis de log incorretos
- Verifique a configuração em appsettings.json

- Confirme se o ambiente está correto

- Verifique sobreposição de configuraçães

### Logs duplicados
- O Serilog substitui o sistema de logging padrão

- Não usar AddLogging() adicional

### Monitoramento
#### Kibana (Visualização)
Acesse: http://localhost:5601

- Use o índice: nomedoprojeto-* para visualizar todos os logs

### Consultas úteis no Elasticsearch
```text
# Logs de erro no último dia
nivel: "Error" AND @timestamp:[now-1d TO now]

# Logs por host
hostname: "NOME_DO_HOST"

# Logs específicos de transação

TransactionId: "GUID_AQUI"
```

### Consideraçães de Produção
- Retenção de Logs: Configure políticas de retenção no Elasticsearch

- Segurança: Proteja o endpoint do Elasticsearch

- Performance: Ajuste NumberOfShards baseado no volume

- Backup: Implemente backup dos índices críticos

- Esta configuração proporciona uma solução robusta de logging pronta para desenvolvimento e produção.

