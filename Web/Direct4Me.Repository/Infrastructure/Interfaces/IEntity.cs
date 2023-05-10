namespace Direct4Me.Repository.Infrastructure.Interfaces;

public interface IEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}