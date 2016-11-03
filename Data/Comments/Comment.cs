using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGameList.Data
{
    public class Comment
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public int Flags { get; set; }
        [Required]
        public string UserId { get; set; }
        /// <summary>
        /// pointing to the master comment this entry is replying to, or null if the comment is not a reply.
        /// </summary>
        public int? ParentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser Author { get; set; }

        [ForeignKey("ParentId")]
        public virtual Comment Parent { get; set; }

        public virtual List<Comment> Children { get; set; }
    }
}