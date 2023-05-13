namespace Direct4Me.Repository.Entities;

public class UserStatisticsEntity
{
    public int DefaultLoginCount { get; set; } = 0;
    public int FaceLoginCount { get; set; } = 0;
    public DateTime LastModified { get; set; } = DateTime.Now;
}