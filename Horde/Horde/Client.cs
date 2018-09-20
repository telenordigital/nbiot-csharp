using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Horde
{
    public partial class Client
    {
        string addr;
        string token;
        HttpClient client = new HttpClient();


        public Client()
        {
            (addr, token) = Config.addressTokenFromConfig(Config.DefaultFileName);
            ping();
        }

        public Client(string addr, string token)
        {
            this.addr = addr;
            this.token = token;
            ping();
        }

        async void ping()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, addr + "/");
            var resp = await client.SendAsync(req);
            if (resp.IsSuccessStatusCode || resp.StatusCode == HttpStatusCode.Forbidden)
            {
                // A token with restricted access will receive 403 Forbidden from "/"
                // but that still indicates a succesful connection.
                return;
            }
            throw new ClientException(resp.StatusCode, await resp.Content.ReadAsStringAsync());
        }

        Task<T> get<T>(string path)
            where T : struct
        {
            return request<T>(HttpMethod.Get, addr + path, null);
        }

        Task<T> create<T>(string path, T x)
            where T : struct
        {
            return request<T>(HttpMethod.Post, addr + path, x);
        }

        Task<T> update<T>(string path, T x)
            where T : struct
        {
            return request<T>(new HttpMethod("PATCH"), addr + path, x);
        }

        async Task delete(string path)
        {
            await request<int>(HttpMethod.Delete, addr + path, null);
        }

        async Task<T> request<T>(HttpMethod method, string path, T? x)
            where T : struct
        {
            var settings = new DataContractJsonSerializerSettings() { UseSimpleDictionaryFormat = true };
            var serializer = new DataContractJsonSerializer(typeof(T), settings);

            var req = new HttpRequestMessage(method, path);
            if (x != null)
            {
                var stream1 = new MemoryStream();
                serializer.WriteObject(stream1, x);
                stream1.Position = 0;
                req.Content = new StreamContent(stream1);
                req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            var resp = await client.SendAsync(req);
            if (!resp.IsSuccessStatusCode)
            {
                throw new ClientException(resp.StatusCode, await resp.Content.ReadAsStringAsync());
            }
            if (method == HttpMethod.Delete)
            {
                return new T();
            }
            var stream = await resp.Content.ReadAsStreamAsync();
            return (T)serializer.ReadObject(stream);
        }
    }

    public class ClientException : Exception
    {
        HttpStatusCode Status { get; set; }

        public ClientException(HttpStatusCode status, string message)
            : base(message)
        {
            Status = status;
        }
    }
}
