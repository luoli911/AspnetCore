using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    //public enum CellName
    //{
    //    lcell1,lcell2,lcell3, lcell4, lcell5, lcell6,lcell7,lcell8,lcell9,lcell10,lcell11,lcell12,
    //    wcell1, wcell2, wcell3, wcell4, wcell6, wcell7, wcell8, wcell9
    //}
    public class Cell
    {  
        [Key]
        public int CellId { get; set; }
        public string CellName { get; set; }
        public string Machine { get; set; }
        public string OS { get; set; }
        public string ARM { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ServerName { get; set; }
        public string ClientName { get; set; }     
        public virtual ICollection<Scenario> Scenarios { get; set; }
    }
}