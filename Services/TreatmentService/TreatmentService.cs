using Contract;

namespace Services
{
    public class TreatmentService : ITreatmentService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public TreatmentService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<GeneralResponseDto> Create(TreatmentCreateDto treatmentDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Post($"{ApiEndpoints.TreatmentController}/create", treatmentDto);
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

        public async Task<GeneralResponseDto> Delete(int treatmentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Delete($"{ApiEndpoints.TreatmentController}/{treatmentId}");
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

        public async Task<GeneralResponseDto> Update(int treatmentId, TreatmentUpdateDto treatmentDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Put($"{ApiEndpoints.TreatmentController}/update/{treatmentId}", treatmentDto);
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

        public async Task<ObservableCollection<TreatmentDto>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Get($"{ApiEndpoints.TreatmentController}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<ObservableCollection<TreatmentDto>>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<TreatmentDto> GetById(int treatmentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Get($"{ApiEndpoints.TreatmentController}/details/{treatmentId}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<TreatmentDto>(responseStream, _options, cancellationToken);
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
