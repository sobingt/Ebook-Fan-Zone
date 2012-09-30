using System.Collections.Generic;

namespace EbookZone.Web.Models.Base
{
    public class BaseListViewModel<T> where T : BaseViewModel
    {
        public List<T> Items { get; set; }
    }
}