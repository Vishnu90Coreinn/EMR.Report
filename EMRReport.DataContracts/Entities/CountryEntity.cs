using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class CountryEntity : BaseEntity
    {
        public int CountryId { get; set; }
        public string Country { get; set; }
        public Guid CountryGuid { get; set; }
        public virtual ICollection<AddressEntity> addressEntityList { get; set; }
        public virtual ICollection<StateEntity> stateEntityList { get; set; }
    }
}
