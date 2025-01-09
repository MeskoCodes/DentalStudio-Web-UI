using Services.Common.Dto;

namespace Services.TreatmentService
{
    public interface ITreatmentService
    {
        Task<GeneralResponseDto> Create(TreatmentCreateDto treatmentDto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int treatmentId, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int treatmentId, TreatmentUpdateDto treatmentDto, CancellationToken cancellationToken = default);
        Task<ObservableCollection<TreatmentDto>> GetAll(CancellationToken cancellationToken = default);
        Task<TreatmentDto> GetById(int treatmentId, CancellationToken cancellationToken = default);
    }
}
