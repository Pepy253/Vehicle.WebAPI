using AutoMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Vehicle.Common.Helpers;
using Vehicle.Model.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;
using Vehicle.WebAPI.Models;

namespace Vehicle.WebAPI.Controllers
{
    public class MakeController : ApiController
    {
        private readonly IMakeService _makeService;
        private readonly IMapper _mapper;

        public MakeController(IMakeService makeService, IMapper mapper)
        {
            _makeService = makeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Makes([FromUri]PagingParams paging, [FromUri]SortingParams sorting, [FromUri]FilteringParams filtering)
        {
            var makes = await _makeService.FindMakesAsync(paging, sorting, filtering);
            var makesDTO = _mapper.Map<List<IVehicleMake>, List<VehicleMakeDTO>>(makes);

            var metadata = new
            {
                makes.TotalCount,
                makes.PageSize,
                makes.CurrentPage,
                makes.TotalPages,
                makes.HasNext,
                makes.HasPrevious
            };

            var response = Request.CreateResponse
                (
                    HttpStatusCode.OK,
                    new PagedList<VehicleMakeDTO>(makesDTO, makes.TotalCount, makes.CurrentPage, makes.PageSize),
                    Configuration.Formatters.JsonFormatter
                );

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(metadata));

            return response;  
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetMake([FromUri]VehicleMakeDTO makeDTO)
        {
            var make = await _makeService.GetMakeAsync(_mapper.Map<IVehicleMake>(makeDTO));

            if (make == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Vehicle make with specified id was not found!");
            }

            return Request.CreateResponse
                (
                    HttpStatusCode.OK,
                    _mapper.Map<VehicleMakeDTO>(make),
                    Configuration.Formatters.JsonFormatter
                );
        }

        [HttpPost]
        public async Task<HttpResponseMessage> InsertMake([FromBody]VehicleMakeDTO makeDTO)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            await _makeService.InsertMakeAsync(_mapper.Map<IVehicleMake>(makeDTO));

            return Request.CreateResponse(HttpStatusCode.Created, "Vehicle make successfully created!");
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateMake([FromBody]VehicleMakeDTO makeDTO)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            await _makeService.UpdateMakeAsync(_mapper.Map<IVehicleMake>(makeDTO));

            return Request.CreateResponse(HttpStatusCode.OK, "Vehicle make updated successfully!");
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteMake([FromUri]VehicleMakeDTO makeDTO)
        {
            if (makeDTO == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Vehicle make not found!");
            }

            await _makeService.DeleteMakeAsync(_mapper.Map<IVehicleMake>(makeDTO));

            return Request.CreateResponse(HttpStatusCode.OK, "Vehicle make successfully deleted");
        }
    }
}
