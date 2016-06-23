using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace GMBoss.Model
{
    [XmlRoot("VoiceFilesResp")]
    public class VoiceFilesResp
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

        [XmlElement("files")]
        public List<VoiceFile> files { get; set; }

        public static VoiceFilesResp fromXML(string xml)
        {
            try
            {
                if (xml == null || xml.Trim().Equals(""))
                    return null;

                using (StringReader rdr = new StringReader(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(VoiceFilesResp));
                    return (VoiceFilesResp)serializer.Deserialize(rdr);
                }
            }
            catch { return null; }
        }

        public class VoiceFile
        {
            [XmlElement("length")]
            public int length { get; set; }

            [XmlElement("size")]
            public int size { get; set; }

            [XmlElement("url")]
            public string url { get; set; }

            [XmlElement("times")]
            public int times { get; set; }

            [XmlElement("content")]
            public string content { get; set; }

            [XmlElement("title")]
            public string title { get; set; }
        }
    }
}