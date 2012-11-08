using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using AutoMapper;
using EbookZone.Domain;
using EbookZone.Domain.Enums;
using EbookZone.Repository.Base;
using EbookZone.Services.Interfaces;
using EbookZone.Utils.Helpers;
using EbookZone.ViewModels;

namespace EbookZone.Services.Implementations
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
            var user = _entityRepository.Load().SingleOrDefault(x => x.Email == viewModel.Email);

            if (user == null)
            {
                user = Mapper.Map<IdentityViewModel, User>(viewModel);
            }
            else
            {
                if (viewModel.AccountType == AccountType.Google)
                    user.GoogleId = viewModel.GoogleId;

                if (viewModel.AccountType == AccountType.Facebook)
                    user.FacebookId = viewModel.FacebookId;

                if (viewModel.AccountType == AccountType.BoxCloud)
                    user.BoxCloudId = viewModel.BoxCloudId;
            }

            string password = user.Password;

            password = EncryptionHelper.Encrypt(user.Email, string.IsNullOrEmpty(password) ? user.Email : user.Password);

            user.Password = password;

            if (!user.Id.HasValue)
            {
                _entityRepository.Create(user);
            }
            else
            {
                _entityRepository.Update(user);
            }

            return Login(viewModel, viewModel.AccountType, true);
        }

        public bool Login(IdentityViewModel viewModel, AccountType accountType, bool afterRegster)
        {
            User user = null;

            if(accountType != AccountType.Default)
            {
                if(accountType == AccountType.BoxCloud)
                {
                    user =
                        _entityRepository.Load().SingleOrDefault(
                            x => x.Email == viewModel.Email && x.BoxCloudId == viewModel.BoxCloudId);
                }

                if(accountType == AccountType.Facebook)
                {
                    user =
                        _entityRepository.Load().SingleOrDefault(
                            x => x.Email == viewModel.Email && x.FacebookId == viewModel.FacebookId);
                }

                if(accountType == AccountType.Google)
                {
                    user =
                        _entityRepository.Load().SingleOrDefault(
                            x => x.Email == viewModel.Email && x.GoogleId == viewModel.GoogleId);
                }
            }
            else
            {
                string password = EncryptionHelper.Decrypt(viewModel.Email, viewModel.Password);
                user = _entityRepository.Load().SingleOrDefault(x => x.Email == viewModel.Email && x.Password == password);
            }

            if (user != null && user.Id.HasValue)
            {
                SaveCookie(user.Id.Value, user.Email);
                return true;
            }

            return false;
        }

        public void LogOff()
        {
            FormsAuthentication.SignOut();
        }

        private void SaveCookie(int userId, string userEmail)
        {
            DateTime dt = DateTime.Now;

            var ticket = new FormsAuthenticationTicket(1, userId.ToString(CultureInfo.InvariantCulture), dt, dt.AddMinutes(30), false, userEmail, FormsAuthentication.FormsCookiePath);

            string hash = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash)
                {
                    HttpOnly = true,
                    Secure = FormsAuthentication.RequireSSL,
                    Path = FormsAuthentication.FormsCookiePath
                };

            if (!string.IsNullOrEmpty(FormsAuthentication.CookieDomain))
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}