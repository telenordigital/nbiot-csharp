using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Horde
{
    [DataContract]
    public struct Team
    {
        [DataMember(Name = "teamId")]
        public string ID;

        [DataMember(Name = "members")]
        public Member[] Members;

        [DataMember(Name = "tags")]
        public Dictionary<string, string> Tags;
    }

    [DataContract]
    struct TeamList
    {
        [DataMember(Name = "teams")]
        public Team[] Teams;
    }

    [DataContract]
    public struct Member
    {
        [DataMember(Name = "userId")]
        public string UserID;

        [DataMember(Name = "role")]
        public string Role;
    }
}
