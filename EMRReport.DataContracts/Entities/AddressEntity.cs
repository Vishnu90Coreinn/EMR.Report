using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class AddressEntity
    {
        public int AddressID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string FullAddress { get; set; }
        public string StreetName { get; set; }
        public string CityName { get; set; }
        public int? CountryID { get; set; }
        public int? StateID { get; set; }
        public Guid AddressGuid { get; set; }
        public bool Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual UserEntity userEntityCreated { get; set; }
        public virtual UserEntity userEntityModified { get; set; }
        public virtual CountryEntity countryEntity { get; set; }
        public virtual StateEntity stateEntity { get; set; }
        public virtual ICollection<UserEntity> userEntityList { get; set; }
    }
}
