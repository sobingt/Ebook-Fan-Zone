﻿using System;
using EbookZone.Domain.Base;

namespace EbookZone.Domain.Enums
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public UserType UserType { get; set; }

        public RegisterType RegisterType { get; set; }

        public string GoogleId { get; set; }

        public string FacebookId { get; set; }

        public string TwitterId { get; set; }
    }
}