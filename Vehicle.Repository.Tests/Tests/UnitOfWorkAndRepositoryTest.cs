﻿using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Intefaces;
using Vehicle.Model.Entities;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Repository.Repositories;
using Xunit;

namespace Vehicle.Repository.Tests.Tests
{
    public class UnitOfWorkAndRepositoryTest
    {
        readonly Mock<IDbContext> Context;

        public UnitOfWorkAndRepositoryTest()
        {
            Context = new Mock<IDbContext>();;
        }

        public Task<List<VehicleMake>> GetMakes()
        {
            List<VehicleMake> makes = new List<VehicleMake>();

            for (int i = 1; i <= 10; i++)
            {
                makes.Add(new VehicleMake()
                {
                    Id = i,
                    Name = "Make" + i,
                    Abrv = "M" + i
                });
            }

            return Task.FromResult(makes);
        }

        public Mock<IDbSet<TEntity>> CreateDbSetMock<TEntity>(IEnumerable<TEntity> elements) where TEntity : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<IDbSet<TEntity>>();

            dbSetMock.As<IDbAsyncEnumerable>().Setup(m => m.GetAsyncEnumerator())
                        .Returns(new TestDbAsyncEnumerator<TEntity>(elements.GetEnumerator()));
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.Provider)
                        .Returns(new TestDbAsyncQueryProvider<TEntity>(elementsAsQueryable.Provider));
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.Expression)
                        .Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.ElementType)
                        .Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator())
                        .Returns(() => elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }

        [Fact]
        public async Task VehicleMakeRepository_GetByIdAsync_Should_Get_Make()
        {
            //Arrange
            var makes = await GetMakes();
            Mock<IDbSet<VehicleMake>> makesMock = CreateDbSetMock<VehicleMake>(makes);
            Context.Setup(x => x.Set<VehicleMake>()).Returns(makesMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWork(Context.Object);

            int vehicleId = 2;


            //Act
            var actual = await unitOfWork.MakeRepository.GetMakeByIdAsync(vehicleId);

            //Assert
            actual.Should().NotBeNull();
            actual.Should().Equals(makes.Where(m => m.Id == vehicleId));
        }


        [Fact]
        public async Task VehicleMakeRepository_GetByIdAsync_Should_Return_Null()
        {
            //Arrange
            var makes = await GetMakes();
            Mock<IDbSet<VehicleMake>> makesMock = CreateDbSetMock<VehicleMake>(makes);
            Context.Setup(x => x.Set<VehicleMake>()).Returns(makesMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWork(Context.Object);

            int vehicleId = 12;

            //Act
            var actual = await unitOfWork.MakeRepository.GetMakeByIdAsync(vehicleId);

            //Assert
            actual.Should().BeNull();
        }

       [Fact]
        public async Task VehicleMakeRepository_FindAsync_Should_Return_List()
        {
            //Arrange
            var makes = await GetMakes();
            Mock<IDbSet<VehicleMake>> makesMock = CreateDbSetMock<VehicleMake>(makes);
            Context.Setup(x => x.Set<VehicleMake>()).Returns(makesMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWork(Context.Object);
            QueryStringParameters qSParameters = new QueryStringParameters();

            //Act
            var actual = await unitOfWork.MakeRepository.FindMakesAsync(qSParameters);

            //Assert
            actual.Should().NotBeNull();
            actual.Should().NotBeEmpty();
            actual.Should().BeInAscendingOrder(x => x.Name);
            actual.TotalPages.Should().Be(2);
            actual.CurrentPage.Should().Be(1);
        }

        [Fact]
        public async Task VehicleMakeRepository_InsertAsync_Should_Add_Make()
        {
            //Arrange
            var makes = await GetMakes();
            Mock<IDbSet<VehicleMake>> makesMock = CreateDbSetMock<VehicleMake>(makes);
            Context.Setup(x => x.Set<VehicleMake>()).Returns(makesMock.Object);
            makesMock.Setup(x => x.Add(It.IsAny<VehicleMake>()))
                .Returns((VehicleMake m) => m)
                .Callback((VehicleMake m) => makes.Add(m));
            Context.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            IUnitOfWork unitOfWork = new UnitOfWork(Context.Object);

            var make = new VehicleMake()
            {
                Id = 11,
                Name = "Entity11",
                Abrv = "E11"
            };

            //Act
            await unitOfWork.MakeRepository.InsertAsync(make);
            await unitOfWork.CommitAsync();

            //Assert
            makes.Count.Should().Be(11);
            makes.Last().Should().BeSameAs(make);
            Context.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
       
        [Fact]
        public async Task VehicleMakeRepository_DeleteAsync_Should_Delete_Make()
        {
            var makes = await GetMakes();
            Mock<IDbSet<VehicleMake>> makesMock = CreateDbSetMock<VehicleMake>(makes);
            Context.Setup(x => x.Set<VehicleMake>()).Returns(makesMock.Object);
            makesMock.Setup(x => x.Remove(It.IsAny<VehicleMake>()))
                .Returns((VehicleMake m) => m)
                .Callback((VehicleMake m) =>
                {
                    var makeTD = makes.Find(x => x.Id == m.Id);
                    makes.Remove(makeTD);
                });
            Context.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            IUnitOfWork unitOfWork = new UnitOfWork(Context.Object);

            var makeToDelete = new VehicleMake()
            {
                Id = 3
            };

            await unitOfWork.MakeRepository.DeleteAsync(makeToDelete);
            await unitOfWork.CommitAsync();


            makes.Count().Should().Be(9);
            Context.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
