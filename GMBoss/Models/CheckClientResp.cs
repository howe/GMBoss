using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace GMBoss.Model
{
    [XmlRoot("CheckClientResp")]
    public class CheckClientResp
    {
        [XmlElement("ret")]
        public int ret { get; set; }

        [XmlElement("message")]
        public string message { get; set; }

        [XmlElement("forceUpdate")]
        public bool forceUpdate { get; set; }

        [XmlElement("latesVersion")]
        public int latesVersion { get; set; }

        [XmlElement("latesVersionName")]
        public string latesVersionName { get; set; }

        [XmlElement("latesVersionDesc")]
        public string latesVersionDesc { get; set; }

        [XmlElement("latesVersionUrl")]
        public string latesVersionUrl { get; set; }

        [XmlElement("needLogin")]
        public bool needLogin { get; set; }

        [XmlElement("confVolumn")]
        public int confVolumn { get; set; }

        public static CheckClientResp fromXML(string xml)
        {
            try
            {
                if (xml == null || xml.Trim().Equals(""))
                    return null;

                using (StringReader reader = new StringReader(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CheckClientResp));
                    return (CheckClientResp)serializer.Deserialize(reader);
                }
            }
            catch { return null; }
        }
    }
}