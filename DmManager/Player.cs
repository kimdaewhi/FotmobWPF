using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmManager
{
    // DTO
    public class Player
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public int Height { get; set; }
        public int Age { get; set; }
        public DateTime Birth { get; set; }
        public string? Position { get; set; }
        public string? ClubID { get; set; }
        public int ShirtNum { get; set; }
        public string? Foot { get; set; }
        public int MarketValue { get; set; }
        public string? ImgUri { get; set; }
        public string? NationID { get; set; }
    }
}
