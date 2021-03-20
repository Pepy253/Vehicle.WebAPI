using AutoMapper;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Entities;
using Vehicle.DAL.Intefaces;
using Vehicle.Model.Common.Interfaces;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Repository.Repositories;
using Vehicle.Service.Common.Interfaces;
using Vehicle.Service.Services;
using Vehicle.WebAPI.AutoMapperConfig;
using Xunit;

namespace Vehicle.Service.Tests
{
    public class ModelServiceTest
    {

        public Task<List<VehicleModelEntity>> GetModels()
        {
            List<VehicleModelEntity> models = new List<VehicleModelEntity>();

            for (int i = 1; i <= 10; i++)
            {
                models.Add(new VehicleModelEntity()
                {
                    Id = i,
                    Name = "Model" + i,
                    Abrv = "M" + i
                });
            }

            return Task.FromResult(models);
        }

        public Task<IDbContext> CreateMockDBContext(List<VehicleModelEntity> models)
        {
            var modelsAsQueryable = models.AsQueryable();
            var dbSetMock = new Mock<IDbSet<VehicleModelEntity>>();

            dbSetMock.As<IDbAsyncEnumerable>().Setup(m => m.GetAsyncEnumerator())
                        .Returns(new TestDbAsyncEnumerator<VehicleModelEntity>(models.GetEnumerator()));
            dbSetMock.As<IQueryable<VehicleModelEntity>>().Setup(m => m.Provider)
                        .Returns(new TestDbAsyncQueryProvider<VehicleModelEntity>(modelsAsQueryable.Provider));
            dbSetMock.As<IQueryable<VehicleModelEntity>>().Setup(m => m.Expression)
                        .Returns(modelsAsQueryable.Expression);
            dbSetMock.As<IQueryable<VehicleModelEntity>>().Setup(m => m.ElementType)
                        .Returns(modelsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<VehicleModelEntity>>().Setup(m => m.GetEnumerator())
                        .Returns(() => modelsAsQueryable.GetEnumerator());
            dbSetMock.Setup(x => x.Add(It.IsAny<VehicleModelEntity>()))
                .Returns((VehicleModelEntity m) => m)
                .Callback((VehicleModelEntity m) => models.Add(m));
            dbSetMock.Setup(x => x.Attach(It.IsAny<VehicleModelEntity>()))
                .Returns((VehicleModelEntity m) => m)
                .Callback((VehicleModelEntity m) =>
                {
                    var modelToUpdate = models.Find(x => x.Id == m.Id);
                    modelToUpdate.Name = m.Name;
                    modelToUpdate.Abrv = m.Abrv;
                });
            dbSetMock.Setup(x => x.Remove(It.IsAny<VehicleModelEntity>()))
               .Returns((VehicleModelEntity m) => m)
               .Callback((VehicleModelEntity m) =>
               {
                   var modelTD = models.Find(x => x.Id == m.Id);
                   models.Remove(modelTD);
               });

            Mock<IDbContext> contextMock = new Mock<IDbContext>();
            contextMock.Setup(x => x.Set<VehicleModelEntity>()).Returns(dbSetMock.Object);

            return Task.FromResult(contextMock.Object);
        }

        public Task<IMapper> CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            IMapper mapper = new Mapper(config);

            return Task.FromResult(mapper);
        }

        [Fact]
        public async Task ModelService_FindModelsAsync_Should_Return_PageList_IVehicleModel()
        {
            var models = await GetModels();
            IUnitOfWork uOW = new UnitOfWork(await CreateMockDBContext(models), await CreateMapper());
            IModelRepository modelRepo = new ModelRepository(await CreateMockDBContext(models), await CreateMapper());
            IModelService modelService = new ModelService(uOW, modelRepo);
            PagingParams pagingParams = new PagingParams();
            SortingParams sortingParams = new SortingParams
            {
                OrderBy = "model_name_desc"
            };
            FilteringParams filteringParams = new FilteringParams();

            var actual = await modelService.FindModelsAsync(pagingParams, sortingParams, filteringParams);

            actual.Should().BeOfType(typeof(PagedList<IVehicleModel>));
            actual.HasNext.Should().BeTrue();
            actual.HasPrevious.Should().BeFalse();
            actual.Count().Should().Be(5);
            actual.Should().BeInDescendingOrder(x => x.Name);
        }

        [Fact]
        public async Task ModelService_GetModelAsync_Should_Return_IVehicleModel()
        {
            var models = await GetModels();
            var mapper = await CreateMapper();
            IUnitOfWork uOW = new UnitOfWork(await CreateMockDBContext(models), await CreateMapper());
            IModelRepository modelRepo = new ModelRepository(await CreateMockDBContext(models), await CreateMapper());
            IModelService modelService = new ModelService(uOW, modelRepo);
            var model = new VehicleModelEntity()
            {
                Id = 3
            };
            var modelToFind = mapper.Map<IVehicleModel>(model);

            var actual = await modelService.GetModelAsync(modelToFind);

            actual.Should().NotBeNull();
            actual.Should().Equals(modelToFind);
        }


        [Fact]
        public async Task ModelService_InsertModelAsync_Should_Add_VehicleModelEntity_Object_From_IVehicleModel()
        {
            var models = await GetModels();
            var mapper = await CreateMapper();
            IUnitOfWork uOW = new UnitOfWork(await CreateMockDBContext(models), await CreateMapper());
            IModelRepository modelRepo = new ModelRepository(await CreateMockDBContext(models), await CreateMapper());
            IModelService modelService = new ModelService(uOW, modelRepo);
            var newModel = new VehicleModelEntity()
            {
                Id = 11,
                Name = "Model11",
                Abrv = "M11"
            };
            var modelToInsert = mapper.Map<IVehicleModel>(newModel);

            var actual = await modelService.InsertModelAsync(modelToInsert);

            actual.Should().Be(1);
            models.Last().Id.Should().Be(newModel.Id);
            models.Count.Should().Be(11);
        }

        [Fact]
        public async Task ModelService_DeleteModelAsync_Should_Delete_VehicleModel_Object_From_IVehicleModel()
        {
            var models = await GetModels();
            var mapper = await CreateMapper();
            IUnitOfWork uOW = new UnitOfWork(await CreateMockDBContext(models), await CreateMapper());
            IModelRepository modelRepo = new ModelRepository(await CreateMockDBContext(models), await CreateMapper());
            IModelService modelService = new ModelService(uOW, modelRepo);
            var model = new VehicleModelEntity()
            {
                Id = 3,
                Name = "Model3",
                Abrv = "M3"
            };
            var modelToDelete = mapper.Map<IVehicleModel>(model);

            var actual = await modelService.DeleteModelAsync(modelToDelete);

            actual.Should().Be(1);
            models.Count().Should().Be(9);
        }
    }
}
