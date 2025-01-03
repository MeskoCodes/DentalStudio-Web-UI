using Contract.Account;

namespace Services
{
    public class PatientService : IPatientService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public PatientService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<GeneralResponseDto> Create(PatientCreateDto patientDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Post($"{ApiEndpoints.PatientController}/create", patientDto);
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

        public async Task<GeneralResponseDto> Delete(int patientId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Delete($"{ApiEndpoints.PatientController}/{patientId}");
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

        public async Task<GeneralResponseDto> Update(int patientId, PatientUpdateDto patientDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Put($"{ApiEndpoints.PatientController}/update/{patientId}", patientDto);
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

        public async Task<ObservableCollection<PatientDto>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Get($"{ApiEndpoints.PatientController}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<ObservableCollection<PatientDto>>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<PatientDto> GetById(int patientId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Get($"{ApiEndpoints.PatientController}/details/{patientId}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<PatientDto>(responseStream, _options, cancellationToken);
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
