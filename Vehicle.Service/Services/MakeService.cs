using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.Model.Models;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;

namespace Vehicle.Service.Services
{
    public class MakeService : IMakeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MakeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<VehicleMake>> GetMakesAsync()
        {
            return await _unitOfWork.MakeRepository.GetAllAsync();
        }

        public async Task<VehicleMake> GetMakeAsync(int id)
        {
            return await _unitOfWork.MakeRepository.GetByIdAsync(id);
        }

        public async void InsertMakeAsync(VehicleMake make)
        {
            try
            {
                if (make == null)
                {
                    throw new ArgumentNullException("make");
                }

                await _unitOfWork.MakeRepository.InsertAsync(make);
                await _unitOfWork.CommitAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async void UpdateMakeAsync(VehicleMake make)
        {
            try
            {
                if (make == null)
                {
                    throw new ArgumentNullException("make");
                }

                await _unitOfWork.MakeRepository.UpdateAsync(make);
                await _unitOfWork.CommitAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async void DeleteMakeAsync(VehicleMake make)
        {
            try
            {
                if (make == null)
                {
                    throw new ArgumentNullException("make");
                }

                await _unitOfWork.MakeRepository.DeleteAsync(make);
                await _unitOfWork.CommitAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }
    }
}
