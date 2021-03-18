using AutoMapper;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Entities;
using Vehicle.Model.Common.Interfaces;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;

namespace Vehicle.Service.Services
{
    public class MakeService : IMakeService
    {
        private readonly IMakeRepository _makeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MakeService(IUnitOfWork unitOfWork, IMapper mapper, IMakeRepository makeReposiotry)
        {
            _makeRepository = makeReposiotry;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedList<IVehicleMake>> FindMakesAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering)
        {
            var makes = await _makeRepository.FindMakesAsync(paging, sorting, filtering);
            var makesDTO = _mapper.Map<List<VehicleMakeEntity>, List<IVehicleMake>>(makes);            

            return new PagedList<IVehicleMake>(makesDTO, makes.TotalCount, makes.CurrentPage, makes.PageSize);
        }

        public async Task<IVehicleMake> GetMakeAsync(IVehicleMake make)
        {          
            var vehicleMake = _mapper.Map<IVehicleMake>
                (
                    await _makeRepository.GetMakeByIdAsync
                    (
                        make.Id
                    )
                );

            return vehicleMake;
        }

        public async Task<int> InsertMakeAsync(IVehicleMake make)
        {
            try
            {

                var makeToAdd = _mapper.Map<VehicleMakeEntity>(make);

                _unitOfWork.MakeRepository.CreateMakeAsync(makeToAdd);
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
                var makeToUpdate = _mapper.Map<VehicleMakeEntity>(make);

                _unitOfWork.MakeRepository.UpdateMakeAsync(makeToUpdate);
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
                var makeToDelete = _mapper.Map<VehicleMakeEntity>(make);

                _unitOfWork.MakeRepository.DeleteMakeAsync(makeToDelete);
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
