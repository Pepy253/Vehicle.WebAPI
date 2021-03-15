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
using Vehicle.Model.Entities;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Service.Tests
{
    public class MakeServiceTest
    {

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

        public Task<IUnitOfWork> CreateUnitOfWork(List<VehicleMake> makes)
        {           
            var makesAsQueryable = makes.AsQueryable();
            var dbSetMock = new Mock<IDbSet<VehicleMake>>();

            dbSetMock.As<IDbAsyncEnumerable>().Setup(m => m.GetAsyncEnumerator())
                        .Returns(new TestDbAsyncEnumerator<VehicleMake>(makes.GetEnumerator()));
            dbSetMock.As<IQueryable<VehicleMake>>().Setup(m => m.Provider)
                        .Returns(new TestDbAsyncQueryProvider<VehicleMake>(makesAsQueryable.Provider));
            dbSetMock.As<IQueryable<VehicleMake>>().Setup(m => m.Expression)
                        .Returns(makesAsQueryable.Expression);
            dbSetMock.As<IQueryable<VehicleMake>>().Setup(m => m.ElementType)
                        .Returns(makesAsQueryable.ElementType);
            dbSetMock.As<IQueryable<VehicleMake>>().Setup(m => m.GetEnumerator())
                        .Returns(() => makesAsQueryable.GetEnumerator());
            dbSetMock.Setup(x => x.Add(It.IsAny<VehicleMake>()))
                .Returns((VehicleMake m) => m)
                .Callback((VehicleMake m) => makes.Add(m));
            dbSetMock.Setup(x => x.Attach(It.IsAny<VehicleMake>()))
                .Returns((VehicleMake m) => m)
                .Callback((VehicleMake m) =>
                {
                    var makeToUpdate = makes.Find(x => x.Id == m.Id);
                    makeToUpdate.Name = m.Name;
                    makeToUpdate.Abrv = m.Abrv;
                });
            dbSetMock.Setup(x => x.Remove(It.IsAny<VehicleMake>()))
               .Returns((VehicleMake m) => m)
               .Callback((VehicleMake m) =>
               {
                   var makeTD = makes.Find(x => x.Id == m.Id);
                   makes.Remove(makeTD);
               });

            Mock<IDbContext> contextMock = new Mock<IDbContext>();
            contextMock.Setup(x => x.Set<VehicleMake>()).Returns(dbSetMock.Object);            
            IUnitOfWork unitOfWork = new UnitOfWork(contextMock.Object);

            return Task.FromResult(unitOfWork);
        }

        public Task<IMapper> CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            IMapper mapper = new Mapper(config);

            return Task.FromResult(mapper);
        }

        [Fact]
        public async Task VehicleMakeService_FindAsync_Should_Return_StaticPagedList_IVehicleMake()
        {
            var makes = await GetMakes();
            IMakeService makeService = new MakeService(await CreateUnitOfWork(makes), await CreateMapper());
            QueryStringParameters qSParameters = new QueryStringParameters();

            var actual = await makeService.FindMakesAsync(qSParameters);

            actual.Should().BeOfType(typeof(PagedList<IVehicleMakeDTO>));
        }

        [Fact]
        public async Task VehicleMakeService_GetByIdAsync_Should_Return_IVehicleMake_Object()
        {
            var makes = await GetMakes();
            var mapper = await CreateMapper();
            IMakeService makeService = new MakeService(await CreateUnitOfWork(makes), mapper);
            var make = new VehicleMake()
            {
                Id = 3
            };
            var makeToFind = mapper.Map<IVehicleMakeDTO>(make);

            var actual = await makeService.GetMakeAsync(makeToFind);

            actual.Should().NotBeNull();
            actual.Should().Equals(makeToFind);
        }
        

        [Fact]
        public async Task VehicleMakeService_InsertMakeAsync_Should_Add_VehicleMake_Object_From_IVehicleMake_Object()
        {
            var makes = await GetMakes();                       
            var mapper = await CreateMapper();
            IMakeService makeService = new MakeService(await CreateUnitOfWork(makes), mapper);
            var newMake = new VehicleMake()
            {
                Id = 11,
                Name = "Make11",
                Abrv = "M11"
            };
            var makeToInsert = mapper.Map<IVehicleMakeDTO>(newMake);

            var actual = await makeService.InsertMakeAsync(makeToInsert);

            actual.Should().Be(1);
            makes.Last().Id.Should().Be(newMake.Id);
            makes.Count.Should().Be(11);
        }

        [Fact]
        public async Task VehicleMakeService_DeleteMakeAsync_Should_Delete_VehicleMake_Object_From_IVehicleMake_Object()
        {
            var makes = await GetMakes();
            var mapper = await CreateMapper();
            IMakeService makeService = new MakeService(await CreateUnitOfWork(makes), mapper);
            var make = new VehicleMake()
            {
                Id = 3,
                Name = "Make3",
                Abrv = "M3"
            };
            var makeToDelete = mapper.Map<IVehicleMakeDTO>(make);

            var actual = await makeService.DeleteMakeAsync(makeToDelete);

            actual.Should().Be(1);
            makes.Count().Should().Be(9);
        }
    }    
}
