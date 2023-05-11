using Direct4Me.Repository.Infrastructure.Interfaces;

namespace Direct4Me.Repository.Entities;

public class UserStatisticsEntity : IEntity
{
    public Guid UserFk { get; set; }
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}