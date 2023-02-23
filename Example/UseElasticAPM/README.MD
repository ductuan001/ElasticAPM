# Distributed Tracing with Elastic APM
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

##### **Use Fleet Server**

* **Download Fleet Server** package at [Elastic Pages](https://artifacts.elastic.co/downloads/beats/elastic-agent/elastic-agent-8.5.3-windows-x86_64.zip)
* **Installation documentation** https://www.elastic.co/guide/en/fleet/8.5/install-fleet-managed-elastic-agent.html

### **.NET application configuration**
> Use .NET 6
#### **Configuration**
    #Add ElasticApm configuration to appsetting.json
    "ElasticApm": {
        "ServerUrl": "apm_server_host:apm_server_port",
        "SecretToken": "apm_server_secret_token",
        "ServiceName": "serviceName"
    },

#### **Install Elastic.Apm.NetCoreAll** NuGet
    https://www.nuget.org/packages/Elastic.Apm.NetCoreAll/

#### **Register** ElasticApm to your .NET application
    using Elastic.Apm.NetCoreAll;
    using Order.Constant;
    var builder = WebApplication.CreateBuilder(args);;
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();
// Start register ElasticApm 

    app.UseAllElasticApm(app.Configuration);
// End register ElasticApm
        
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
![alt text](/assets/images/elastic-apm.png)