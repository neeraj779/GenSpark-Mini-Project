namespace StudentManagementAPI.Interfaces
{
    public interface IUserRepository<K, T> : IRepository<K, T> where T : class
    {
        public Task<T> GetByUserName(string userName);
    }
}
