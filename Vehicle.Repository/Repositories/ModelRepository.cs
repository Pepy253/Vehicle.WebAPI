using System.Linq;
using System.Threading.Tasks;
using Vehicle.DAL.Intefaces;
using Vehicle.Repository.Common.Interfaces;
using Vehicle.Common.Helpers;
using System.Data.Entity;
using Vehicle.DAL.Entities;
using Vehicle.Model.Common.Interfaces;
using AutoMapper;
using System.Collections.Generic;

namespace Vehicle.Repository.Repositories
{
    public class ModelRepository : Repository<VehicleModelEntity>, IModelRepository 
    {
        private readonly IMapper _mapper;
        public ModelRepository(IDbContext _context, IMapper mapper) 
                : base (_context)
        {
            _mapper = mapper;
        }
       
        public async Task<PagedList<IVehicleModel>> FindModelsAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering)
        {
            var models = await GetAsync();

            models = models.Include(x => x.VehicleMake);

            FilterModels(ref models, filtering.SearchString);

            ApplySort(ref models, sorting.OrderBy);

            var pagedModels =  await PagedList<VehicleModelEntity>.ToPagedListAsync
                (
                    models,
                    paging.PageNumber,
                    paging.PageSize
                );

            var listIModels = _mapper.Map<List<VehicleModelEntity>, List<IVehicleModel>>(pagedModels);

            return new PagedList<IVehicleModel>(listIModels, pagedModels.TotalCount, pagedModels.CurrentPage, pagedModels.PageSize);

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


        public async Task<IVehicleModel> GetModelByIdAsync(int id)
        {
            var models = await GetAsync();

            return _mapper.Map<IVehicleModel>(await models.Include(x => x.VehicleMake)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync()); 
        }

        public async void CreateModelAsync(IVehicleModel model)
        {
            await InsertAsync(_mapper.Map<VehicleModelEntity>(model));
        }

        public async void UpdateModelAsync(IVehicleModel model)
        {
            await UpdateAsync(_mapper.Map<VehicleModelEntity>(model));
        }        

        public async void DeleteModelAsync(IVehicleModel model)
        {
            await DeleteAsync(_mapper.Map<VehicleModelEntity>(model));
        }
    }
}
