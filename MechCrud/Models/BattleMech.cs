using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MechCrud.Models
{
    public class BattleMech
    {
        [XmlElement("Id")]
        public int Id { get; set; }
        [XmlElement("MechModel")]
        public string MechModel { get; set; }
        [XmlElement("MechName")]
        public string MechName{ get; set; }
        [XmlElement("Price")]
        [RegularExpression(@"[\d]")]
        public int Price { get; set; }
        [XmlElement("LA")]
        public string LA { get; set; }
        [XmlElement("RA")]
        public string RA { get; set; }
        [XmlElement("LT")]
        public string LT { get; set; }
        [XmlElement("CT")]
        public string CT { get; set; }
        [XmlElement("RT")]
        public string RT { get; set; }
        [XmlElement("Head")]
        public string Head { get; set; }
        [XmlElement("Armor")]
        [RegularExpression(@"[\d]")]
        public float Armor { get; set; }
        [XmlElement("Heatsinks")]
        [RegularExpression(@"[\d]")]
        public int Heatsinks { get; set; }       
        [Range(20,100, ErrorMessage ="20 to 100 tonnes only!")]
        [XmlElement("Tonnage")]
        public int Tonnage { get; set; }
    }
}