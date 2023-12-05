using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Editions;
using Prosperium.Management.Editions;

namespace Prosperium.Management.EntityFrameworkCore.Seed.Host
{
    public class DefaultEditionCreator
    {
        private readonly ManagementDbContext _context;

        public DefaultEditionCreator(ManagementDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            var editionNames = EditionManager.DefaultEditionName.Split(',');

            foreach (var editionName in editionNames)
            {
                var edition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == editionName.Trim());

                if (edition == null)
                {
                    edition = new Edition { Name = editionName.Trim(), DisplayName = editionName.Trim() };
                    _context.Editions.Add(edition);
                    _context.SaveChanges();
                }
            }
        }
    }
}

        //private void CreateFeatureIfNotExists(int editionId, string featureName, bool isEnabled)
        //{
        //    if (_context.EditionFeatureSettings.IgnoreQueryFilters().Any(ef => ef.EditionId == editionId && ef.Name == featureName))
        //    {
        //        return;
        //    }

        //    _context.EditionFeatureSettings.Add(new EditionFeatureSetting
        //    {
        //        Name = featureName,
        //        Value = isEnabled.ToString(),
        //        EditionId = editionId
        //    });
        //    _context.SaveChanges();
        //}