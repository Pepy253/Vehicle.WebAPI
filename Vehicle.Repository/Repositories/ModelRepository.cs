using System.Linq;
using System.Threading.Tasks;
using Vehicle.DAL.Intefaces;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Common.Helpers;
using System.Data.Entity;
using Vehicle.DAL.Entities;

namespace Vehicle.Repository.Repositories
{
    public class ModelRepository : Repository<VehicleModelEntity>, IModelRepository 
    {
        public ModelRepository(IDbContext _context) 
                : base (_context)
        {
        }
       
        public async Task<PagedList<VehicleModelEntity>> FindModelsAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering)
        {
            var models = await GetAsync();

            models = models.Include(x => x.VehicleMake);

            FilterModels(ref models, filtering.SearchString);

            ApplySort(ref models, sorting.OrderBy);

            return await PagedList<VehicleModelEntity>.ToPagedListAsync
                (
                    models,
                    paging.PageNumber,
                    paging.PageSize
                );
        }

        private void FilterModels(ref IQueryable<VehicleModelEntity> models, string searchString)
        {
            if (!models.Any() || string.IsNullOrWhiteSpace(searchString))
            {
                return;
            }

            models = models.Where(mo => mo.Name.ToLower() == searchString.ToLower()
                || mo.VehicleMake.Name.ToLower() == searchString.ToLower());
        }

        private void ApplySort(ref IQueryable<VehicleModelEntity> models, string sortOrder)
        {
            switch (sortOrder)
            {
                case "model_name":
                    models = models.OrderBy(x => x.Name);
                    break;
                case "model_name_desc":
                    models = models.OrderByDescending(x => x.Name);
                    break;
                case "make_name_desc":
                    models = models.OrderByDescending(x => x.VehicleMake.Name);
                    break;
                default:
                    models = models.OrderBy(x => x.VehicleMake.Name);
                    break;
            }
        }


        public async Task<VehicleModelEntity> GetModelByIdAsync(int id)
        {
            var models = await GetAsync();

            return await models.Include(x => x.VehicleMake).Where(x => x.Id == id).FirstOrDefaultAsync(); 
        }

        public async  void CreateModelAsync(VehicleModelEntity model)
        {
            await InsertAsync(model);
        }

        public async void UpdateModelAsync(VehicleModelEntity model)
        {
            await UpdateAsync(model);
        }        

        public async void DeleteModelAsync(VehicleModelEntity model)
        {
            await DeleteAsync(model);
        }
    }
}
