﻿using EmployeeTagManagerApp.Data;
using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Services;
using EmployeeTagManagerApp.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using EmployeeTagManagerApp.Services.EmployeeTagManagerApp.Services;

namespace EmployeeTagManagerApp.Tests
{
    public class TagServiceTests
    {
        private ITagService _tagService;
        private ManagerDbContext _dbContext;
        private IValidator<Tag> _validator;
        private IEventAggregator _eventAggregator;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ManagerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ManagerDbContext(options);
            _eventAggregator = Substitute.For<IEventAggregator>();
            _validator = Substitute.For<IValidator<Tag>>();
            _tagService = new TagService(_dbContext, _eventAggregator, _validator);
        }

        [Test]
        public async Task GetTagByIdAsync_TagExists_ReturnsTag()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Tag1", Description = "Description1" };
            _dbContext.Tags.Add(tag);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _tagService.GetTagByIdAsync(tag.Id);

            // Assert
            Assert.AreEqual(tag.Id, result.Id);

            // Cleanup
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task CreateTagAsync_TagDoesNotExist_CreatesTag()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Tag1", Description = "Description1" };
            _validator.Validate(tag).Returns(new ValidationResult()); // Assuming the validation passes.

            // Act
            await _tagService.CreateTagAsync(tag);

            // Assert
            Assert.AreEqual(1, _dbContext.Tags.Count());

            // Cleanup
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task UpdateTagAsync_TagExists_UpdatesTag()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Tag1", Description = "Description1" };
            _dbContext.Tags.Add(tag);
            await _dbContext.SaveChangesAsync();

            tag.Name = "UpdatedTag";
            tag.Description = "UpdatedDescription";
            _validator.Validate(tag).Returns(new ValidationResult()); // Assuming the validation passes.

            // Act
            await _tagService.UpdateTagAsync(tag);

            // Assert
            var updatedTag = _dbContext.Tags.FirstOrDefault(t => t.Id == tag.Id);
            Assert.AreEqual("UpdatedTag", updatedTag.Name);
            Assert.AreEqual("UpdatedDescription", updatedTag.Description);

            // Cleanup
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task DeleteTagAsync_TagExists_DeletesTag()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Tag1", Description = "Description1" };
            _dbContext.Tags.Add(tag);
            await _dbContext.SaveChangesAsync();

            // Act
            await _tagService.DeleteTagAsync(tag.Id);

            // Assert
            Assert.AreEqual(0, _dbContext.Tags.Count());
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
