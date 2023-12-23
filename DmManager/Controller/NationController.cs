using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmManager.Controller
{
    public class NationController
    {
        private static HttpClient _client = new HttpClient();

        private static List<Nation> _nations;       // 중앙 저장소
        private static bool _playersLoaded = false; // 중앙 저장소 로드 여부

        public List<Nation> GetNations()
        {
            return _nations;
        }

        // Singleton Instance
        private static NationController _instance;
        public static NationController Instance => _instance ??= new NationController();

        private NationController() { }



        public static async Task<Nation> GetNationDetail(string nationID)
        {
            HttpResponseMessage response = await _client.GetAsync($"http://13.124.254.65:8080/api/Nations/details/{nationID}");
            string jsonString = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();

                string serverUrl = "http://13.124.254.65:8080/";
                string nationJson = AddServerUrlToJsonObject(jsonString, serverUrl);
                Nation? nation = JsonConvert.DeserializeObject<Nation>(nationJson);

                return nation;
            }
            else
            {
                Debug.WriteLine($"Failed to fetch nation. StatusCode: {response.StatusCode}");
                return null;
            }
        }


        private static string AddServerUrlToJsonObject(string jsonStr, string serverUrl)
        {
            // JSON 문자열을 JArray로 변환
            // JArray jArray = JsonConvert.DeserializeObject<JArray>(jsonStr);
            JObject nation = JsonConvert.DeserializeObject<JObject>(jsonStr);
            if (nation["imgUri"] != null)
            {
                nation["imgUri"] = serverUrl + nation["imgUri"].ToString();
            }

            // 가공된 JObject를 다시 JSON 문자열로 변환하여 반환
            return nation.ToString();
        }

    }
}
