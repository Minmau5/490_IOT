using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT1.Model
{
    public class PacketStreamModel
    {
        public ObjectId Id { get; set; }

        [BsonElement("iot_id")]
        public int IotId { get; set; }

        [BsonElement("iot_timestamp")]
        public long Timestamp { get; set; }

        public Data Data { get; set; }


        public string GetIotIdString()
        {
            
            switch (IotId)
            {
                case 0:
                    return "Temperature";
                case 1:
                    return "CO2";
                case 2:
                    return "Heart Rate";
                case 3:
                    return "Blood Oxygen Level";
                default:
                    return "GPS";
            }
        }
    }

    public class Data
    {
        public int Uid { get; set; }

        public float Value { get; set; }

        public float Lat { get; set; }

        public float Long { get; set; }

        [BsonElement("ref")]
        private BsonDocument sensor { get; set; }
    }
}
