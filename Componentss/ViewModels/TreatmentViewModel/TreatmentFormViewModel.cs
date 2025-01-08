namespace ViewModels
{
    public class TreatmentFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public TreatmentCreateDto? TreatmentCreate { get; set; }

        [Parameter]
        public TreatmentUpdateDto? TreatmentUpdate { get; set; }

        // Zajednički DTO za bindovanje
        public TreatmentDto TreatmentDto { get; set; } = new TreatmentDto();

        protected override async Task OnInitializedAsync()
        {
            if (TreatmentUpdate != null)
            {
                // Prebacivanje podataka iz TreatmentUpdate u TreatmentDto
                TreatmentDto = new TreatmentDto
                {
                    TreatmentId = TreatmentUpdate.TreatmentId,
                    Name = TreatmentUpdate.Name,
                    Description = TreatmentUpdate.Description,
                    Price = TreatmentUpdate.Price,
                    Duration = TreatmentUpdate.Duration
                };
            }

            if (TreatmentCreate != null)
            {
                // Inicijalizacija TreatmentDto za kreiranje novog tretmana
                TreatmentDto = new TreatmentDto
                {
                    TreatmentId = TreatmentCreate.TreatmentId,
                    Name = TreatmentCreate.Name,
                    Description = TreatmentCreate.Description,
                    Price = TreatmentCreate.Price,
                    Duration = TreatmentCreate.Duration
                };
            }

            StateHasChanged();
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (TreatmentDto.TreatmentId == 0)
                {
                    // Kreiranje novog tretmana
                    var createDto = new TreatmentCreateDto
                    {
                        TreatmentId = TreatmentDto.TreatmentId,
                        Name = TreatmentDto.Name,
                        Description = TreatmentDto.Description,
                        Price = TreatmentDto.Price,
                        Duration = TreatmentDto.Duration
                    };
                    response = await TreatmentService!.Create(createDto);
                }
                else
                {
                    // Ažuriranje postojećeg tretmana
                    var updateDto = new TreatmentUpdateDto
                    {
                        TreatmentId = TreatmentDto.TreatmentId,
                        Name = TreatmentDto.Name,
                        Description = TreatmentDto.Description,
                        Price = TreatmentDto.Price,
                        Duration = TreatmentDto.Duration
                    };
                    response = await TreatmentService!.Update(updateDto.TreatmentId, updateDto);
                }

                HandleResponse(response);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                MudDialog!.Close(DialogResult.Ok(true));
            }
        }

        private void HandleResponse(GeneralResponseDto response)
        {
            var isSuccess = response?.IsSuccess == true;
            Snackbar!.Add(isSuccess ? "Success!" : "Error!", isSuccess ? Severity.Success : Severity.Error);
            MudDialog!.Close(DialogResult.Ok(true));
        }

        public void Cancel() => MudDialog!.Cancel();

        public bool Disabled =>
            string.IsNullOrWhiteSpace(TreatmentDto.Name) ||
            string.IsNullOrWhiteSpace(TreatmentDto.Description) ||
            TreatmentDto.Price <= 0 || TreatmentDto.Duration <= 0;
    }
}
