﻿namespace DAL.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }   
        public string TokenId { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatingDate { get; set; }  
        public DateTime ExpirationDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
