using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

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

    public partial class Client
    {
        public Task<Team> GetTeam(string id)
        {
            return get<Team>("/teams/" + id);
        }

        public async Task<Team[]> GetTeams()
        {
            return (await get<TeamList>("/teams")).Teams;
        }

        public Task<Team> CreateTeam(Team team)
        {
            return create("/teams", team);
        }

        public Task<Team> UpdateTeam(Team team)
        {
            return update("/teams/" + team.ID, team);
        }

        public Task DeleteTeam(string id)
        {
            return delete("/teams/" + id);
        }
    }
}
