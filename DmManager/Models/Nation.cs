using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmManager
{
    public class Nation
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? NameKO { get; set; }
        public int? Rank { get; set; }
        public string? ImgUri { get; set; }
    }
}
