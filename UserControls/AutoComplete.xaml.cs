using DmManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UserControls
{
    /// <summary>
    /// AutoComplete.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AutoComplete : UserControl
    {
        public event EventHandler<string> IDUpdated;
        private string _id;
        public string ID 
        { 
            get { return _id; } 
            set 
            { 
                _id = value;
                // setter에 id변경
                OnIDUpdated(_id);
            }
        }

        //private List<PlayerOrClubItem> suggestionList = new List<PlayerOrClubItem>
        //{
        //    new PlayerOrClubItem("282775", "Resources\\players\\282775.png", "Jorginho" ),
        //    new PlayerOrClubItem("434325", "Resources\\players\\434325.png", "Thomas Partey" ),
        //    new PlayerOrClubItem("576165", "Resources\\players\\576165.png", "Gabriel Jesus" ),
        //    new PlayerOrClubItem("961995", "Resources\\players\\961995.png", "Bukayo Saka" ),
        //    new PlayerOrClubItem("1021586", "Resources\\players\\1021586.png", "Gabriel Martinelli" ),
        //};

        private List<DmManager.Player> suggestionList = new List<DmManager.Player>();


        public AutoComplete()
        {
            InitializeComponent();

            LoadPlayers();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtInput.Focus();
        }


        private void autoComplete_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = txtInput.Text.ToLower();
            
            // List<PlayerOrClubItem> filteredList = suggestionList.Where(item => item.Name.ToLower().Contains(inputText)).ToList();
            List<DmManager.Player> filteredList = suggestionList.Where(item => item.Name.ToLower().Contains(inputText)).ToList();

            listSuggestions.ItemsSource = filteredList;

            if (inputText != string.Empty)
            {
                listSuggestions.Visibility = filteredList.Any() ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                // textInput 비어있을 때는 list 닫아줌
                listSuggestions.Visibility = Visibility.Collapsed;
            }

        }

        protected virtual void OnIDUpdated(string id)
        {
            // id값 변화에 따른 callback 메서드
            IDUpdated?.Invoke(this, id);
        }

        private void txtInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (listSuggestions.Visibility == Visibility.Visible)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        // 위쪽 화살표
                        if (listSuggestions.SelectedIndex > 0)
                        {
                            listSuggestions.SelectedIndex--;
                        }
                        break;
                    case Key.Down:
                        // 아래쪽 화살표
                        if (listSuggestions.SelectedIndex < listSuggestions.Items.Count - 1)
                        {
                            listSuggestions.SelectedIndex++;
                        }
                        break;
                    case Key.Enter:
                        // 엔터
                        if (listSuggestions.SelectedIndex != -1)
                        {
                            // 선택된 값을 TextInput에 입력
                             var selectedItem = (DmManager.Player)listSuggestions.SelectedItem;
                            txtInput.Text = selectedItem.Name;

                            // 커서 위치를 선택된 값의 끝으로 이동
                            txtInput.CaretIndex = selectedItem.Name.Length;
                            listSuggestions.Visibility = Visibility.Collapsed;

                            // ID = selectedItem.ID;
                            OnIDUpdated(selectedItem.Id);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void txtInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtInput.Focus();
        }

        private void listSuggestions_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (listSuggestions.SelectedIndex != -1)
            {
                var selectedItem = (PlayerOrClubItem)listSuggestions.SelectedItem;
                txtInput.Text = selectedItem.Name;

                // 커서 위치를 선택된 값의 끝으로 이동
                txtInput.CaretIndex = selectedItem.Name.Length;
                listSuggestions.Visibility = Visibility.Collapsed;

                // ID = selectedItem.ID;
                OnIDUpdated(selectedItem.ID);
            }
        }

        public void Clear()
        {
            this.txtInput.Clear();
            this.ID = string.Empty;
        }


        private async void LoadPlayers()
        {
            string? jsonStr = await DmManager.ConnectionMain.GetPlayerList();
            suggestionList = JsonConvert.DeserializeObject<List<DmManager.Player>>(jsonStr);
        }

        
    }
}
