using EmployeeTagManagerApp.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeTagManagerApp.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetTagsAsync();

        Task<Tag> GetTagByIdAsync(int id);

        Task CreateTagAsync(Tag tag);

        Task UpdateTagAsync(Tag tag);

        Task DeleteTagAsync(int id);
    }
}