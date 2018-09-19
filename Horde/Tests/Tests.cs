using System;
using Horde;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void TestClient()
        {
            var _ = new Client();
        }

        [Fact]
        public async void TestTeams()
        {
            var client = new Client();

            var teams = await client.GetTeams();
            Assert.Single(teams);

            var team = await client.CreateTeam(new Team());
            teams = await client.GetTeams();
            Assert.Equal(2, teams.Length);
            Assert.Equal(team.ID, Array.Find(teams, t => t.ID == team.ID).ID);

            var tagKey = "test key";
            var tagValue = "test value";
            team.Tags = new System.Collections.Generic.Dictionary<string, string>();
            team.Tags[tagKey] = tagValue;
            team = await client.UpdateTeam(team);
            Assert.NotNull(team.Tags);
            Assert.Equal(tagValue, team.Tags[tagKey]);

            var team2 = await client.GetTeam(team.ID);
            Assert.Equal(team.ID, team2.ID);

            await client.DeleteTeam(team.ID);

            teams = await client.GetTeams();
            Assert.Single(teams);
        }

        [Fact]
        public async void TestCollections()
        {
            var client = new Client();

            var collections = await client.GetCollections();
            Assert.Single(collections);

            var collection = await client.CreateCollection(new Collection());
            collections = await client.GetCollections();
            Assert.Equal(2, collections.Length);
            Assert.Equal(collection.ID, Array.Find(collections, t => t.ID == collection.ID).ID);

            var tagKey = "test key";
            var tagValue = "test value";
            collection.Tags = new System.Collections.Generic.Dictionary<string, string>();
            collection.Tags[tagKey] = tagValue;
            collection = await client.UpdateCollection(collection);
            Assert.NotNull(collection.Tags);
            Assert.Equal(tagValue, collection.Tags[tagKey]);

            var collection2 = await client.GetCollection(collection.ID);
            Assert.Equal(collection.ID, collection2.ID);

            await client.DeleteCollection(collection.ID);

            collections = await client.GetCollections();
            Assert.Single(collections);
        }

        [Fact]
        public async void TestDevices()
        {
            var client = new Client();

            var collection = await client.CreateCollection(new Collection());

            var devices = await client.GetDevices(collection.ID);
            Assert.Empty(devices);

            var device = await client.CreateDevice(collection.ID, new Device { IMEI = "12", IMSI = "34" });
            devices = await client.GetDevices(collection.ID);
            Assert.Single(devices);
            Assert.Equal(device, devices[0]);

            var imei = "56";
            var imsi = "78";
            var tagKey = "test key";
            var tagValue = "test value";
            device.IMEI = imei;
            device.IMSI = imsi;
            device.Tags = new System.Collections.Generic.Dictionary<string, string>();
            device.Tags[tagKey] = tagValue;
            device = await client.UpdateDevice(collection.ID, device);
            Assert.NotNull(device.Tags);
            Assert.Equal(imei, device.IMEI);
            Assert.Equal(imsi, device.IMSI);
            Assert.Equal(tagValue, device.Tags[tagKey]);

            var device2 = await client.GetDevice(collection.ID, device.ID);
            Assert.Equal(device.ID, device2.ID);

            await client.DeleteDevice(collection.ID, device.ID);

            devices = await client.GetDevices(collection.ID);
            Assert.Empty(devices);

            await client.DeleteCollection(collection.ID);
        }
    }
}
