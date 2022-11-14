using macrix_api.Models;

public interface IPeopleDbContext 
{
    Task<IEnumerable<PersonEntity>> GetAllAsync();
    Task<PersonEntity?> GetOneAsync(long id);
    Task ModifyOneAsync(PersonEntity entity);
    Task AddOneAsync(PersonEntity entity);
    Task<bool> DeleteOneAsync(long id);
    Task<IEnumerable<PersonEntity>> BatchInsertUpdateAsync(IEnumerable<PersonEntity> entities);
    bool EntityExists(long id);
}