using Services.Common.Dto;
using Services.Common.Dto.Billing;

namespace Services.InvoiceService
{
    public interface IInvoiceService
    {
        Task<GeneralResponseDto> Create(InvoiceCreateDto invoiceDto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int invoiceId, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int invoiceId, InvoiceUpdateDto invoiceDto, CancellationToken cancellationToken = default);
        Task<ObservableCollection<InvoiceDto>> GetAll(CancellationToken cancellationToken = default);
        Task<InvoiceDto> GetById(int invoiceId, CancellationToken cancellationToken = default);
    }
}
