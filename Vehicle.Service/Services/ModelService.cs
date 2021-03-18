using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;
using Vehicle.Common.Helpers;
using AutoMapper;
using Vehicle.Model.Common.Interfaces;
using Vehicle.DAL.Entities;

namespace Vehicle.Service.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ModelService(IUnitOfWork unitOfWork, IMapper mapper, IModelRepository modelRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _modelRepository = modelRepository;
        }

        public async Task<PagedList<IVehicleModel>> FindModelsAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering)
        {

            var models = await _modelRepository.FindModelsAsync(paging, sorting, filtering);
            var pagedModels = _mapper.Map<List<VehicleModelEntity>, List<IVehicleModel>>(models);

            return new PagedList<IVehicleModel>(pagedModels, models.TotalCount, models.CurrentPage, models.PageSize);
        }

        public async Task<IVehicleModel> GetModelAsync(IVehicleModel model)
        {
            return _mapper.Map<IVehicleModel>
                (
                    await _modelRepository.GetModelByIdAsync(model.Id)
                );             
        }

        public async Task<int> InsertModelAsync(IVehicleModel model)
        {
            try
            {
                var modelToInsert = _mapper.Map<VehicleModelEntity>(model);

                await _modelRepository.InsertAsync(modelToInsert);
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
                var modelToUpdate = _mapper.Map<VehicleModelEntity>(model);

                await _modelRepository.UpdateAsync(modelToUpdate);
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
                var modelToDelete = _mapper.Map<VehicleModelEntity>(model);

                await _modelRepository.DeleteAsync(modelToDelete);
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
