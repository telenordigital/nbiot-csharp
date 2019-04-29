using System;
using System.Net.Http;
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

    [DataContract]
    public struct DownstreamMessage
    {
		public DownstreamMessage(int port, byte[] payload) {
			Port = port;
			Payload = payload;
		}

        [DataMember(Name = "port")]
        public int Port;

        [DataMember(Name = "payload")]
        public byte[] Payload;
    }

    [DataContract]
    public struct BroadcastResult
    {
        [DataMember(Name = "sent")]
        public int Sent;

        [DataMember(Name = "failed")]
        public int Failed;

        [DataMember(Name = "errors")]
        public BroadcastError Errors;
    }

    [DataContract]
    public struct BroadcastError
    {
        [DataMember(Name = "deviceID")]
        public string DeviceID;

        [DataMember(Name = "message")]
        public string Message;
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

		public Task Send(string collectionID, string deviceID, DownstreamMessage msg) {
			return request<DownstreamMessage, int>(HttpMethod.Post, $"/collections/{collectionID}/devices/{deviceID}/to", msg);
		}

		public Task<BroadcastResult> Broadcast(string collectionID, DownstreamMessage msg) {
			return request<DownstreamMessage, BroadcastResult>(HttpMethod.Post, $"/collections/{collectionID}/to", msg);
		}
    }
}
