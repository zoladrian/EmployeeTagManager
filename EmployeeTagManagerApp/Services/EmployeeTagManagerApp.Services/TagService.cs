using EmployeeTagManagerApp.Data;
using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Events;
using EmployeeTagManagerApp.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTagManagerApp.Services
{
    namespace EmployeeTagManagerApp.Services
    {
        public class TagService : ITagService
        {
            private readonly ManagerDbContext _dbContext;
            private readonly IEventAggregator _eventAggregator;
            private readonly IValidator<Tag> _validator;

            public TagService(ManagerDbContext dbContext, IEventAggregator eventAggregator, IValidator<Tag> validator)
            {
                _dbContext = dbContext;
                _eventAggregator = eventAggregator;
                _validator = validator;
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
                ValidationResult results = _validator.Validate(tag);

                if (!results.IsValid)
                {
                    string errorMessage = string.Join(", ", results.Errors.Select(x => x.ErrorMessage));
                    _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish(errorMessage);
                    return;
                }

                _dbContext.Tags.Add(tag);
                await _dbContext.SaveChangesAsync();
            }

            public async Task UpdateTagAsync(Tag tag)
            {
                ValidationResult results = _validator.Validate(tag);

                if (!results.IsValid)
                {
                    string errorMessage = string.Join(", ", results.Errors.Select(x => x.ErrorMessage));
                    _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish(errorMessage);
                    return;
                }

                var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == tag.Id);

                if (existingTag != null)
                {
                    existingTag.Name = tag.Name;
                    existingTag.Description = tag.Description;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"Tag with ID {tag.Id} does not exist.");
                }
            }

            public async Task DeleteTagAsync(int id)
            {
                var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
                if (tag != null)
                {
                    _dbContext.Tags.Remove(tag);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"Cannot delete. Tag with ID {id} does not exist.");
                }
            }
        }
    }