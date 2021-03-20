using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;
using Vehicle.Common.Helpers;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Service.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ModelService(IUnitOfWork unitOfWork, IModelRepository modelRepository)
        {
            _unitOfWork = unitOfWork;
            _modelRepository = modelRepository;
        }

        public async Task<PagedList<IVehicleModel>> FindModelsAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering)
        {
            return await _modelRepository.FindModelsAsync(paging, sorting, filtering);
        }

        public async Task<IVehicleModel> GetModelAsync(IVehicleModel model)
        {
            return await _modelRepository.GetModelByIdAsync(model.Id);
        }

        public async Task<int> InsertModelAsync(IVehicleModel model)
        {
            try
            {
                _unitOfWork.ModelRepository.CreateModelAsync(model);
                await _unitOfWork.CommitAsync();

                return 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async Task<int> UpdateModelAsync(IVehicleModel model)
        {
            try
            {
                _unitOfWork.ModelRepository.UpdateModelAsync(model);
                await _unitOfWork.CommitAsync();

                return 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async Task<int> DeleteModelAsync(IVehicleModel model)
        {
            try
            {
                //var modelToDelete = _mapper.Map<VehicleModelEntity>(model);

                _unitOfWork.ModelRepository.DeleteModelAsync(model);
                await _unitOfWork.CommitAsync();

                return 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }
    }
}
