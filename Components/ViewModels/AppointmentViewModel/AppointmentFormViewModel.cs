using Services.Common.Dto;

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

        protected override async Task OnInitializedAsync()
        {
            if (AppointmentUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (AppointmentCreate != null && AppointmentCreate.AppointmentId == 0)
                {
                    response = await AppointmentService!.Create(AppointmentCreate);
                }
                else
                {
                    response = await AppointmentService!.Update(AppointmentUpdate!.AppointmentId, AppointmentUpdate);
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
            (AppointmentCreate != null && string.IsNullOrWhiteSpace(AppointmentCreate.Status)) ||
            (AppointmentUpdate != null && string.IsNullOrWhiteSpace(AppointmentUpdate.Status));
    }
}
