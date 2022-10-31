using Diploma.Models;

namespace Diploma.Repository {
    public interface IModalTypeRepository {
        public Task<ModalType?> GetModalTypeById(int id);
    }
}
