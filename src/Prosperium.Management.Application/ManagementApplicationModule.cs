using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Prosperium.Management.Authorization;

namespace Prosperium.Management
{
    [DependsOn(
        typeof(ManagementCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ManagementApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ManagementAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ManagementApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
