using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Horde
{
    [DataContract]
    public struct Collection
    {
        [DataMember(Name = "collectionId")]
        public string ID;

        [DataMember(Name = "teamId")]
        public string TeamID;

        [DataMember(Name = "tags")]
        public Dictionary<string, string> Tags;
    }

    [DataContract]
    struct CollectionList
    {
        [DataMember(Name = "collections")]
        public Collection[] Collections;
    }

    public partial class Client
    {
        public Task<Collection> GetCollection(string id)
        {
            return get<Collection>("/collections/" + id);
        }

        public async Task<Collection[]> GetCollections()
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

        public Task DeleteCollection(string id)
        {
            return delete("/collections/" + id);
        }
    }
}
