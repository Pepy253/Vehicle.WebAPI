using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Vehicle.DAL.Intefaces;
using Vehicle.Model.Entities;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Common.Helpers;

namespace Vehicle.Repository.Repositories
{
    public class MakeRepository : Repository<VehicleMake>, IMakeReposiotry
    {
        public MakeRepository(IDbContext _context) 
                : base(_context)
        {      
        }

        public async Task<PagedList<VehicleMake>> FindMakesAsync(QueryStringParameters qSParameters)
        {
            var makes = await GetAsync();

            FilterMakes(ref makes, qSParameters.SearchString);

            ApplySort(ref makes, qSParameters.OrderBy);

            return await PagedList<VehicleMake>.ToPagedListAsync
                (
                    makes, 
                    qSParameters.PageNumber, 
                    qSParameters.PageSize
                );            
        }

        private void FilterMakes(ref IQueryable<VehicleMake> makes, string searchString)
        {
            if (!makes.Any() || string.IsNullOrWhiteSpace(searchString))
            {
                return;
            }

            makes = makes.Where(m => m.Name.ToLower() == searchString.ToLower());
        }

        private void ApplySort(ref IQueryable<VehicleMake> makes, string sortOrder)
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

        public async Task<VehicleMake> GetMakeByIdAsync(int id)
        {
            var make = await GetAsync();

            return await make.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async void CreateMakeAsync(VehicleMake make)
        {
            await InsertAsync(make);
        }

        public async void UpdateMakeAsync(VehicleMake make)
        {
            await UpdateAsync(make);
        }

        public async void DeleteMakeAsync(VehicleMake make)
        {
            await DeleteAsync(make);
        }
    }
}
