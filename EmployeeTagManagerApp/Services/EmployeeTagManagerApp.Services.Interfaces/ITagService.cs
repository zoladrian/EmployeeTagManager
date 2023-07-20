using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeTagManagerApp.Data.Models;

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
