using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NBIoT
{
    [DataContract]
    public struct OutputDataMessage
    {
        [DataMember(Name = "device")]
        public Device Device;

        [DataMember(Name = "payload")]
        public byte[] Payload;

        [DataMember(Name = "received")]
        public long Received;
    }

    [DataContract]
    struct OutputDataMessageList
    {
        [DataMember(Name = "messages")]
        public OutputDataMessage[] Messages;
    }

    public partial class Client
    {
        public async Task<OutputDataMessage[]> Data(string collectionID, DateTime? since, DateTime? until, int limit)
        {
            return (await get<OutputDataMessageList>($"/collections/{collectionID}/data?since={unixms(since)}&until={unixms(until)}&limit={limit}")).Messages;
        }

        public async Task<OutputDataMessage[]> Data(string collectionID, string deviceID, DateTime? since, DateTime? until, int limit)
        {
            return (await get<OutputDataMessageList>($"/collections/{collectionID}/devices/{deviceID}/data?since={unixms(since)}&until={unixms(until)}&limit={limit}")).Messages;
        }

		static long unixms(DateTime? t) {
			if (t == null) {
				return 0;
			}
			return ((DateTimeOffset)t.Value.ToUniversalTime()).ToUnixTimeMilliseconds();
		}
    }
}
