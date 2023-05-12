using Direct4Me.Repository.Infrastructure.Interfaces;

namespace Direct4Me.Repository.Entities;

public class PackageStatisticsEntity : IEntity
{
    public string Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public Guid PackageFk { get; set; }
}