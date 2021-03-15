using AutoMapper;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.Model.Common.Interfaces;
using Vehicle.Model.Entities;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Service.Common.Interfaces;

namespace Vehicle.Service.Services
{
    public class MakeService : IMakeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MakeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedList<IVehicleMakeDTO>> FindMakesAsync(QueryStringParameters qSParameters)
        {
            var makes = await _unitOfWork.MakeRepository.FindMakesAsync(qSParameters);
            var makesDTO = _mapper.Map<List<VehicleMake>, List<IVehicleMakeDTO>>(makes);            

            return new PagedList<IVehicleMakeDTO>(makesDTO, makes.TotalCount, makes.CurrentPage, makes.PageSize);
        }

        public async Task<IVehicleMakeDTO> GetMakeAsync(IVehicleMakeDTO makeDTO)
        {          
            var make = _mapper.Map<IVehicleMakeDTO>
                (
                    await _unitOfWork.MakeRepository.GetMakeByIdAsync
                    (
                        makeDTO.Id
                    )
                );

            return make;
        }

        public async Task<int> InsertMakeAsync(IVehicleMakeDTO makeDTO)
        {
            try
            {

                var makeToAdd = _mapper.Map<VehicleMake>(makeDTO);

                await _unitOfWork.MakeRepository.InsertAsync(makeToAdd);
                await _unitOfWork.CommitAsync();

                return 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async Task<int> UpdateMakeAsync(IVehicleMakeDTO makeDTO)
        {
            try
            {
                var makeToUpdate = _mapper.Map<VehicleMake>(makeDTO);

                await _unitOfWork.MakeRepository.UpdateAsync(makeToUpdate);
                await _unitOfWork.CommitAsync();

                return 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ExceptionHelper.GetValidationException(dbEx);
            }
        }

        public async Task<int> DeleteMakeAsync(IVehicleMakeDTO makeDTO)
        {
            try
            {
                var makeToDelete = _mapper.Map<VehicleMake>(makeDTO);

                await _unitOfWork.MakeRepository.DeleteAsync(makeToDelete);
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
