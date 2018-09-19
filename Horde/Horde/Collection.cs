using System.Collections.Generic;
using System.Runtime.Serialization;

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
}
