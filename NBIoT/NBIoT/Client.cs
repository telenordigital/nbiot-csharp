using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace NBIoT
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
            req.Headers.Add("X-API-Token", token);
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
            return request<T>(HttpMethod.Get, path, null);
        }

        Task<T> create<T>(string path, T x)
            where T : struct
        {
            return request<T>(HttpMethod.Post, path, x);
        }

        Task<T> update<T>(string path, T x)
            where T : struct
        {
            return request<T>(new HttpMethod("PATCH"), path, x);
        }

        async Task delete(string path)
        {
            await request<int>(HttpMethod.Delete, path, null);
        }

        async Task<T> request<T>(HttpMethod method, string path, T? x)
            where T : struct
        {
            return await request<T, T>(method, path, x);
        }

        async Task<U> request<T, U>(HttpMethod method, string path, T? x)
            where T : struct
            where U : struct
        {
            var settings = new DataContractJsonSerializerSettings() { UseSimpleDictionaryFormat = true };

            var req = new HttpRequestMessage(method, addr + path);
            req.Headers.Add("X-API-Token", token);
            if (x != null)
            {
                var serializer1 = new DataContractJsonSerializer(typeof(T), settings);
                var stream1 = new MemoryStream();
                serializer1.WriteObject(stream1, x);
                stream1.Position = 0;
                req.Content = new StreamContent(stream1);
                req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            var resp = await client.SendAsync(req);
            if (!resp.IsSuccessStatusCode)
            {
                throw new ClientException(resp.StatusCode, await resp.Content.ReadAsStringAsync());
            }
            if (typeof(U) == typeof(int))
            {
                return new U();
            }
            var serializer = new DataContractJsonSerializer(typeof(U), settings);
            var stream = await resp.Content.ReadAsStreamAsync();
            return (U)serializer.ReadObject(stream);
        }
    }

    public class ClientException : Exception
    {
        public HttpStatusCode Status { get; set; }

        public ClientException(HttpStatusCode status, string message)
            : base(message)
        {
            Status = status;
        }
    }
}
