using System.Linq;
using System.Threading.Tasks;
using Vehicle.DAL.Intefaces;
using Vehicle.Model.Entities;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Common.Helpers;
using System.Data.Entity;

namespace Vehicle.Repository.Repositories
{
    public class ModelRepository : Repository<VehicleModel>, IModelRepository 
    {
        public ModelRepository(IDbContext _context) 
                : base (_context)
        {
        }
       
        public async Task<PagedList<VehicleModel>> FindModelsAsync(QueryStringParameters qSParameters)
        {
            var models = await GetAsync();

            FilterModels(ref models, qSParameters.SearchString);

            ApplySort(ref models, qSParameters.OrderBy);

            return await PagedList<VehicleModel>.ToPagedListAsync
                (
                    models,
                    qSParameters.PageNumber,
                    qSParameters.PageSize
                );
        }

        private void FilterModels(ref IQueryable<VehicleModel> models, string searchString)
        {
            if (!models.Any() || string.IsNullOrWhiteSpace(searchString))
            {
                return;
            }

            models = models.Where(mo => mo.Name.ToLower() == searchString.ToLower()
                || mo.VehicleMake.Name.ToLower() == searchString.ToLower());
        }

        private void ApplySort(ref IQueryable<VehicleModel> models, string sortOrder)
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


        public async Task<VehicleModel> GetModelByIdAsync(int id)
        {
            var models = await GetAsync();

            return await models.Where(x => x.Id == id).FirstOrDefaultAsync(); 
        }

        public async  void CreateModelAsync(VehicleModel model)
        {
            await InsertAsync(model);
        }

        public async void UpdateModelAsync(VehicleModel model)
        {
            await UpdateAsync(model);
        }        

        public async void DeleteModelAsync(VehicleModel model)
        {
            await DeleteAsync(model);
        }
    }
}
