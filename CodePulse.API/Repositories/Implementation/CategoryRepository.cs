using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using CodePulse.API.Data;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : IRepository<Category>

    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return _dbContext.Categories.ToList();
        }
        public async Task<Category?> GetAsyncById(Guid id)
        {
            return _dbContext.Categories.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Category> UpdateAsync(Category entity)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(entity.Id);
            if(existingCategory != null)
            {
                _dbContext.Entry(existingCategory).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
                return existingCategory;
            }
            return null!;

        }
        public async Task<Category> DeleteAsync(Guid id)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return null;
            }
            _dbContext.Categories.Remove(existingCategory);

            await _dbContext.SaveChangesAsync();

            return existingCategory;
        }

    }
}
