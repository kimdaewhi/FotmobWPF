using Main.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stack<FrameworkElement> pageStack = new Stack<FrameworkElement>();

        public MainWindow()
        {
            InitializeComponent();

            autoComplete.IDUpdated += AutoComplete_IDUpdated;
            Views.MainPage mainPage = new Views.MainPage();
            // ContentFrame.Content = mainPage;
            ContentFrame.NavigationService.Navigate(mainPage);
        }

        private void Image_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // 홈으로 이동
            while (ContentFrame.NavigationService.CanGoBack)
            {
                ContentFrame.NavigationService.RemoveBackEntry();
            }
            autoComplete.Clear();
            ContentFrame.NavigationService.Navigate(new Views.MainPage());
        }

        // Autocomplete의 ID 업데이트 여부 받는 콜백 함수
        private void AutoComplete_IDUpdated(object sender, string updatedID)
        {
            // 업데이트된 ID 값을 이용한 로직을 처리
            Console.WriteLine($"Updated ID: {updatedID}");
            if (!string.IsNullOrEmpty(updatedID))
            {
                Views.PlayerInfoPage playerInfoPage = new Views.PlayerInfoPage(updatedID);
                ContentFrame.NavigationService.Navigate(playerInfoPage);
            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using var client = new HttpClient();
            var playerId = "654096"; // 원하는 플레이어 ID

            // var response = await client.GetAsync($"https://localhost:7064/api/Players/details/{playerId}");
            var response = await client.GetAsync($"http://13.124.254.65:8080/api/Players/details/{playerId}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var player = JsonConvert.DeserializeObject<Player>(jsonString);

                // 반환된 player 객체를 사용합니다.
                Debug.WriteLine($"Player Name: {player.Name}, Role: {player.Role}");
                MessageBox.Show(player.Name + ", " + player.Position + ", " + player.Age);
            }
            else
            {
                Debug.WriteLine($"Failed to fetch player. StatusCode: {response.StatusCode}");
            }
        }


    }
}
