using Contract.Billing;

namespace Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public InvoiceService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<GeneralResponseDto> Create(InvoiceCreateDto invoiceDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Post($"{ApiEndpoints.InvoiceController}/create", invoiceDto);
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<GeneralResponseDto> Delete(int invoiceId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Delete($"{ApiEndpoints.InvoiceController}/{invoiceId}");
                return response.IsSuccessStatusCode
                    ? new GeneralResponseDto { IsSuccess = true }
                    : new GeneralResponseDto { IsSuccess = false };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new GeneralResponseDto { IsSuccess = false };
            }
        }

        public async Task<GeneralResponseDto> Update(int invoiceId, InvoiceUpdateDto invoiceDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Put($"{ApiEndpoints.InvoiceController}/update/{invoiceId}", invoiceDto);
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ObservableCollection<InvoiceDto>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Get($"{ApiEndpoints.InvoiceController}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<ObservableCollection<InvoiceDto>>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<InvoiceDto> GetById(int invoiceId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Get($"{ApiEndpoints.InvoiceController}/details/{invoiceId}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<InvoiceDto>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }
    }
}
