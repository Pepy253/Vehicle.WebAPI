using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Vehicle.Model.Entities;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;
using Vehicle.Common.Helpers;
using AutoMapper;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Service.Services
{
    public class ModelService : IModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ModelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedList<IVehicleModelDTO>> FindModelsAsync(QueryStringParameters qSParameters)
        {

            var models = await _unitOfWork.ModelRepository.FindModelsAsync(qSParameters);
            var modelsDTO = _mapper.Map<List<VehicleModel>, List<IVehicleModelDTO>>(models);

            return new PagedList<IVehicleModelDTO>(modelsDTO, models.TotalCount, models.CurrentPage, models.PageSize);
        }

        public async Task<IVehicleModelDTO> GetModelAsync(IVehicleModelDTO modelDTO)
        {
            return _mapper.Map<IVehicleModelDTO>
                (
                    await _unitOfWork.ModelRepository.GetModelByIdAsync(modelDTO.Id)
                );             
        }

        public async Task<int> InsertModelAsync(IVehicleModelDTO modelDTO)
        {
            try
            {
                var modelToInsert = _mapper.Map<VehicleModel>(modelDTO);

                await _unitOfWork.ModelRepository.InsertAsync(modelToInsert);
                await _unitOfWork.CommitAsync();

                return 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async Task<int> UpdateModelAsync(IVehicleModelDTO modelDTO)
        {
            try
            {
                var modelToUpdate = _mapper.Map<VehicleModel>(modelDTO);

                await _unitOfWork.ModelRepository.UpdateAsync(modelToUpdate);
                await _unitOfWork.CommitAsync();

                return 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async Task<int> DeleteModelAsync(IVehicleModelDTO modelDTO)
        {
            try
            {
                var modelToDelete = _mapper.Map<VehicleModel>(modelDTO);

                await _unitOfWork.ModelRepository.DeleteAsync(modelToDelete);
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
