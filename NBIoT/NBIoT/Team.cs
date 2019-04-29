using System.Net.Http;
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

    [DataContract]
    public struct Invite
    {
        [DataMember(Name = "code")]
        public string Code;

        [DataMember(Name = "createdAt")]
        public long CreatedAt;
    }

    [DataContract]
    struct InviteList
    {
        [DataMember(Name = "invites")]
        public Invite[] Invites;
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

        public Task<Invite> GetInvite(string teamID, string code)
        {
            return get<Invite>($"/teams/{teamID}/invites/{code}");
        }

        public async Task<Invite[]> GetInvites(string teamID)
        {
            return (await get<InviteList>($"/teams/{teamID}/invites")).Invites;
        }

        public Task<Invite> CreateInvite(string teamID)
        {
            return create($"/teams/{teamID}/invites", new Invite());
        }

        public Task<Team> AcceptInvite(string code)
        {
            Invite invite = new Invite();
            invite.Code = code;
            return request<Invite, Team>(HttpMethod.Post, "/teams/accept", invite);
        }

        public Task DeleteInvite(string teamID, string code)
        {
            return delete($"/teams/{teamID}/invites/{code}");
        }
    }
}
