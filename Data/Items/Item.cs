using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGameList.Data
{
    public class Item
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string Notes { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public int Flags { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ViewCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser Author { get; set; }
        
        public virtual List<Comment> Comments { get; set; }
    }
}