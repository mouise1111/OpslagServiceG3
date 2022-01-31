using App;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Class1
{
    public Class1()
    {
    }

    static void Main()
    {

        try
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };

            // https://groups.google.com/g/rabbitmq-users/c/l0GQ4w3sYEU?pli=1
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "in",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                StorageRequest request = new StorageRequest();
                var response = Process(request, message);
                // request needs to be saved on disk (storage service).

                var serializer = new XmlSerializer(typeof(StorageResponse));
                var memory = new MemoryStream();
                serializer.Serialize(memory, response);
                var outputxml = Encoding.UTF8.GetString(memory.ToArray());
                OutQueue(outputxml);
            };

            channel.BasicConsume(queue: "in",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("An error occurred serialization");
        }

        catch (TimeoutException e)
        {
            Console.WriteLine("Connection creation to rabbitMQ timeout");
        }
        catch (IOException e)
        {
            Console.WriteLine("IOException");
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public static StorageResponse Process(StorageRequest request, string requestXmlString)
    {
        StorageResponse response = new StorageResponse();
        DatabaseConnection connection = new DatabaseConnection();
        if (validateRequest(requestXmlString))
        {
            response.Status = "Succes";
            response.Errors = new List<string>() { "" };
            request = parseRequest(requestXmlString);
            response.Ticket = request.Ticket;
            response.originalRequestString = "";
            connection.Bewaar(requestXmlString);
        }
        else
        {
            response.Status = "Failure";
            response.Errors = new List<string>()
            {
                "XSD validation failed"
            };
            response.Ticket = "";  // is onbekend
            // gemaakte keuze: igv fout, stuur inkomende request terug naar afzender om qfzender toe te lqten om te debuggen
            response.originalRequestString = requestXmlString;
        }

        return response;
    }

    static Boolean validateRequest(String requestXmlString)
    {
        bool validationResult = true;

        try
        {
            XDocument xdoc = new XDocument();

            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(null, "StorageRequest.xsd");

            xdoc = XDocument.Parse(requestXmlString);

            xdoc.Validate(schemaSet, (o, e) =>
            {
                Console.WriteLine("{0}", e.Message);
                validationResult = false;
            });
        }
        catch (XmlSchemaValidationException e)
        {
            Console.WriteLine("Xml schema validation error.");
            validationResult = false;
        }
        catch (System.IO.FileNotFoundException e)
        {
            Console.WriteLine("XSD file not found.");
            validationResult = false;
        }
        catch (XmlException e)
        {
            Console.WriteLine("Malformed Xml, load or parse error.");
            validationResult = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            validationResult = false;
        }
        return validationResult;
    }

    static StorageRequest parseRequest(String request)
    {
        StorageRequest requestObj = new StorageRequest();

        try
        {
            XDocument xdoc = new XDocument();

            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(null, "StorageRequest.xsd");

            xdoc = XDocument.Parse(request);

            String ticket = xdoc.Descendants("Ticket").FirstOrDefault().Value.ToString();
            String documentType = xdoc.Descendants("DocumentType").FirstOrDefault().Value.ToString();
            String binary = xdoc.Descendants("Binary").FirstOrDefault().Value.ToString();

            requestObj.Ticket = ticket;
            requestObj.DocumentType = documentType;
            requestObj.Binary = binary;

            Console.WriteLine("Ticket No = {0}", ticket);
            Console.WriteLine("DocumentType = {0}", documentType);
            Console.WriteLine("Binary = {0}", binary);

        }
        catch (XmlException e)
        {
            Console.WriteLine("Malformed Xml, load or parse error.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return requestObj;
    }

    public static void OutQueue(string data)
    {

        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare("Outqueue", false, false, false, null);
            channel.BasicPublish(string.Empty, "Outqueue", null, Encoding.UTF8.GetBytes(data));
        }

        //Console.WriteLine(" Press [enter] to exit.");
        //Console.ReadLine();
    }

    //moet dit nog staan?
    //private static string GetMessage(string[] args)
    //{
    //    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
    //}


    static void storagerequestValidationEventHandler(object sender, ValidationEventArgs e)
    {
        if (e.Severity == XmlSeverityType.Warning)
        {
            Console.Write("WARNING: ");
            Console.WriteLine(e.Message);
        }
        else if (e.Severity == XmlSeverityType.Error)
        {
            Console.Write("ERROR: ");
            Console.WriteLine(e.Message);



        }
    }
}
