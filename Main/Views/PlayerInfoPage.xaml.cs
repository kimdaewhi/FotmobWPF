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

            string mainPosition = viewModels.SelectedPlayerDetail.Position.Split(',')[0].Trim();
            RenderPlayerPosition(mainPosition);

            this.DataContext = viewModels;
        }



        // 선수 포지션 필드 렌더
        private void RenderPlayerPosition(string mainPosition)
        {
            Dictionary<string, Border> borderControls = new Dictionary<string, Border>
            {
                // Att
                { "ST", txtPos_ST },
                { "LW", txtPos_LW },
                { "AM", txtPos_AM },
                { "RW", txtPos_RW },

                // Mid
                { "LM", txtPos_LM },
                { "CM", txtPos_CM },
                { "RM", txtPos_RM },
                { "DM", txtPos_DM },

                // Def
                { "LWB", txtPos_LWB },
                { "RWB", txtPos_RWB },
                { "LB", txtPos_LB },
                { "RB", txtPos_RB },
                { "CB", txtPos_CB },

                // GK
                { "GK", txtPos_GK },
            };

            Dictionary<string, string> positionKor = new Dictionary<string, string>
            {
                // Att
                { "ST", "스트라이커" },
                { "LW", "왼쪽 윙어"},
                { "AM", "중앙 공격형 미드필더" },
                { "RW", "오른쪽 윙어" },

                // Mid
                { "LM", "왼쪽 미드필더" },
                { "CM", "중앙 미드필더" },
                { "RM", "오른쪽 미드필더" },
                { "DM", "중앙 수비형 미드필더" },

                // Def
                { "LWB", "왼쪽 윙백" },
                { "RWB", "오른쪽 윙백" },
                { "LB", "왼쪽 수비수" },
                { "RB", "오른쪽 수비수" },
                { "CB", "중앙 수비수" },

                // GK
                { "GK", "골키퍼" },
            };

            // Main 포지션 렌더
            borderControls[mainPosition].Visibility = Visibility.Visible;
            Color color = (Color)ColorConverter.ConvertFromString(viewModels.SelectedPlayerTeam.ColorCode);
            borderControls[mainPosition].Background = new SolidColorBrush(color);

            txt_basePosition.Text = positionKor[mainPosition];



            // Sub 포지션 렌더
            if (viewModels.SelectedPlayerDetail.Position.Split(',').Length > 1)
            {
                txt_otherPosionLabel.Visibility = Visibility.Visible;
                for (int i = 1; i < viewModels.SelectedPlayerDetail.Position.Split(',').Length; i++)
                {
                    string currSubPos = (viewModels.SelectedPlayerDetail.Position.Split(',')[i].Trim());
                    borderControls[currSubPos].Visibility = Visibility.Visible;

                    // Sub 포지션 첫 번째면 투명도 없이, 후순위 Sub 포지션이면 투명도 설정
                    if(i <= (viewModels.SelectedPlayerDetail.Position.Split(',').Length / 2))
                    {
                        borderControls[currSubPos].Background = new SolidColorBrush(Color.FromRgb(160, 160, 160));
                    }
                    else
                    {
                        borderControls[currSubPos].Background = new SolidColorBrush(Color.FromArgb(90, 160, 160, 160));
                    }
                    txt_otherPosition.Text += positionKor[viewModels.SelectedPlayerDetail.Position.Split(',')[i].Trim()] + ", ";
                }
                txt_otherPosition.Text = txt_otherPosition.Text.Substring(0, txt_otherPosition.Text.Length - 2);
            }
            else
            {
                txt_otherPosionLabel.Visibility = Visibility.Hidden;
            }
            


        }


    }
}
