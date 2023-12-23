using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmManager
{
    public class Team
    {
        public string? ID { get; set; }
        public string? TeamName { get; set; }
        public string? Country { get; set; }
        public string? LeagueID { get; set; }
        public string? StadiumID { get; set; }
        public string? ColorCode { get; set; }
        public string? ImgUri { get; set; }
    }
}
