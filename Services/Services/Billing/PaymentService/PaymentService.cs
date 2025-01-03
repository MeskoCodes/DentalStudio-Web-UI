using Contract.Billing;

namespace Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public PaymentService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<GeneralResponseDto> Create(PaymentCreateDto paymentDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Post($"{ApiEndpoints.PaymentController}/create", paymentDto);
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

        public async Task<GeneralResponseDto> Delete(int paymentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Delete($"{ApiEndpoints.PaymentController}/{paymentId}");
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

        public async Task<GeneralResponseDto> Update(int paymentId, PaymentUpdateDto paymentDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Put($"{ApiEndpoints.PaymentController}/update/{paymentId}", paymentDto);
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

        public async Task<ObservableCollection<PaymentDto>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Get($"{ApiEndpoints.PaymentController}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<ObservableCollection<PaymentDto>>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<PaymentDto> GetById(int paymentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Get($"{ApiEndpoints.PaymentController}/details/{paymentId}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<PaymentDto>(responseStream, _options, cancellationToken);
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
