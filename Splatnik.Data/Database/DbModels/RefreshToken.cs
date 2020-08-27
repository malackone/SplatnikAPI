using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Splatnik.Data.Database.DbModels
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}
