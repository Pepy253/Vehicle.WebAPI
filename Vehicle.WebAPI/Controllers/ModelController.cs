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
    public class ModelController : ApiController
    {
        private readonly IModelService _modelService;
        private readonly IMapper _mapper;

        public ModelController(IModelService modelService, IMapper mapper)
        {
            _modelService = modelService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Models([FromUri] PagingParams paging, [FromUri] SortingParams sorting, [FromUri]FilteringParams filtering)
        {            
            var models = await _modelService.FindModelsAsync(paging, sorting, filtering);
            var modelsDTO = _mapper.Map<List<IVehicleModel>, List<VehicleModelDTO>>(models);

            var metadata = new
            {
                models.TotalCount,
                models.PageSize,
                models.CurrentPage,
                models.TotalPages,
                models.HasNext,
                models.HasPrevious
            };

            var response = Request.CreateResponse
                (
                    HttpStatusCode.OK,
                    new PagedList<VehicleModelDTO>(modelsDTO, models.TotalCount, models.CurrentPage, models.PageSize),
                    Configuration.Formatters.JsonFormatter
                );
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(metadata));

            return response;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetModel([FromUri]VehicleModelDTO modelDTO)
        {
            var model = await _modelService.GetModelAsync(_mapper.Map<IVehicleModel>(modelDTO));

            return Request.CreateResponse
                (
                    HttpStatusCode.OK,
                    _mapper.Map<VehicleModelDTO>(model),
                    Configuration.Formatters.JsonFormatter
                );
        }

        [HttpPost]
        public async Task<HttpResponseMessage> InsertModel([FromBody] VehicleModelDTO modelDTO)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            await _modelService.InsertModelAsync(_mapper.Map<IVehicleModel>(modelDTO));

            return Request.CreateResponse(HttpStatusCode.Created, "Vehicle model successfully created!");
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateModel([FromBody] VehicleModelDTO modelDTO)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            await _modelService.UpdateModelAsync(_mapper.Map<IVehicleModel>(modelDTO));

            return Request.CreateResponse(HttpStatusCode.OK, "Vehicle model updated successfully!");
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteModel([FromUri] VehicleModelDTO modelDTO)
        {
            if (modelDTO == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Vehicle model not found!");
            }

            await _modelService.DeleteModelAsync(_mapper.Map<IVehicleModel>(modelDTO));

            return Request.CreateResponse(HttpStatusCode.OK, "Vehicle model successfully deleted");
        }
    }
}
