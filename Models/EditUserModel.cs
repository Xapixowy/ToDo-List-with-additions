﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ToDo_List_with_additions.Models
{
    public class EditUserModel
    {
        [BsonElement("login")]
        [Required]
        public string? Login { get; set; }
        [BsonElement("password")]
        [MinLength(6)]
        public string? Password { get; set; }
        [BsonElement("firstName")]
        [Required]
        public string? FirstName { get; set; }
        [BsonElement("lastName")]
        [Required]
        public string? LastName { get; set; }
        [BsonElement("nickname")]
        [Required]
        public string? Nickname { get; set; }
    }
}
