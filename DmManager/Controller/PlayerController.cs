using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DmManager.Controller
{
    public class PlayerController
    {
        private static HttpClient _client = new HttpClient();

        private static List<Player> _players;       // 중앙 저장소
        private static bool _playersLoaded = false; // 중앙 저장소 로드 여부

        public List<Player> GetPlayers()
        {
            return _players;
        }

        // Singleton Instance
        private static PlayerController _instance;
        public static PlayerController Instance => _instance ??= new PlayerController();



        private PlayerController() { }


        /// <summary>
        /// 선수 List 조회
        /// </summary>
        /// <returns></returns>
        public static async Task<string?> GetPlayerList()
        {
            HttpResponseMessage response = await _client.GetAsync("http://13.124.254.65:8080/api/Players/list/");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 선수 세부 정보 조회
        /// </summary>
        /// <param name="playerID">선수 ID</param>
        /// <returns>선수 정보 인스턴스 반환</returns>
        public static async Task<Player> GetPlayerDetail(string playerID)
        {
            // 이 메서드가 구지 필요한지는 나중에 생각좀 해보자
            HttpResponseMessage response = await _client.GetAsync($"http://13.124.254.65:8080/api/Players/details/{playerID}");
            string jsonString = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
                Player? player = JsonConvert.DeserializeObject<Player>(jsonString);

                // 반환된 player 객체를 사용
                // Debug.WriteLine($"Player Name: {player?.Name}, Role: {player?.Role}");

                return player;
            }
            else
            {
                Debug.WriteLine($"Failed to fetch player. StatusCode: {response.StatusCode}");
                return null;
            }
        }


        /// <summary>
        /// 플레이어 중앙 저장소(_players) 데이터 로드 여부
        /// </summary>
        /// <returns></returns>
        public async Task LoadPlayersIfNeeded()
        {
            if (!_playersLoaded)
            {
                string playersJson = await GetPlayerList();
                if (playersJson != null)
                {
                    _players = JsonConvert.DeserializeObject<List<Player>>(playersJson);

                    // Image Uri 변경
                    string serverUrl = "http://13.124.254.65:8080/";
                    playersJson = AddServerUrlToJson(playersJson, serverUrl);

                    _players = JsonConvert.DeserializeObject<List<Player>>(playersJson);
                    _playersLoaded = true;
                }
                else
                {
                    Debug.WriteLine("Failed to fetch players.");
                }
            }
        }


        /// <summary>
        /// 선수 이미지에 서버 정보 바인딩
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="serverUrl"></param>
        /// <returns></returns>
        private string AddServerUrlToJson(string jsonStr, string serverUrl)
        {
            // JSON 문자열을 JArray로 변환
            JArray jArray = JsonConvert.DeserializeObject<JArray>(jsonStr);

            // 각 플레이어의 ImgUri에 서버 URL 추가
            foreach (JObject player in jArray)
            {
                if (player["imgUri"] != null)
                {
                    player["imgUri"] = serverUrl + player["imgUri"].ToString();
                }
            }

            // 가공된 JObject를 다시 JSON 문자열로 변환하여 반환
            return jArray.ToString();
        }


        /// <summary>
        /// store에서 선수 세부정보 조회
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Player GetPlayerById(string playerId)
        {
            return _players.FirstOrDefault(player => player.Id == playerId);
        }


    }
}