using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Vehicle.DAL.Intefaces;
using Vehicle.DAL.Entities;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Common.Helpers;

namespace Vehicle.Repository.Repositories
{
    public class MakeRepository : Repository<VehicleMakeEntity>, IMakeRepository
    {
        public MakeRepository(IDbContext _context) 
                : base(_context)
        {      
        }

        public async Task<PagedList<VehicleMakeEntity>> FindMakesAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering)
        {
            var makes = await GetAsync();

            FilterMakes(ref makes, filtering.SearchString);

            ApplySort(ref makes, sorting.OrderBy);

            return await PagedList<VehicleMakeEntity>.ToPagedListAsync
                (
                    makes, 
                    paging.PageNumber, 
                    paging.PageSize
                );            
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

        public async Task<VehicleMakeEntity> GetMakeByIdAsync(int id)
        {
            var make = await GetAsync();

            return await make.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async void CreateMakeAsync(VehicleMakeEntity make)
        {
            await InsertAsync(make);
        }

        public async void UpdateMakeAsync(VehicleMakeEntity make)
        {
            await UpdateAsync(make);
        }

        public async void DeleteMakeAsync(VehicleMakeEntity make)
        {
            await DeleteAsync(make);
        }
    }
}
