using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class StateEntity : BaseEntity
    {

        public int StateId { get; set; }
        public string State { get; set; }
        public int CountryId { get; set; }
        public virtual CountryEntity countryEntity { get; set; }
        public virtual ICollection<AddressEntity> addressEntityList { get; set; }
    }
}
