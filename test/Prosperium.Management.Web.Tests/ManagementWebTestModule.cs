using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Prosperium.Management.EntityFrameworkCore;
using Prosperium.Management.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Prosperium.Management.Web.Tests
{
    [DependsOn(
        typeof(ManagementWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class ManagementWebTestModule : AbpModule
    {
        public ManagementWebTestModule(ManagementEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ManagementWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(ManagementWebMvcModule).Assembly);
        }
    }
}