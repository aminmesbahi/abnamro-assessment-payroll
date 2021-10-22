namespace Assessment.ApplicationCore.Entities
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; protected set; }
        public virtual DateTime ModifiedDate { get; protected set; }
    }
}
