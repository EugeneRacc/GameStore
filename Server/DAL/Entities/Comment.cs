using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Comment : BaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string GameId { get; set; }
        public Guid? ReplieId { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; }
        public Game Game { get; set; }
    }
}
