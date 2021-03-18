[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Vehicle.WebAPI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Vehicle.WebAPI.App_Start.NinjectWebCommon), "Stop")]

namespace Vehicle.WebAPI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Vehicle.DAL.Contexts;
    using Vehicle.DAL.Intefaces;
    using Vehicle.Repository.Common.Interfaces;
    using Vehicle.Repository.Repositories;
    using Vehicle.Service.Common.Interfaces;
    using Vehicle.Service.Services;
    using System.Web.Http;
    using Ninject.Web.WebApi;
    using Vehicle.Model.Common.Interfaces;
    using Vehicle.Model.Models;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));           
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                
                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new AutoMapperConfig.AutoMapperModule());

            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IMakeService>().To<MakeService>();
            kernel.Bind<IModelService>().To<ModelService>();
            kernel.Bind<IDbContext>().To<VehicleDbContext>();
            kernel.Bind<IMakeRepository>().To<MakeRepository>();
            kernel.Bind<IModelRepository>().To<ModelRepository>();
            kernel.Bind<IVehicleMake>().To<VehicleMake>();
            kernel.Bind<IVehicleModel>().To<VehicleModel>();
        }
    }
}