using System;
namespace EMRReport.DataContracts.Entities
{
    public abstract class BaseEntity
    {
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual UserEntity userEntityCreated { get; set; }
        public virtual UserEntity userEntityModified { get; set; }
    }
}
