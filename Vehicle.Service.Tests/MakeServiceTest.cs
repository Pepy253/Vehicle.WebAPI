using AutoMapper;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Intefaces;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;
using Vehicle.Service.Services;
using Xunit;
using Vehicle.WebAPI.AutoMapperConfig;
using Vehicle.Repository.Repositories;
using Vehicle.DAL.Entities;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Service.Tests
{
    public class MakeServiceTest
    {

        public Task<List<VehicleMakeEntity>> GetMakes()
        {
            List<VehicleMakeEntity> makes = new List<VehicleMakeEntity>();

            for (int i = 1; i <= 10; i++)
            {
                makes.Add(new VehicleMakeEntity()
                {
                    Id = i,
                    Name = "Make" + i,
                    Abrv = "M" + i
                });
            }

            return Task.FromResult(makes);
        }

        public Task<IDbContext> CreateMockDBContext(List<VehicleMakeEntity> makes)
        {           
            var makesAsQueryable = makes.AsQueryable();
            var dbSetMock = new Mock<IDbSet<VehicleMakeEntity>>();

            dbSetMock.As<IDbAsyncEnumerable>().Setup(m => m.GetAsyncEnumerator())
                        .Returns(new TestDbAsyncEnumerator<VehicleMakeEntity>(makes.GetEnumerator()));
            dbSetMock.As<IQueryable<VehicleMakeEntity>>().Setup(m => m.Provider)
                        .Returns(new TestDbAsyncQueryProvider<VehicleMakeEntity>(makesAsQueryable.Provider));
            dbSetMock.As<IQueryable<VehicleMakeEntity>>().Setup(m => m.Expression)
                        .Returns(makesAsQueryable.Expression);
            dbSetMock.As<IQueryable<VehicleMakeEntity>>().Setup(m => m.ElementType)
                        .Returns(makesAsQueryable.ElementType);
            dbSetMock.As<IQueryable<VehicleMakeEntity>>().Setup(m => m.GetEnumerator())
                        .Returns(() => makesAsQueryable.GetEnumerator());
            dbSetMock.Setup(x => x.Add(It.IsAny<VehicleMakeEntity>()))
                .Returns((VehicleMakeEntity m) => m)
                .Callback((VehicleMakeEntity m) => makes.Add(m));
            dbSetMock.Setup(x => x.Attach(It.IsAny<VehicleMakeEntity>()))
                .Returns((VehicleMakeEntity m) => m)
                .Callback((VehicleMakeEntity m) =>
                {
                    var makeToUpdate = makes.Find(x => x.Id == m.Id);
                    makeToUpdate.Name = m.Name;
                    makeToUpdate.Abrv = m.Abrv;
                });
            dbSetMock.Setup(x => x.Remove(It.IsAny<VehicleMakeEntity>()))
               .Returns((VehicleMakeEntity m) => m)
               .Callback((VehicleMakeEntity m) =>
               {
                   var makeTD = makes.Find(x => x.Id == m.Id);
                   makes.Remove(makeTD);
               });

            Mock<IDbContext> contextMock = new Mock<IDbContext>();
            contextMock.Setup(x => x.Set<VehicleMakeEntity>()).Returns(dbSetMock.Object);                        

            return Task.FromResult(contextMock.Object);
        }

        public Task<IMapper> CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            IMapper mapper = new Mapper(config);

            return Task.FromResult(mapper);
        }

        [Fact]
        public async Task MakeService_FindMakesAsync_Should_Return_PagedList_IVehicleMake()
        {
            var makes = await GetMakes();
            IUnitOfWork uOW = new UnitOfWork(await CreateMockDBContext(makes));
            IMakeRepository makeRepo = new MakeRepository(await CreateMockDBContext(makes));
            IMakeService makeService = new MakeService(uOW, await CreateMapper(), makeRepo);
            PagingParams pagingParams = new PagingParams();
            SortingParams sortingParams = new SortingParams();
            FilteringParams filteringParams = new FilteringParams();

            var actual = await makeService.FindMakesAsync(pagingParams, sortingParams, filteringParams);

            actual.Should().BeOfType(typeof(PagedList<IVehicleMake>));
            actual.Count().Should().Be(5);
            actual.HasNext.Should().BeTrue();
        }

        [Fact]
        public async Task MakeService_GetMakeAsync_Should_Return_IVehicleMake()
        {
            var makes = await GetMakes();
            var mapper = await CreateMapper();
            IUnitOfWork uOW = new UnitOfWork(await CreateMockDBContext(makes));
            IMakeRepository makeRepo = new MakeRepository(await CreateMockDBContext(makes));
            IMakeService makeService = new MakeService(uOW, mapper, makeRepo);
            var make = new VehicleMakeEntity()
            {
                Id = 3
            };
            var makeToFind = mapper.Map<IVehicleMake>(make);

            var actual = await makeService.GetMakeAsync(makeToFind);

            actual.Should().NotBeNull();
            actual.Should().Equals(makeToFind);
        }
        

        [Fact]
        public async Task MakeService_InsertMakeAsync_Should_Add_VehicleMakeEntity_Object_From_IVehicleMake()
        {
            var makes = await GetMakes();                       
            var mapper = await CreateMapper();
            IUnitOfWork uOW = new UnitOfWork(await CreateMockDBContext(makes));
            IMakeRepository makeRepo = new MakeRepository(await CreateMockDBContext(makes));
            IMakeService makeService = new MakeService(uOW, mapper, makeRepo);
            var newMake = new VehicleMakeEntity()
            {
                Id = 11,
                Name = "Make11",
                Abrv = "M11"
            };
            var makeToInsert = mapper.Map<IVehicleMake>(newMake);

            var actual = await makeService.InsertMakeAsync(makeToInsert);

            actual.Should().Be(1);
            makes.Last().Id.Should().Be(newMake.Id);
            makes.Count.Should().Be(11);
        }

        [Fact]
        public async Task MakeService_DeleteMakeAsync_Should_Delete_VehicleMakeEntity_Object_From_IVehicleMake()
        {
            var makes = await GetMakes();
            var mapper = await CreateMapper();
            IUnitOfWork uOW = new UnitOfWork(await CreateMockDBContext(makes));
            IMakeRepository makeRepo = new MakeRepository(await CreateMockDBContext(makes));
            IMakeService makeService = new MakeService(uOW, mapper, makeRepo);
            var make = new VehicleMakeEntity()
            {
                Id = 3,
                Name = "Make3",
                Abrv = "M3"
            };
            var makeToDelete = mapper.Map<IVehicleMake>(make);

            var actual = await makeService.DeleteMakeAsync(makeToDelete);

            actual.Should().Be(1);
            makes.Count().Should().Be(9);
        }
    }    
}
