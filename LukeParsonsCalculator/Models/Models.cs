using System.Xml.Serialization;
using System.Text.Json.Serialization;

namespace LukeParsonsCalculator.Models
{
    [XmlRoot("Maths")]
    public class MathsRequest
    {
        [XmlElement("Operation")]
        [JsonPropertyName("Operation")]
        public Operation Operation { get; set; }
    }
    
    public class Operation
    {
        [XmlAttribute("ID")]
        [JsonPropertyName(@"ID")]
        public string OperationType { get; set; }
        [XmlElement("value")]
        [JsonPropertyName("Value")]
        public List<decimal> Values { get; set; } = new List<decimal>();

        [XmlElement("Operation")]
        [JsonPropertyName("Operation")]
        public Operation NestedOperation { get; set; }

    }


    public class CalculationResult
    {
        public decimal Result { get; set; }
    }
}

