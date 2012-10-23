using AutoMapper;
using EbookZone.Domain;
using EbookZone.Repository.Base;
using EbookZone.Utils.Helpers;
using EbookZone.Web.Models;

namespace EbookZone.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IEntityRepository<User> _entityRepository;

        public IdentityService(IEntityRepository<User> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public bool Register(IdentityViewModel viewModel)
        {
            User model = Mapper.Map<IdentityViewModel, User>(viewModel);

            if (string.IsNullOrEmpty(model.Password))
            {
                string password = EncryptionHelper.GenerateToken(7);
                password = EncryptionHelper.Encrypt(password, model.NetworkId);
                model.Password = password;
            }

            _entityRepository.Create(model);

            return false;
        }

        public bool Login(IdentityViewModel viewModel)
        {
            return false;
        }
    }
}