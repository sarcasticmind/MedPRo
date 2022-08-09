using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace keef2.Models
{
    public class Message
    {
        public int id { get; set; }
        [StringLength(250)]
        public string text { get; set; }
        public DateTime date { get; set; }
        [ForeignKey("User")]
        public string user_id { get; set; }
        public virtual AccountUser User { get; set; }

    }
}
