using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NBIoT
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

        [DataMember(Name = "name")]
        public string Name;
        
        [DataMember(Name = "email")]
        public string Email;
        
        [DataMember(Name = "phone")]
        public string Phone;
        
        [DataMember(Name = "verifiedEmail")]
        public bool VerifiedEmail;
        
        [DataMember(Name = "verifiedPhone")]
        public bool VerifiedPhone;
        
        [DataMember(Name = "connectId")]
        public string ConnectID;
        
        [DataMember(Name = "gitHubLogin")]
        public string GitHubLogin;
        
        [DataMember(Name = "authType")]
        public string AuthType;
        
        [DataMember(Name = "avatarUrl")]
        public string AvatarURL;
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

        public Task<Member> UpdateTeamMemberRole(string teamID, string userID, string role)
        {
            Member m = new Member();
            m.Role = role;
            return update($"/teams/{teamID}/members/{userID}", m);
        }

        public Task DeleteTeamMember(string teamID, string userID)
        {
            return delete($"/teams/{teamID}/members/{userID}");
        }

        public Task DeleteTeam(string id)
        {
            return delete("/teams/" + id);
        }
    }
}
