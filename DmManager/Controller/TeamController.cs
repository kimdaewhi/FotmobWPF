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
    public class TeamController
    {
        private static HttpClient _client = new HttpClient();

        private static List<Team> _teams;       // 중앙 저장소
        private static bool _playersLoaded = false; // 중앙 저장소 로드 여부

        public List<Team> GetTeams()
        {
            return _teams;
        }

        // Singleton Instance
        private static TeamController _instance;
        public static TeamController Instance => _instance ??= new TeamController();



        private TeamController() { }



        public static async Task<Team> GetTeamDetail(string teamID)
        {
            HttpResponseMessage response = await _client.GetAsync($"http://13.124.254.65:8080/api/Teams/details/{teamID}");
            string jsonString = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();

                string serverUrl = "http://13.124.254.65:8080/";
                string teamJson = AddServerUrlToJsonObject(jsonString, serverUrl);
                Team? team = JsonConvert.DeserializeObject<Team>(teamJson);

                return team;
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
            JObject team = JsonConvert.DeserializeObject<JObject>(jsonStr);
            if (team["imgUri"] != null)
            {
                team["imgUri"] = serverUrl + team["imgUri"].ToString();
            }

            // 가공된 JObject를 다시 JSON 문자열로 변환하여 반환
            return team.ToString();
        }

    }
}
