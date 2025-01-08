namespace ViewModels
{
    public class EmployeeFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public EmployeeCreateDto? EmployeeCreate { get; set; }

        [Parameter]
        public EmployeeUpdateDto? EmployeeUpdate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (EmployeeUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (EmployeeCreate != null && EmployeeCreate.EmployeeId == 0)
                {
                    response = await EmployeeService!.Create(EmployeeCreate);
                }
                else
                {
                    response = await EmployeeService!.Update(EmployeeUpdate!.EmployeeId, EmployeeUpdate);
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
            (EmployeeCreate != null && string.IsNullOrWhiteSpace(EmployeeCreate.FirstName)) ||
            (EmployeeUpdate != null && string.IsNullOrWhiteSpace(EmployeeUpdate.FirstName));
    }
}
