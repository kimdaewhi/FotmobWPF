using DmManager;
using DmManager.Controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
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


        /// <summary>
        /// 자동 완성 기능
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoComplete_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = txtInput.Text.ToLower();

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


        /// <summary>
        /// ID 업데이트 Callback Method
        /// </summary>
        /// <param name="id"></param>
        protected virtual void OnIDUpdated(string id)
        {
            // id값 변화에 따른 callback 메서드
            IDUpdated?.Invoke(this, id);
        }


        /// <summary>
        /// 필터 ListBox 키보드 이동 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// 필터 ListBox 마우스 클릭 이벤트(selectedID 업데이트)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listSuggestions_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (listSuggestions.SelectedIndex != -1)
            {
                var selectedItem = (Player)listSuggestions.SelectedItem;
                txtInput.Text = selectedItem.Name;

                // 커서 위치를 선택된 값의 끝으로 이동
                txtInput.CaretIndex = selectedItem.Name.Length;
                listSuggestions.Visibility = Visibility.Collapsed;

                // ID = selectedItem.ID;
                OnIDUpdated(selectedItem.Id);
            }
        }

        /// <summary>
        /// Autocomplete Clear
        /// </summary>
        public void Clear()
        {
            this.txtInput.Clear();
            this.ID = string.Empty;
        }


        /// <summary>
        /// 선수 리스트 조회
        /// </summary>
        private async void LoadPlayers()
        {
            await PlayerController.Instance.LoadPlayersIfNeeded();
            suggestionList = PlayerController.Instance.GetPlayers();
        }




    }
}
