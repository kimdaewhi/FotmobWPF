using Main.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Main.ViewModels
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Player> _players;

        public ObservableCollection<Player> Players
        {
            get { return _players; }
            set
            {
                _players = value;
                OnPropertyChanged(nameof(Players));
            }
        }


        // 데이터를 추가하는 메서드
        public void AddPlayer(string id, string name)
        {
            // 임의의 데이터 생성 (예시로 간략하게 작성)
            Player newPlayer = new Player
            {
                Id = id,
                Name = name,
                // 나머지 속성들을 채워넣으세요
            };

            // 만약 Players가 null이면 초기화
            if (Players == null)
            {
                Players = new ObservableCollection<Player>();
            }

            // 데이터 추가
            Players.Add(newPlayer);
        }



        // 데이터를 가져오는 메서드 (예: JSON 파싱 등)
        public void LoadPlayersFromJson(string jsonData)
        {
            // JSON 데이터 파싱하여 Players에 할당
            // 예시로 간략히 작성한 코드입니다.
            Players = JsonConvert.DeserializeObject<ObservableCollection<Player>>(jsonData);
        }

        // INotifyPropertyChanged 구현을 위한 코드
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
