using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GenricFrame.AppCode.Helper
{
    public class APIRequest
    {
        public static APIRequest O { get { return Instance.Value; } }
        private static Lazy<APIRequest> Instance = new Lazy<APIRequest>(() => new APIRequest());
        private APIRequest() { }

        public async Task<string> GetAsync(string URL, IDictionary<string, string> headers = null)
        {
            HttpWebRequest http = (HttpWebRequest)System.Net.WebRequest.Create(URL);
            http.Timeout = 5 * 60 * 1000;
            string result = string.Empty;
            try
            {
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        http.Headers.Add(item.Key, item.Value);
                    }
                }

                using (var response = await http.GetResponseAsync())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = await sr.ReadToEndAsync();
                    }
                }
            }
            catch (UriFormatException ufx)
            {
                throw new Exception(ufx.Message);
            }
            catch (WebException wx)
            {
                if (wx.Response != null)
                {
                    using (var ErrorResponse = wx.Response)
                    {
                        using (StreamReader sr = new StreamReader(ErrorResponse.GetResponseStream()))
                        {
                            result = await sr.ReadToEndAsync();
                        }
                    }
                }
                else
                {
                    throw new Exception(wx.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public async Task<string> PostAsync_With_QueryString(string URL, IDictionary<string, string> headers = null)
        {
            HttpWebRequest http = (HttpWebRequest)System.Net.WebRequest.Create(URL);
            http.Timeout = 5 * 60 * 1000;
            http.Method = HttpMethod.Post.ToString();
            string result = string.Empty;
            try
            {
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        http.Headers.Add(item.Key, item.Value);
                    }
                }

                using (var response = await http.GetResponseAsync())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = await sr.ReadToEndAsync();
                    }
                }
            }
            catch (UriFormatException ufx)
            {
                throw new Exception(ufx.Message);
            }
            catch (WebException wx)
            {
                if (wx.Response != null)
                {
                    using (var ErrorResponse = wx.Response)
                    {
                        using (StreamReader sr = new StreamReader(ErrorResponse.GetResponseStream()))
                        {
                            result = await sr.ReadToEndAsync();
                        }
                    }
                }
                else
                {
                    throw new Exception(wx.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public async Task<string> PostAsync_With_Urlencoded(string URL, string PostData, string ContentType = "application/x-www-form-urlencoded")
        {
            HttpWebRequest http = (HttpWebRequest)System.Net.WebRequest.Create(URL);
            http.Timeout = 5 * 60 * 1000;
            var data = Encoding.ASCII.GetBytes(PostData);
            http.Method = "POST";
            http.ContentType = ContentType;
            http.ContentLength = data.Length;
            using (Stream stream = await http.GetRequestStreamAsync().ConfigureAwait(false))
            {
                await stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
            }
            string result = "";
            try
            {
                WebResponse response = await http.GetResponseAsync().ConfigureAwait(false);
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            }
            catch (UriFormatException ufx)
            {
                throw new Exception(ufx.Message);
            }
            catch (WebException wx)
            {
                if (wx.Response != null)
                {
                    using (var ErrorResponse = wx.Response)
                    {
                        using (StreamReader sr = new StreamReader(ErrorResponse.GetResponseStream()))
                        {
                            result = await sr.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }
                }
                else
                {
                    throw new Exception(wx.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public async Task<string> PostAsync_With_Json(string URL, object PostData, IDictionary<string, string> headers)
        {
            string result = "";
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest http = (HttpWebRequest)System.Net.WebRequest.Create(URL);
                http.Timeout = 3 * 60 * 1000;
                var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PostData));
                http.Method = "POST";
                http.Accept = ContentType.application_json;
                http.ContentType = ContentType.application_json;
                http.MediaType = ContentType.application_json;
                http.ContentLength = data.Length;
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        http.Headers.Add(item.Key, item.Value);
                    }
                }
                using (Stream stream = http.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                WebResponse response = await http.GetResponseAsync().ConfigureAwait(false);

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            }
            catch (UriFormatException ufx)
            {
                throw new Exception(ufx.Message);
            }
            catch (WebException wx)
            {
                if (wx.Response != null)
                {
                    using (var ErrorResponse = wx.Response)
                    {
                        using (StreamReader sr = new StreamReader(ErrorResponse.GetResponseStream()))
                        {
                            result = await sr.ReadToEndAsync();
                        }
                    }
                }
                else
                {
                    throw new Exception(wx.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}