﻿using System.ComponentModel.DataAnnotations.Schema;
using EbookZone.Domain.Base;
using EbookZone.Domain.Enums;

namespace EbookZone.Domain
{
    [Table("Users")]
    public class User : BaseEntity
    {
        public string ParentNetworkId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public AccountType AccountType { get; set; }

        public UserType UserType { get; set; }

        public string GoogleId { get; set; }

        public string FacebookId { get; set; }

        public string BoxCloudId { get; set; }
    }
}