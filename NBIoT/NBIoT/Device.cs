using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NBIoT
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

    public partial class Client
    {
        public Task<Device> GetDevice(string collectionID, string deviceID)
        {
            return get<Device>($"/collections/{collectionID}/devices/{deviceID}");
        }

        public async Task<Device[]> GetDevices(string collectionID)
        {
            return (await get<DeviceList>($"/collections/{collectionID}/devices")).Devices;
        }

        public Task<Device> CreateDevice(string collectionID, Device device)
        {
            return create($"/collections/{collectionID}/devices", device);
        }

        public Task<Device> UpdateDevice(string collectionID, Device device)
        {
            return update($"/collections/{collectionID}/devices/{device.ID}", device);
        }

        public Task DeleteDevice(string collectionID, string deviceID)
        {
            return delete($"/collections/{collectionID}/devices/{deviceID}");
        }
    }
}
