using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Vehicle.Common.Helpers;
using Vehicle.Model.DTOs;
using Vehicle.Service.Common.Interfaces;

namespace Vehicle.WebAPI.Controllers
{
    public class ModelController : ApiController
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Models([FromUri]QueryStringParameters qSParameters)
        {            
            var models = await _modelService.FindModelsAsync(qSParameters);

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
                    models,
                    Configuration.Formatters.JsonFormatter
                );
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(metadata));

            return response;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetModel([FromUri]VehicleModelDTO modelDTO)
        {
            return Request.CreateResponse
                (
                    HttpStatusCode.OK,
                    await _modelService.GetModelAsync(modelDTO),
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

            await _modelService.InsertModelAsync(modelDTO);

            return Request.CreateResponse(HttpStatusCode.Created, "Vehicle model successfully created!");
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateModel([FromBody] VehicleModelDTO modelDTO)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            await _modelService.UpdateModelAsync(modelDTO);

            return Request.CreateResponse(HttpStatusCode.OK, "Vehicle model updated successfully!");
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteModel([FromUri] VehicleModelDTO modelDTO)
        {
            if (modelDTO == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Vehicle model not found!");
            }

            await _modelService.DeleteModelAsync(modelDTO);

            return Request.CreateResponse(HttpStatusCode.OK, "Vehicle model successfully deleted");
        }
    }
}
