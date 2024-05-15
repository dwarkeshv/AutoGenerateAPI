using AutoGenerateAPI.Database.Models;
using AutoGenerateAPI.Interface;
namespace AutoGenerateAPI.Repository
{
    public class GETRepository : IGET
    {
        public readonly TryAutomationContext _context;
        public IConfiguration _configuration;
        public GETRepository(TryAutomationContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
    }
}
