using System.Collections.Generic;
using EbookZone.Web.Core;
using EbookZone.Web.Models.Base;

namespace EbookZone.Web.Services
{
    public interface IBaseService<T> where T : BaseViewModel
    {
        List<T> Load();

        void Execute(T viewModel, EntityAction action);
    }
}