using Contract.Billing;

namespace Services
{
    public interface IPaymentService
    {
        Task<GeneralResponseDto> Create(PaymentCreateDto paymentDto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int paymentId, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int paymentId, PaymentUpdateDto paymentDto, CancellationToken cancellationToken = default);
        Task<ObservableCollection<PaymentDto>> GetAll(CancellationToken cancellationToken = default);
        Task<PaymentDto> GetById(int paymentId, CancellationToken cancellationToken = default);
    }
}
