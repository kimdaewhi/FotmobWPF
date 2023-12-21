using System.Diagnostics;
using Newtonsoft.Json;

namespace DmManager
{
    public class ConnectionMain
    {
        private static HttpClient _client = new HttpClient();


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


        public static async void GetPlayerDetail(string playerID)
        {
            HttpResponseMessage response = await _client.GetAsync($"http://13.124.254.65:8080/api/Players/details/{playerID}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var player = JsonConvert.DeserializeObject<Player>(jsonString);

                // 반환된 player 객체를 사용합니다.
                Debug.WriteLine($"Player Name: {player?.Name}, Role: {player?.Role}");
            }
            else
            {
                Debug.WriteLine($"Failed to fetch player. StatusCode: {response.StatusCode}");
            }
        }

    }
}