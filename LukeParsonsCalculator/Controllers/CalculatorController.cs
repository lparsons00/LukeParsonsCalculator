using LukeParsonsCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using LukeParsonsCalculator.Services.Interfaces;
using System.Text.Json;
using System.Xml.Serialization;
using System.Xml;


namespace LukeParsonsCalculator.Controllers
{
    [ApiController]
    [Route("api/calculator")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;
        private readonly ILogger<CalculatorController> _logger;
        public CalculatorController(ICalculatorService calculatorService, ILogger<CalculatorController> logger)
        {
            _calculatorService = calculatorService;
            _logger = logger;
        }

        //public IActionResult Calculate([FromBody] MathsRequest mathsRequest)

        [HttpPost]
        [Consumes("application/json", "application/xml")]
        public async Task<IActionResult> Calculate()
        {
            try
            {
                var contentType = Request.ContentType;

                //read request body
                using var reader = new StreamReader(Request.Body);
                var content = await reader.ReadToEndAsync();
                _logger.LogInformation($"Recieved request with content type: {contentType}");
                _logger.LogInformation($"Request body: {content}");

                Operation operation;

                if (contentType != null && contentType.Contains("application/json"))
                {
                    //Parse json, allowing us to handle structure of req
                    var jsonDoc = JsonDocument.Parse(content);
                    var root = jsonDoc.RootElement;

                    //check structure of obj
                    if (root.TryGetProperty("Maths", out var mathsElement) && mathsElement.TryGetProperty("Operation", out var operationElement))
                    {
                        operation = ParseOperationFromJson(operationElement);
                    }
                    else
                    {
                        return BadRequest(new { Error = "Invalid JSON structure. Expected {'Maths': {'Operation': {...}}}" });
                    }
                }
                else if (contentType != null && contentType.Contains("application/xml"))
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(MathsRequest));
                        using var stringReader = new StringReader(content);
                        using var xmlReader = XmlReader.Create(stringReader);

                        var mathsRequest = (MathsRequest)serializer.Deserialize(xmlReader);
                        operation = mathsRequest?.Operation;

                        if (operation == null || operation.Values.Count == 0)
                        {
                            _logger.LogInformation("Standard XML deserialization failed, trying custom parsing");
                            operation = ParseOperationFromXml(content);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Standard XML deserialization failed, trying custom parsing");
                        operation = ParseOperationFromXml(content);
                    }
                }
                else
                {
                    return BadRequest(new { Error = "Unsupported content type. Use application/json or application/xml" });
                }

                if (operation == null)
                {
                    return BadRequest(new { Error = "Operation is required" });
                }
                var result = _calculatorService.Calculate(operation);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing request");
                return BadRequest(new { Error = ex.Message });
            }
        }

        private Operation ParseOperationFromJson(JsonElement operationElement)
        {
            var operation = new Operation();

            //parse operation type
            if (operationElement.TryGetProperty("@ID", out var idElement))
            {
                operation.OperationType = idElement.GetString();
            }

            //parse values
            if (operationElement.TryGetProperty("Value", out var valuesElement))
            {
                if (valuesElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var valueElement in valuesElement.EnumerateArray())
                    {
                        if (valueElement.ValueKind == JsonValueKind.String && decimal.TryParse(valueElement.GetString(), out var decimalValue))
                        {
                            operation.Values.Add(decimalValue);
                        }
                        else if (valueElement.ValueKind == JsonValueKind.Number)
                        {
                            operation.Values.Add(valueElement.GetDecimal());
                        }
                    }
                }
            }

            //parse nested operations
            if (operationElement.TryGetProperty("Operation", out var nestedOperationElement))
            {
                operation.NestedOperation = ParseOperationFromJson(nestedOperationElement);
            }

            return operation;
        }

        private Operation ParseOperationFromXml(string xmlContent)
        {
            try
            {
                var operation = new Operation();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                // Find the first Operation element
                var operationNode = xmlDoc.SelectSingleNode("//Operation");
                if (operationNode == null) return null;

                // Get the operation type from the ID attribute
                if (operationNode.Attributes?["ID"] != null)
                {
                    operation.OperationType = operationNode.Attributes["ID"].Value;
                }

                // Get all Value elements
                var valueNodes = operationNode.SelectNodes("Value");
                if (valueNodes != null)
                {
                    foreach (XmlNode valueNode in valueNodes)
                    {
                        if (decimal.TryParse(valueNode.InnerText.Replace('\"', ' '), out var value))
                        {
                            operation.Values.Add(value);
                        }
                    }
                }

                // Check for nested operations
                var nestedOperationNode = operationNode.SelectSingleNode("Operation");
                if (nestedOperationNode != null)
                {
                    // Create a new XML document for the nested operation
                    var nestedXml = new XmlDocument();
                    nestedXml.LoadXml(nestedOperationNode.OuterXml);
                    operation.NestedOperation = ParseOperationFromXml(nestedXml.OuterXml);
                }

                return operation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Custom XML parsing failed");
                return null;
            }
        }
    }
}
