
namespace ViewModels
{
    public class AppointmentFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public AppointmentCreateDto? AppointmentCreate { get; set; }

        [Parameter]
        public AppointmentUpdateDto? AppointmentUpdate { get; set; }

        public IEnumerable<TreatmentDto> Treatments { get; set; } = [];

        public TreatmentDto? SelectedTreatment { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Treatments = await TreatmentService!.GetAll();
            if (AppointmentUpdate != null)
            {
                var treatment = Treatments.FirstOrDefault(x => x.Id == AppointmentUpdate.TreatmentId);
                SelectedTreatment = treatment;
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (AppointmentCreate != null && AppointmentCreate.Id == 0)
                {
                    AppointmentCreate.TreatmentId = SelectedTreatment.Id;
                    response = await AppointmentService!.Create(AppointmentCreate);
                }
                else
                {
                    AppointmentUpdate!.TreatmentId = SelectedTreatment.Id;
                    response = await AppointmentService!.Update(AppointmentUpdate!.Id!, AppointmentUpdate);
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
            (
                (AppointmentCreate != null && (string.IsNullOrWhiteSpace(AppointmentCreate.Title))) ||
                (AppointmentUpdate != null && (string.IsNullOrWhiteSpace(AppointmentUpdate.Title)))
            );
    }
}
