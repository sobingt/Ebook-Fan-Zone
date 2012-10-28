using System.ComponentModel.DataAnnotations;
using EbookZone.Domain.Enums;

namespace EbookZone.ViewModels
{
    public class IdentityViewModel
    {
        public string ParentNetworkId { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public AccountType AccountType { get; set; }

        public UserType UserType { get; set; }

        // Network Ids
        public string GoogleId { get; set; }

        public string FacebookId { get; set; }

        public string BoxCloudId { get; set; }
    }
}