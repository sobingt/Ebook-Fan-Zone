﻿using EbookZone.Domain.Enums;

namespace EbookZone.Web.Models
{
    public class IdentityViewModel
    {
        public string NetworkId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public AccountType AccountType { get; set; }
    }
}