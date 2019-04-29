using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NBIoT
{
    [DataContract]
    public struct Collection
    {
        [DataMember(Name = "collectionId")]
        public string ID;

        [DataMember(Name = "teamId")]
        public string TeamID;

        [DataMember(Name = "fieldMask")]
        public FieldMask FieldMask;

        [DataMember(Name = "tags")]
        public Dictionary<string, string> Tags;
    }

    [DataContract]
    public struct FieldMask
    {
        [DataMember(Name = "imsi")]
        public bool? IMSI;

        [DataMember(Name = "imei")]
        public bool? IMEI;

        [DataMember(Name = "location")]
        public bool? Location;

        [DataMember(Name = "msisdn")]
        public bool? MSISDN;
    }

    [DataContract]
    struct CollectionList
    {
        [DataMember(Name = "collections")]
        public Collection[] Collections;
    }

    public partial class Client
    {
        public Task<Collection> Collection(string id)
        {
            return get<Collection>("/collections/" + id);
        }

        public async Task<Collection[]> Collections()
        {
            return (await get<CollectionList>("/collections")).Collections;
        }

        public Task<Collection> CreateCollection(Collection collection)
        {
            return create("/collections", collection);
        }

        public Task<Collection> UpdateCollection(Collection collection)
        {
            return update("/collections/" + collection.ID, collection);
        }

        public Task DeleteCollectionTag(string collectionID, string key)
        {
            return delete($"/collections/{collectionID}/tags/{key}");
        }

        public Task DeleteCollection(string id)
        {
            return delete("/collections/" + id);
        }
    }
}
