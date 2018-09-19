using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Horde
{
    [DataContract]
    public struct Device
    {
        [DataMember(Name = "deviceId")]
        public string ID;

        [DataMember(Name = "collectionId")]
        public string CollectionID;

        [DataMember(Name = "imei")]
        public string IMEI;

        [DataMember(Name = "imsi")]
        public string IMSI;

        [DataMember(Name = "tags")]
        public Dictionary<string, string> Tags;
    }

    [DataContract]
    struct DeviceList
    {
        [DataMember(Name = "devices")]
        public Device[] Devices;
    }
}
