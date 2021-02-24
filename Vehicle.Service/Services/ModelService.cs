using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Model.Models;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;
using Vehicle.Common.Helpers;

namespace Vehicle.Service.Services
{
    public class ModelService : IModelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ModelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<VehicleModel>> GetModelsAsync()
        {
            return await _unitOfWork.ModelRepository.GetAllAsync();
        }

        public async Task<VehicleModel> GetModelAsync(int id)
        {
            return await _unitOfWork.ModelRepository.GetByIdAsync(id);
        }

        public async void InsertModelAsync(VehicleModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }

                await _unitOfWork.ModelRepository.InsertAsync(model);
                await _unitOfWork.CommitAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async void UpdateModelAsync(VehicleModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }

                await _unitOfWork.ModelRepository.UpdateAsync(model);
                await _unitOfWork.CommitAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async void DeleteModelAsync(VehicleModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }

                await _unitOfWork.ModelRepository.DeleteAsync(model);
                await _unitOfWork.CommitAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }
    }
}
