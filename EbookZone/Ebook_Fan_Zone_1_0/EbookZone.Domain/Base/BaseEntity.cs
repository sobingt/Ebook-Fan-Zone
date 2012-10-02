using System;

namespace EbookZone.Domain.Base
{
    public class BaseEntity
    {
        private DateTime? _createDate;
        private DateTime? _updateDate;

        public int Id { get; set; }

        public DateTime? CreateDate
        {
            get { return this._createDate ?? (this._createDate = DateTime.Now); }
            set { this._createDate = value; }
        }

        public DateTime? UpdateDate
        {
            get { return this._updateDate ?? (this._updateDate = DateTime.Now); }
            set { this._updateDate = value; }
        }
    }
}