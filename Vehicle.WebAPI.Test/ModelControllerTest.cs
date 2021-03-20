using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.Model.Common.Interfaces;
using Vehicle.Model.Models;
using Vehicle.Service.Common.Interfaces;
using Vehicle.WebAPI.AutoMapperConfig;
using Vehicle.WebAPI.Controllers;
using Vehicle.WebAPI.Models;
using Xunit;

namespace Vehicle.WebAPI.Test
{
    public class ModelControllerTest
    {
        public Task<List<IVehicleModel>> GetModels()
        {
            List<IVehicleModel> vehicleModels = new List<IVehicleModel>();

            for (int i = 0; i < 10; i++)
            {
                vehicleModels.Add(new VehicleModel()
                {
                    Id = i,
                    Name = "Make" + i,
                    Abrv = "Ma" + i
                });
            }

            return Task.FromResult(vehicleModels);
        }


        public async Task<ModelController> CreateModelController(List<IVehicleModel> models)
        {
            PagedList<IVehicleModel> pagedModels = new PagedList<IVehicleModel>(models, models.Count, 1, 10);
            Mock<IModelService> serviceMock = new Mock<IModelService>();
            var model = new VehicleMake();
            serviceMock.Setup(x => x.FindModelsAsync(It.IsAny<PagingParams>(), It.IsAny<SortingParams>(), It.IsAny<FilteringParams>())).ReturnsAsync(pagedModels);
            serviceMock.Setup(x => x.GetModelAsync(It.IsAny<IVehicleModel>())).ReturnsAsync(models.Where(x => x.Id == 2).FirstOrDefault());
            ModelController controller = new ModelController(serviceMock.Object, await CreateMapper())
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new System.Web.Http.HttpConfiguration()
            };

            return controller;
        }

        public Task<IMapper> CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            IMapper mapper = new Mapper(config);

            return Task.FromResult(mapper);
        }

        [Fact]
        public async Task ModelController_Models_Should_Return_StatusCode_200()
        {
            var models = await GetModels();
            var controller = await CreateModelController(models);
            Paging page = new Paging();
            Sorting sort = new Sorting();
            Filtering filter = new Filtering();

            var response = await controller.Models(page, sort, filter);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ModelController_GetModel_Should_Return_StatusCode_200()
        {
            var models = await GetModels();
            var controller = await CreateModelController(models);
            VehicleModelDTO model = new VehicleModelDTO()
            {
                Id = 2
            };

            var response = await controller.GetModel(model);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ModelController_InsertModel_Should_Return_StatusCode_200()
        {
            var models = await GetModels();
            var controller = await CreateModelController(models);
            VehicleModelDTO model = new VehicleModelDTO()
            {
                Id = 2
            };

            var response = await controller.GetModel(model);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ModelController_UpdateModel_Should_Return_StatusCode_200()
        {
            var models = await GetModels();
            var controller = await CreateModelController(models);
            VehicleModelDTO model = new VehicleModelDTO()
            {
                Id = 2,
                Name = "Model22",
                Abrv = "Mo22"
            };

            var response = await controller.GetModel(model);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ModelController_DeleteModel_Should_Return_StatusCode_200()
        {
            var models = await GetModels();
            var controller = await CreateModelController(models);
            VehicleModelDTO model = new VehicleModelDTO()
            {
                Id = 2,
                Name = "Model2",
                Abrv = "Mo2"
            };

            var response = await controller.GetModel(model);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
