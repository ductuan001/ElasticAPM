# Distributed Tracing with Elastic APM and OpenTelementry
## **Installation**
### **Prerequisites**
* Elasticsearch [Elastic pages](https://www.elastic.co/guide/en/elasticsearch/reference/8.5/docker.html)
* Kibana [Elastic Pages](https://www.elastic.co/guide/en/kibana/8.5/docker.html)
#### **Install APM Integration**
> Use **Elastic APM Server** or **Fleet server** to serve APM data and stream to ElasticSearch
##### **Use Elastic APM Server**
* **Download APM Server** package at [Elastic Pages](https://www.elastic.co/downloads/past-releases/apm-server-8-5-3)
* **Configuration**



        # apm-server.yml
        apm-server:
            host: "apm_server_host:apm_server_port"
            auth:
                secret_token: "uuid (32 characters)"

        #-------------------------- Elasticsearch output --------------------------
        output.elasticsearch:
            hosts: ["elastic_server"]
            username: "elastic_user"
            password: "elastic_password"

For more information, see "[Elastic APM configuration](https://www.elastic.co/guide/en/apm/guide/8.5/configuration-process.html)."

* **Run**
    
    APM require root/administrator privileges

        PS C:\Program Files\APM-Server> .\apm-server.exe

#### **Use Fleet Server**

* **Download Fleet Server** package at [Elastic Pages](https://artifacts.elastic.co/downloads/beats/elastic-agent/elastic-agent-8.5.3-windows-x86_64.zip)
* **Installation documentation** https://www.elastic.co/guide/en/fleet/8.5/install-fleet-managed-elastic-agent.html

#### **Install otelcol-contrib collector**
* **Download otelcol-contrib** collector https://github.com/open-telemetry/opentelemetry-collector-releases/releases
#### **Configuration**
    # config.yml
    receivers:
    otlp:
        protocols:
        grpc:
        http:

    processors:
    batch:

    exporters:
    logging:
        verbosity: detailed
    otlp:
        endpoint: "apm_server_host:apm_server_port"
        tls:
            insecure: true
            insecure_skip_verify: true
        headers:
            Authorization: "Bearer apm-server_auth_secret_token"

    pipelines:
        traces:
            receivers: [otlp]
            exporters: [logging, otlp]
        metrics:
            receivers: [otlp]
            exporters: [logging, otlp]
        logs:
            receivers: [otlp]
            exporters: [logging, otlp]

### **.NET application configuration**
> Use .NET 6
#### **Configuration**
    #Add ElasticApm configuration to appsetting.json
    "OTEL_EXPORTER_OTLP_ENDPOINT": "http://localhost:4318"

#### **Install Elastic.Apm.NetCoreAll** NuGet
    OpenTelemetry.Exporter;
    OpenTelemetry.Trace;
    OpenTelemetry.Resources;

#### **Register** ElasticApm to your .NET application
    using OpenTelemetry.Exporter;
    using OpenTelemetry.Trace;
    using OpenTelemetry.Resources;

    var builder = WebApplication.CreateBuilder(args);;
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

// Start register OpenTelemetry 

    builder.Services.AddOpenTelemetry()
        .WithTracing(b =>
        {
            b.AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .ConfigureResource(r =>
                {
                    r.AddService(
                        serviceName: {{service name}},
                        serviceVersion: {{service version}}
                    );
                })
                .AddSource({{service name}})
                .AddOtlpExporter(opt =>
                {
                    opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                });
        });
// End register OpenTelemetry

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();

## **RUN Application**

## **APM result**
![alt text](/images/elastic-apm.png)
