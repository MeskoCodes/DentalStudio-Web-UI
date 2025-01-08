using Contract;

namespace Services
{
    public interface IAppointmentService
    {
        Task<GeneralResponseDto> Create(AppointmentCreateDto appointmentDto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int appointmentId, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int appointmentId, AppointmentUpdateDto appointmentDto, CancellationToken cancellationToken = default);
        Task<ObservableCollection<AppointmentDto>> GetAll(CancellationToken cancellationToken = default);
        Task<AppointmentDto> GetById(int appointmentId, CancellationToken cancellationToken = default);
    }
}
