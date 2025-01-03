namespace ViewModels
{
    public class PatientFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public PatientCreateDto? PatientCreate { get; set; }

        [Parameter]
        public PatientUpdateDto? PatientUpdate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (PatientUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (PatientCreate != null && PatientCreate.Id == 0)
                {
                    response = await PatientService!.Create(PatientCreate);
                }
                else
                {
                    response = await PatientService!.Update(PatientUpdate!.Id, PatientUpdate);
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
            (PatientCreate != null && string.IsNullOrWhiteSpace(PatientCreate.FirstName)) ||
            (PatientUpdate != null && string.IsNullOrWhiteSpace(PatientUpdate.FirstName));
    }
}
