using AutoMapper;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.Model.Common.Interfaces;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;

namespace Vehicle.Service.Services
{
    public class MakeService : IMakeService
    {
        private readonly IMakeRepository _makeRepository;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;

        public MakeService(IUnitOfWork unitOfWork, /*IMapper mapper,*/ IMakeRepository makeReposiotry)
        {
            _makeRepository = makeReposiotry;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedList<IVehicleMake>> FindMakesAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering)
        {    
            return await _makeRepository.FindMakesAsync(paging, sorting, filtering);
        }

        public async Task<IVehicleMake> GetMakeAsync(IVehicleMake make)
        {          
            return await _makeRepository.GetMakeByIdAsync(make.Id);
        }

        public async Task<int> InsertMakeAsync(IVehicleMake make)
        {
            try
            {
                _unitOfWork.MakeRepository.CreateMakeAsync(make);
                await _unitOfWork.CommitAsync();

                return 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async Task<int> UpdateMakeAsync(IVehicleMake make)
        {
            try
            {
                _unitOfWork.MakeRepository.UpdateMakeAsync(make);
                await _unitOfWork.CommitAsync();

                return 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async Task<int> DeleteMakeAsync(IVehicleMake make)
        {
            try
            {
                _unitOfWork.MakeRepository.DeleteMakeAsync(make);
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
