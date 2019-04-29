using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NBIoT
{
    public interface IOutput { }

    public struct WebHookOutput : IOutput
    {
        public string ID;
        public string CollectionID;
        public string URL;
        public string BasicAuthUser;
        public string BasicAuthPass;
        public string CustomHeaderName;
        public string CustomHeaderValue;
    }

    public struct MQTTOutput : IOutput
    {
        public string ID;
        public string CollectionID;
        public string Endpoint;
        public bool DisableCertCheck;
        public string Username;
        public string Password;
        public string ClientID;
        public string TopicName;
    }

    public struct IFTTTOutput : IOutput
    {
        public string ID;
        public string CollectionID;
        public string Key;
        public string EventName;
        public bool AsIsPayload;
    }

    public partial class Client
    {
        public async Task<IOutput> Output(string collectionID, string outputID)
        {
            return (await get<RawOutput>($"/collections/{collectionID}/outputs/{outputID}")).toOutput();
        }

        public async Task<IOutput[]> Outputs(string collectionID)
        {
            return (await get<OutputList>($"/collections/{collectionID}/outputs")).Outputs.Select(o => o.toOutput()).ToArray();
        }

        public async Task<IOutput> CreateOutput(string collectionID, IOutput output)
        {
            var o = new RawOutput(output);
            return (await create($"/collections/{collectionID}/outputs", o)).toOutput();
        }

        public async Task<IOutput> UpdateOutput(string collectionID, IOutput output)
        {
            var o = new RawOutput(output);
            return (await update($"/collections/{collectionID}/outputs/{o.ID}", o)).toOutput();
        }

        public Task DeleteOutputTag(string collectionID, string outputID, string key)
        {
            return delete($"/collections/{collectionID}/outputs/{outputID}/tags/{key}");
        }

        public Task DeleteOutput(string collectionID, string outputID)
        {
            return delete($"/collections/{collectionID}/outputs/{outputID}");
        }
    }

    [DataContract]
    struct RawOutput
    {
        [DataMember(Name = "outputId")]
        public string ID;

        [DataMember(Name = "collectionId")]
        public string CollectionID;

        [DataMember(Name = "type")]
        public string Type;

        [DataMember(Name = "config")]
        public Dictionary<string, object> Config;

        internal RawOutput(IOutput output)
        {
            {
                if (output is WebHookOutput o)
                {
                    ID = o.ID;
                    CollectionID = o.CollectionID;
                    Type = "webhook";
                    Config = new Dictionary<string, object>()
                    {
                        ["url"] = o.URL,
                        ["basicAuthUser"] = o.BasicAuthUser,
                        ["basicAuthPass"] = o.BasicAuthPass,
                        ["customHeaderName"] = o.CustomHeaderName,
                        ["customHeaderValue"] = o.CustomHeaderValue,
                    };
                    return;
                }
            }
            {
                if (output is MQTTOutput o)
                {
                    ID = o.ID;
                    CollectionID = o.CollectionID;
                    Type = "mqtt";
                    Config = new Dictionary<string, object>()
                    {
                        ["endpoint"] = o.Endpoint,
                        ["disableCertCheck"] = o.DisableCertCheck,
                        ["username"] = o.Username,
                        ["password"] = o.Password,
                        ["clientId"] = o.ClientID,
                        ["topicName"] = o.TopicName,
                    };
                    return;
                }
            }
            {
                if (output is IFTTTOutput o)
                {
                    ID = o.ID;
                    CollectionID = o.CollectionID;
                    Type = "ifttt";
                    Config = new Dictionary<string, object>()
                    {
                        ["key"] = o.Key,
                        ["eventName"] = o.EventName,
                        ["asIsPayload"] = o.AsIsPayload,
                    };
                    return;
                }
            }
            throw new System.Exception("unknown output type");
        }

        internal IOutput toOutput()
        {
            switch (Type)
            {
                case "webhook":
                    return new WebHookOutput
                    {
                        ID = ID,
                        CollectionID = CollectionID,
                        URL = str("url"),
                        BasicAuthUser = str("basicAuthUser"),
                        BasicAuthPass = str("basicAuthPass"),
                        CustomHeaderName = str("customHeaderName"),
                        CustomHeaderValue = str("customHeaderValue"),
                    };
                case "mqtt":
                    return new MQTTOutput
                    {
                        ID = ID,
                        CollectionID = CollectionID,
                        Endpoint = str("endpoint"),
                        DisableCertCheck = boolean("disableCertCheck"),
                        Username = str("username"),
                        Password = str("password"),
                        ClientID = str("clientId"),
                        TopicName = str("topicName"),
                    };
                case "ifttt":
                    return new IFTTTOutput
                    {
                        ID = ID,
                        CollectionID = CollectionID,
                        Key = str("key"),
                        EventName = str("eventName"),
                        AsIsPayload = boolean("asIsPayload"),
                    };
            }
            return null;
        }

        string str(string key)
        {
            if (Config.ContainsKey(key) && Config[key] is string s)
            {
                return s;
            }
            return "";
        }

        bool boolean(string key)
        {
            if (Config.ContainsKey(key) && Config[key] is bool b)
            {
                return b;
            }
            return false;
        }
    }

    [DataContract]
    struct OutputList
    {
        [DataMember(Name = "outputs")]
        public RawOutput[] Outputs;
    }
}
