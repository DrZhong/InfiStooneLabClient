namespace Guoxu.LabManager.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly LabManagerDbContext _context;

        public InitialHostDbBuilder(LabManagerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            new DefaultDictCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
