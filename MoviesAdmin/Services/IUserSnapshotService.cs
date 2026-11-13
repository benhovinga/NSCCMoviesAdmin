namespace MoviesAdmin.Services
{
    public interface IUserSnapshotService
    {
        Task AddUserAsync(string UserId, string email, string name);
    }
}
