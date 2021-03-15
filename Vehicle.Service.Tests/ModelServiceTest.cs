using AutoMapper;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Intefaces;
using Vehicle.Model.Common.Interfaces;
using Vehicle.Model.Entities;
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

        public Task<List<VehicleModel>> GetModels()
        {
            List<VehicleModel> models = new List<VehicleModel>();

            for (int i = 1; i <= 10; i++)
            {
                models.Add(new VehicleModel()
                {
                    Id = i,
                    Name = "Model" + i,
                    Abrv = "M" + i
                });
            }

            return Task.FromResult(models);
        }

        public Task<IUnitOfWork> CreateUnitOfWork(List<VehicleModel> models)
        {
            var modelsAsQueryable = models.AsQueryable();
            var dbSetMock = new Mock<IDbSet<VehicleModel>>();

            dbSetMock.As<IDbAsyncEnumerable>().Setup(m => m.GetAsyncEnumerator())
                        .Returns(new TestDbAsyncEnumerator<VehicleModel>(models.GetEnumerator()));
            dbSetMock.As<IQueryable<VehicleModel>>().Setup(m => m.Provider)
                        .Returns(new TestDbAsyncQueryProvider<VehicleModel>(modelsAsQueryable.Provider));
            dbSetMock.As<IQueryable<VehicleModel>>().Setup(m => m.Expression)
                        .Returns(modelsAsQueryable.Expression);
            dbSetMock.As<IQueryable<VehicleModel>>().Setup(m => m.ElementType)
                        .Returns(modelsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<VehicleModel>>().Setup(m => m.GetEnumerator())
                        .Returns(() => modelsAsQueryable.GetEnumerator());
            dbSetMock.Setup(x => x.Add(It.IsAny<VehicleModel>()))
                .Returns((VehicleModel m) => m)
                .Callback((VehicleModel m) => models.Add(m));
            dbSetMock.Setup(x => x.Attach(It.IsAny<VehicleModel>()))
                .Returns((VehicleModel m) => m)
                .Callback((VehicleModel m) =>
                {
                    var modelToUpdate = models.Find(x => x.Id == m.Id);
                    modelToUpdate.Name = m.Name;
                    modelToUpdate.Abrv = m.Abrv;
                });
            dbSetMock.Setup(x => x.Remove(It.IsAny<VehicleModel>()))
               .Returns((VehicleModel m) => m)
               .Callback((VehicleModel m) =>
               {
                   var modelTD = models.Find(x => x.Id == m.Id);
                   models.Remove(modelTD);
               });

            Mock<IDbContext> contextMock = new Mock<IDbContext>();
            contextMock.Setup(x => x.Set<VehicleModel>()).Returns(dbSetMock.Object);
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
        public async Task VehicleModelService_FindAsync_Should_Return_PageList_IVehicleModelDTO()
        {
            var models = await GetModels();
            IModelService modelService = new ModelService(await CreateUnitOfWork(models), await CreateMapper());
            QueryStringParameters qSParameters = new QueryStringParameters();

            var actual = await modelService.FindModelsAsync(qSParameters);

            actual.Should().BeOfType(typeof(PagedList<IVehicleModelDTO>));
        }

        [Fact]
        public async Task VehicleModelService_GetByIdAsync_Should_Return_IVehicleModelDTO()
        {
            var models = await GetModels();
            var mapper = await CreateMapper();
            IModelService modelService = new ModelService(await CreateUnitOfWork(models), mapper);
            var model = new VehicleModel()
            {
                Id = 3
            };
            var modelToFind = mapper.Map<IVehicleModelDTO>(model);

            var actual = await modelService.GetModelAsync(modelToFind);

            actual.Should().NotBeNull();
            actual.Should().Equals(modelToFind);
        }


        [Fact]
        public async Task VehicleModelService_InsertModelAsync_Should_Add_VehicleModel_Object_From_IVehicleModelDTO()
        {
            var models = await GetModels();
            var mapper = await CreateMapper();
            IModelService modelService = new ModelService(await CreateUnitOfWork(models), mapper);
            var newModel = new VehicleModel()
            {
                Id = 11,
                Name = "Model11",
                Abrv = "M11"
            };
            var modelToInsert = mapper.Map<IVehicleModelDTO>(newModel);

            var actual = await modelService.InsertModelAsync(modelToInsert);

            actual.Should().Be(1);
            models.Last().Id.Should().Be(newModel.Id);
            models.Count.Should().Be(11);
        }

        [Fact]
        public async Task VehicleModelService_DeleteModelAsync_Should_Delete_VehicleModel_Object_From_IVehicleModelDTO()
        {
            var models = await GetModels();
            var mapper = await CreateMapper();
            IModelService modelService = new ModelService(await CreateUnitOfWork(models), mapper);
            var model = new VehicleModel()
            {
                Id = 3,
                Name = "Model3",
                Abrv = "M3"
            };
            var modelToDelete = mapper.Map<IVehicleModelDTO>(model);

            var actual = await modelService.DeleteModelAsync(modelToDelete);

            actual.Should().Be(1);
            models.Count().Should().Be(9);
        }
    }
}
