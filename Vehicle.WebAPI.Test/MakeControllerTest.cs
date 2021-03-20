using AutoMapper;
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
using Vehicle.DAL.Intefaces;
using Vehicle.Model.Common.Interfaces;
using Vehicle.Model.Models;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;
using Vehicle.WebAPI.AutoMapperConfig;
using Vehicle.WebAPI.Controllers;
using Vehicle.WebAPI.Models;
using System.Web.Http.Results;
using Xunit;
using System.Net;

namespace Vehicle.WebAPI.Test
{
    public class MakeControllerTest
    {
        public Task<List<IVehicleMake>> GetMakes()
        {
            List<IVehicleMake> vehicleMakes = new List<IVehicleMake>();

            for (int i = 0; i < 10; i++)
            {
                vehicleMakes.Add(new VehicleMake()
                {
                    Id = i,
                    Name = "Make" + i,
                    Abrv = "Ma" + i
                });
            }

            return Task.FromResult(vehicleMakes);
        }


        public async Task<MakeController> CreateMakeController(List<IVehicleMake>makes)
        {
            PagedList<IVehicleMake> pagedMakes = new PagedList<IVehicleMake>(makes, makes.Count, 1, 10);
            Mock<IMakeService> serviceMock = new Mock<IMakeService>();
            PagingParams pageParam = new PagingParams();
            SortingParams sortParam = new SortingParams();
            FilteringParams filterParam = new FilteringParams();
            var make = new VehicleMake();
            serviceMock.Setup(x => x.FindMakesAsync(It.IsAny<PagingParams>(), It.IsAny<SortingParams>(), It.IsAny<FilteringParams>())).ReturnsAsync(pagedMakes);
            serviceMock.Setup(x => x.GetMakeAsync(It.IsAny<IVehicleMake>())).ReturnsAsync(makes.Where(x => x.Id == 2).FirstOrDefault());
            MakeController controller = new MakeController(serviceMock.Object, await CreateMapper())
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
        public async Task MakeController_Makes_StatusCode_200()
        {
            var makes = await GetMakes();
            var controller = await CreateMakeController(makes);
            Paging page = new Paging();
            Sorting sort = new Sorting();
            Filtering filter = new Filtering();

            var response = await controller.Makes(page, sort, filter);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task MakeController_GetMake_Should_Return_StatusCode_200()
        {
            var makes = await GetMakes();
            var controller = await CreateMakeController(makes);

            var response = await controller.GetMake(new VehicleMakeDTO() { Id = 2 });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task MakeController_InsertMake_Should_Return_StatusCode_200()
        {
            var makes = await GetMakes();
            var controller = await CreateMakeController(makes);
            VehicleMakeDTO make = new VehicleMakeDTO()
            {
                Id = 11,
                Name = "Make11",
                Abrv = "Ma11"
            };


            var response = await controller.InsertMake(make);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task MakeController_UpdateMake_Should_Return_StatusCode_200()
        {
            var makes = await GetMakes();
            var controller = await CreateMakeController(makes);
            VehicleMakeDTO make = new VehicleMakeDTO()
            {
                Id = 2,
                Name = "Make22",
                Abrv = "Ma22"
            };

            var response = await controller.UpdateMake(make);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task MakeController_DeleteMake_Should_Return_StatusCode_200()
        {
            var makes = await GetMakes();
            var controller = await CreateMakeController(makes);
            VehicleMakeDTO make = new VehicleMakeDTO()
            {
                Id = 3,
                Name = "Make3",
                Abrv = "Ma3"
            };

            var response = await controller.DeleteMake(make);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
