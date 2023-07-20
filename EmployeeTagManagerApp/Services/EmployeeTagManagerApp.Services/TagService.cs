using EmployeeTagManagerApp.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeTagManagerApp.Data;
using System;
using Microsoft.EntityFrameworkCore;
using EmployeeTagManagerApp.Data.Models;

namespace EmployeeTagManagerApp.Services
{
    namespace EmployeeTagManagerApp.Services
    {
        public class TagService : ITagService
        {
            private readonly ManagerDbContext _dbContext;

            public TagService(ManagerDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IEnumerable<Tag>> GetTagsAsync()
            {
                return await _dbContext.Tags.ToListAsync();
            }

            public async Task<Tag> GetTagByIdAsync(int id)
            {
                return await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
            }

            public async Task CreateTagAsync(Tag tag)
            {
                _dbContext.Tags.Add(tag);
                await _dbContext.SaveChangesAsync();
            }

            public async Task UpdateTagAsync(Tag tag)
            {
                _dbContext.Tags.Update(tag);
                await _dbContext.SaveChangesAsync();
            }

            public async Task DeleteTagAsync(int id)
            {
                var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
                if (tag != null)
                {
                    _dbContext.Tags.Remove(tag);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
