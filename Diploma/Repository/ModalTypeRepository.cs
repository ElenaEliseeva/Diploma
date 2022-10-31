using Diploma.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Repository {
    public class ModalTypeRepository : IModalTypeRepository {

        private readonly DiplomDbContext _dbContext;

        public ModalTypeRepository(DiplomDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<ModalType?> GetModalTypeById(int id)
        {
            var modalType = await _dbContext.ModalTypes.FirstOrDefaultAsync(x => x.ModalTypeId == id);
            return modalType ?? null;
        }
    }
}
