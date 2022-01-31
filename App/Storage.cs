using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App
{
    [XmlRoot]
    public class StorageResponse
    {
        [XmlElement]
        public string Ticket { get; set; }
        [XmlElement]
        public string Status { get; set; }
        [XmlArray("Errors"), XmlArrayItem("Error")]

        public List<string> Errors { get; set; }

        public string? originalRequestString { get; set; }
    }

    [XmlRoot]
    public class StorageRequest
    {
        [XmlElement]
        public string Ticket { get; set; }
        [XmlElement]
        public string DocumentType { get; set; }
        [XmlElement]
        public string Binary { get; set; }
    }
}
