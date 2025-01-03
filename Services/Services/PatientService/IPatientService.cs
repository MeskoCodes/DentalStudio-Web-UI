using Contract.Account;

namespace Services
{
    public interface IPatientService
    {
        Task<GeneralResponseDto> Create(PatientCreateDto patientDto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int patientId, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int patientId, PatientUpdateDto patientDto, CancellationToken cancellationToken = default);
        Task<ObservableCollection<PatientDto>> GetAll(CancellationToken cancellationToken = default);
        Task<PatientDto> GetById(int patientId, CancellationToken cancellationToken = default);
    }
}
