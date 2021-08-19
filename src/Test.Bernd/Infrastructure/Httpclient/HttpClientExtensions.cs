using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Eurocontrol.Vmp.Api.Infrastructure.HttpClient;
using Test.Bernd.App.Config;

namespace Test.Bernd.Infrastructure.Httpclient
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseWrapper<TResponse, TResponse>> PostAsync<TResponse>(
            this System.Net.Http.HttpClient client,
            string requestUri, Dictionary<string, string> data, CancellationToken cancellationToken = default)
        {
            var body = new FormUrlEncodedContent(data);
            return client.ExecuteRequestAsync<TResponse, TResponse, TResponse>(requestUri, HttpMethod.Post, body,
                cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsync<TRequest>(this System.Net.Http.HttpClient client,
            string requestUri, TRequest item, CancellationToken cancellationToken = default)
        {
            var body = GetHttpContent(item);
            return client.ExecuteRequestAsync(requestUri, HttpMethod.Post, body, cancellationToken);
        }

        public static Task<HttpResponseWrapper<TResponse, TResponse>> PostAsync<TRequest, TResponse>(
            this System.Net.Http.HttpClient client,
            string requestUri, TRequest item,
            CancellationToken cancellationToken = default)
        {
            var body = GetHttpContent(item);
            return client.ExecuteRequestAsync<TRequest, TResponse, TResponse>(requestUri, HttpMethod.Post, body,
                cancellationToken);
        }

        public static Task<HttpResponseWrapper<TResponse, TError>> PostAsync<TRequest, TResponse, TError>(
            this System.Net.Http.HttpClient client,
            string requestUri, TRequest item,
            CancellationToken cancellationToken = default)
        {
            var body = GetHttpContent(item);
            return client.ExecuteRequestAsync<TRequest, TResponse, TError>(requestUri, HttpMethod.Post, body,
                cancellationToken);
        }

        public static Task<HttpResponseMessage> PutAsync<TRequest>(this System.Net.Http.HttpClient client,
            string requestUri, TRequest item, CancellationToken cancellationToken = default)
        {
            var body = GetHttpContent(item);
            return client.ExecuteRequestAsync(requestUri, HttpMethod.Put, body, cancellationToken);
        }

        public static Task<HttpResponseWrapper<TResponse, TError>> PutAsync<TResponse, TError>(
            this System.Net.Http.HttpClient client, string requestUri, CancellationToken cancellationToken = default)
        {
            return client.ExecuteRequestAsync<TResponse, TError>(requestUri, HttpMethod.Put, cancellationToken);
        }

        public static Task<HttpResponseWrapper<TResponse, TError>> PutAsync<TRequest, TResponse, TError>(
            this System.Net.Http.HttpClient client, string requestUri, TRequest item,
            CancellationToken cancellationToken = default)
        {
            var body = GetHttpContent(item);
            return client.ExecuteRequestAsync<TRequest, TResponse, TError>(requestUri, HttpMethod.Put, body,
                cancellationToken);
        }

        public static Task<HttpResponseWrapper<TResponse, TResponse>> GetAsync<TResponse>(
            this System.Net.Http.HttpClient client,
            string requestUri, CancellationToken cancellationToken = default)
        {
            return client.ExecuteRequestAsync<TResponse, TResponse>(requestUri, HttpMethod.Get, cancellationToken);
        }

        public static Task<HttpResponseWrapper<TResponse, TError>> GetAsync<TResponse, TError>(
            this System.Net.Http.HttpClient client,
            string requestUri, CancellationToken cancellationToken = default)
        {
            return client.ExecuteRequestAsync<TResponse, TError>(requestUri, HttpMethod.Get, cancellationToken);
        }

        public static Task<HttpResponseWrapper<TResponse, TError>> DeleteAsync<TResponse, TError>(
            this System.Net.Http.HttpClient client, string requestUri, CancellationToken cancellationToken)
        {
            return client.ExecuteRequestAsync<TResponse, TError>(requestUri, HttpMethod.Delete, cancellationToken);
        }

        private static async Task<HttpResponseWrapper<TResponse, TError>> ExecuteRequestAsync<TRequest, TResponse,
            TError>(
            this System.Net.Http.HttpClient client,
            string uri,
            HttpMethod httpMethod,
            HttpContent body,
            CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(httpMethod, uri)
            {
                Content = body
            };
            using var response = await client.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var parsedResult = default(TResponse);
            var parsedError = default(TError);

            if (response.IsSuccessStatusCode)
            {
                parsedResult = await DeserializeJsonFromStream<TResponse>(stream);
            }
            else
            {
                parsedError = await DeserializeJsonFromStream<TError>(stream, false);
            }

            return new HttpResponseWrapper<TResponse, TError>(response, parsedResult, parsedError);
        }

        private static async Task<HttpResponseMessage> ExecuteRequestAsync(
            this HttpMessageInvoker client,
            string uri,
            HttpMethod httpMethod,
            HttpContent body,
            CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(httpMethod, uri) {Content = body};
            var response = await client.SendAsync(request, cancellationToken);
            return response;
        }

        private static async Task<HttpResponseWrapper<TResponse, TError>> ExecuteRequestAsync<TResponse, TError>(
            this System.Net.Http.HttpClient client,
            string uri,
            HttpMethod httpMethod,
            CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(httpMethod, uri);
            using var response = await client.SendAsync(
                request, 
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var parsedResult = default(TResponse);
            var parsedError = default(TError);

            if (response.IsSuccessStatusCode)
            {
                parsedResult = await DeserializeJsonFromStream<TResponse>(stream);
            }
            else
            {
                parsedError = await DeserializeJsonFromStream<TError>(stream, false);
            }

            return new HttpResponseWrapper<TResponse, TError>(response, parsedResult, parsedError);
        }


        private static async Task<T?> DeserializeJsonFromStream<T>(Stream? stream, bool throwOnError = true)
        {
            if (stream == null || stream.CanRead == false)
                return default;

            try
            {
                using var sr = new StreamReader(stream);
                var searchResult = await JsonSerializer.DeserializeAsync<T>(stream, JsonConfig.GetJsonSettings());
                return searchResult;
            }
            catch (Exception)
            {
                if (!throwOnError)
                {
                    return default;
                }

                throw;
            }
        }

        private static HttpContent GetHttpContent<T>(T item)
        {
            var serialized = JsonSerializer.Serialize(item, JsonConfig.GetJsonSettings());
            
            HttpContent content = new StringContent(
                serialized, 
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            return content;
        }
    }
}