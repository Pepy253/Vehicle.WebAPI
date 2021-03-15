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
    public class MakeController : ApiController
    {
        private readonly IMakeService _makeService;

        public MakeController(IMakeService makeService)
        {
            _makeService = makeService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Makes([FromUri]QueryStringParameters qSParameters)
        {
            var makes = await _makeService.FindMakesAsync(qSParameters);

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
                    await _makeService.FindMakesAsync(qSParameters),
                    Configuration.Formatters.JsonFormatter
                );

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(metadata));

            return response;  
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetMake([FromUri]VehicleMakeDTO makeDTO)
        {
            var make = await _makeService.GetMakeAsync(makeDTO);

            if (make == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Vehicle make with specified id was not found!");
            }

            return Request.CreateResponse
                (
                    HttpStatusCode.OK,
                    await _makeService.GetMakeAsync(makeDTO),
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

            await _makeService.InsertMakeAsync(makeDTO);

            return Request.CreateResponse(HttpStatusCode.Created, "Vehicle make successfully created!");
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateMake([FromBody]VehicleMakeDTO makeDTO)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            await _makeService.UpdateMakeAsync(makeDTO);

            return Request.CreateResponse(HttpStatusCode.OK, "Vehicle make updated successfully!");
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteMake([FromUri]VehicleMakeDTO makeDTO)
        {
            if (makeDTO == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Vehicle make not found!");
            }

            await _makeService.DeleteMakeAsync(makeDTO);

            return Request.CreateResponse(HttpStatusCode.OK, "Vehicle make successfully deleted");
        }
    }
}
