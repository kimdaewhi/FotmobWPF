using DmManager;
using DmManager.Controller;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.Views
{
    /// <summary>
    /// PlayerInfoPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayerInfoPage : Page
    {
        private string _playerID;

        public partial class PlayerInfoViewModel
        {
            public Player? SelectedPlayerDetail { get; set; }
            public Nation? SelectedPlayerNation { get; set; }
            public Team? SelectedPlayerTeam { get; set; }
        }
        private PlayerInfoViewModel viewModels;


        public PlayerInfoPage(string playerID)
        {
            InitializeComponent();
            viewModels = new PlayerInfoViewModel();


            this._playerID = playerID;
            InitPlayerInfoAsync();
            
        }

        /// <summary>
        /// 플레이어 정보 Init(선수정보, 국적, 팀 정보)
        /// </summary>
        private async void InitPlayerInfoAsync()
        {
            viewModels.SelectedPlayerDetail = PlayerController.Instance.GetPlayerById(_playerID);
            // 변경된 포맷으로 데이터 바인딩
            txt_playerMarketValue.Text = "€" + (viewModels.SelectedPlayerDetail.MarketValue / 1000000.0).ToString() + "M";

            viewModels.SelectedPlayerNation = await NationController.GetNationDetail(viewModels.SelectedPlayerDetail.NationID);
            viewModels.SelectedPlayerTeam = await TeamController.GetTeamDetail(viewModels.SelectedPlayerDetail.ClubID);

            this.DataContext = viewModels;
        }


    }
}
