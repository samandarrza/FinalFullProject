using FinalProject.DAL;

namespace FinalProject.Services
{
    public class LayoutService
    {
        private readonly EtradeDbContext _context;

        public LayoutService(EtradeDbContext context )
        {
            _context = context;
        }
        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
