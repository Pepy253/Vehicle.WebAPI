using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Vehicle.DAL.Intefaces;
using Vehicle.DAL.Entities;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Common.Helpers;
using AutoMapper;
using Vehicle.Model.Common.Interfaces;
using System.Collections.Generic;

namespace Vehicle.Repository.Repositories
{
    public class MakeRepository : Repository<VehicleMakeEntity>, IMakeRepository
    {
        private readonly IMapper _mapper;
        public MakeRepository(IDbContext _context, IMapper mapper) 
                : base(_context)
        {
            _mapper = mapper;
        }

        public async Task<PagedList<IVehicleMake>> FindMakesAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering)
        {
            var makes = await GetAsync();

            FilterMakes(ref makes, filtering.SearchString);

            ApplySort(ref makes, sorting.OrderBy);

            var pagedMakes = await PagedList<VehicleMakeEntity>.ToPagedListAsync
                (
                    makes, 
                    paging.PageNumber, 
                    paging.PageSize
                );

            var listIMakes = _mapper.Map<List<VehicleMakeEntity>, List<IVehicleMake>>(pagedMakes);

            return new PagedList<IVehicleMake>(listIMakes, pagedMakes.TotalCount, pagedMakes.CurrentPage, pagedMakes.PageSize);
        }

        private void FilterMakes(ref IQueryable<VehicleMakeEntity> makes, string searchString)
        {
            if (!makes.Any() || string.IsNullOrWhiteSpace(searchString))
            {
                return;
            }

            makes = makes.Where(m => m.Name.ToLower() == searchString.ToLower());
        }

        private void ApplySort(ref IQueryable<VehicleMakeEntity> makes, string sortOrder) 
        {
            switch (sortOrder)
            {
                case "make_name_desc":
                    makes = makes.OrderByDescending(x => x.Name);
                    break;
                default:
                    makes = makes.OrderBy(x => x.Name);
                    break;
            }                
        }

        public async Task<IVehicleMake> GetMakeByIdAsync(int id)
        {
            var make = await GetAsync();

            return _mapper.Map<IVehicleMake>(await make.Where(x => x.Id == id)
                .FirstOrDefaultAsync());
        }

        public async void CreateMakeAsync(IVehicleMake make)
        {
            await InsertAsync(_mapper.Map<VehicleMakeEntity>(make));
        }

        public async void UpdateMakeAsync(IVehicleMake make)
        {
            await UpdateAsync(_mapper.Map<VehicleMakeEntity>(make));
        }

        public async void DeleteMakeAsync(IVehicleMake make)
        {
            await DeleteAsync(_mapper.Map<VehicleMakeEntity>(make));
        }
    }
}
