using System.Collections.Generic;
using EbookZone.Core;
using EbookZone.Web.Models.Base;

namespace EbookZone.Services.Interfaces
{
    public interface IBaseService<T> where T : BaseViewModel
    {
        void Execute(T viewModel, EntityAction action);

        List<T> Load();
    }
}