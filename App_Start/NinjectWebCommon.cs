using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using TaskManagement.Repository;
using TaskManagement.Repository.Interfaces;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TaskManagement.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TaskManagement.App_Start.NinjectWebCommon), "Stop")]

namespace TaskManagement.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            // 🔧 This binds the interface to the implementation
            kernel.Bind<ITaskRepository>().To<TaskRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IDeviceRepository>().To<DeviceRepository>();
            kernel.Bind<ITaskAssignmentRepository>().To<TaskAssignmentRepository>();
        }
    }
}
