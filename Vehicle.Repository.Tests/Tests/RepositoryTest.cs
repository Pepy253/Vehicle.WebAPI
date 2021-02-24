using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Contexts;
using Vehicle.DAL.Intefaces;
using Vehicle.Model.Common.Models;
using Vehicle.Model.Models;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Repository.Repositories;
using Xunit;

namespace Vehicle.Repository.Tests.Tests
{
    public class RepositoryTest
    {
        readonly Mock<IDbSet<BaseEntity>> _mockDbSet;
        readonly Mock<IDbContext> _mockContext;

        public RepositoryTest()
        {
            _mockDbSet = new Mock<IDbSet<BaseEntity>>();
            _mockContext = new Mock<IDbContext>();
        }

        public IQueryable<BaseEntity> GetEntities()
        {
            List<BaseEntity> entities = new List<BaseEntity>();

            for (int i = 1; i <= 5; i++)
            {
                entities.Add(new BaseEntity()
                {
                    Id = i,
                    Name = "Entity" + i,
                    Abrv = "E" + i
                });
            }

            return entities.AsQueryable();
        }

        public IRepository<BaseEntity> GetSetup()
        {
            var entities = GetEntities();

            _mockDbSet.As<IDbAsyncEnumerable>().Setup(m => m.GetAsyncEnumerator())
                        .Returns(new TestDbAsyncEnumerator<BaseEntity>(entities.GetEnumerator()));
            _mockDbSet.As<IQueryable<BaseEntity>>().Setup(m => m.Provider)
                        .Returns(new TestDbAsyncQueryProvider<BaseEntity>(entities.Provider));
            _mockDbSet.As<IQueryable<BaseEntity>>().Setup(m => m.Expression)
                        .Returns(entities.Expression);
            _mockDbSet.As<IQueryable<BaseEntity>>().Setup(m => m.ElementType)
                        .Returns(entities.ElementType);
            _mockDbSet.As<IQueryable<BaseEntity>>().Setup(m => m.GetEnumerator())
                        .Returns(entities.GetEnumerator());
      
            _mockContext.Setup(m => m.Set<BaseEntity>())
                        .Returns(_mockDbSet.Object);

            IRepository<BaseEntity> repo = new Repository<BaseEntity>(_mockContext.Object);

            return repo;
        }

        [Fact]
        public async Task MakeRepository_Get_By_Id_Async_Should_Get_Make()
        {
            //Arrange
            var entities = GetEntities();
            var _repo = GetSetup();
            
            //Act
            var actual = await _repo.GetByIdAsync(2);

            //Assert
            actual.Should().NotBeNull();
            actual.Should().Equals(entities.Where(m => m.Id == 2));
        }
    }
}
