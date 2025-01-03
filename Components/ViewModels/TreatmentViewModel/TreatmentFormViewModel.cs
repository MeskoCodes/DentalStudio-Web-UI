namespace ViewModels
{
    public class PaymentFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public TreatmentCreateDto? TreatmentCreate { get; set; }

        [Parameter]
        public TreatmentUpdateDto? TreatmentUpdate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (TreatmentUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (TreatmentCreate != null && TreatmentCreate.Id == 0)
                {
                    response = await TreatmentService!.Create(TreatmentCreate);
                }
                else
                {
                    response = await TreatmentService!.Update(TreatmentUpdate!.Id, TreatmentUpdate);
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
            (TreatmentCreate != null && string.IsNullOrWhiteSpace(TreatmentCreate.Name)) ||
            (TreatmentUpdate != null && string.IsNullOrWhiteSpace(TreatmentUpdate.Name));
    }
}
