using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace GMBoss.Model
{
    [XmlRoot("LoginResp")]
    public class LoginResp
    {
        [XmlElement("ret")]
        public int ret { get; set; }

        [XmlElement("message")]
        public string message { get; set; }

        [XmlElement("userId")]
        public string userId { get; set; }

        [XmlElement("shopId")]
        public int shopId { get; set; }

        [XmlElement("shopName")]
        public string shopName { get; set; }

        [XmlElement("token")]
        public string token { get; set; }

        public static LoginResp fromXML(string xml)
        {
            try
            {
                if (xml == null || xml.Trim().Equals(""))
                    return null;

                using (StringReader rdr = new StringReader(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(LoginResp));
                    return (LoginResp)serializer.Deserialize(rdr);
                }
            }
            catch { return null; }
        }
    }
}