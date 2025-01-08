using Contract.Billing;

namespace ViewModels
{
    public class InvoiceFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public InvoiceCreateDto? InvoiceCreate { get; set; }

        [Parameter]
        public InvoiceUpdateDto? InvoiceUpdate { get; set; }

        [Inject] private IInvoiceService InvoiceService { get; set; } // Updated to reference the correct service

        protected override async Task OnInitializedAsync()
        {
            if (InvoiceUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (InvoiceCreate != null && InvoiceCreate.InvoiceId == 0)
                {
                    response = await InvoiceService.CreateInvoice(InvoiceCreate); // Updated method call
                }
                else
                {
                    response = await InvoiceService.UpdateInvoice(InvoiceUpdate!.InvoiceId, InvoiceUpdate); // Updated method call
                }

                HandleResponse(response);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                MudDialog!.Close(DialogResult.Ok(false)); // Adjusted to handle failure scenario properly
            }
        }

        private void HandleResponse(GeneralResponseDto response)
        {
            var isSuccess = response?.IsSuccess == true;
            Snackbar!.Add(isSuccess ? "Success!" : "Error!", isSuccess ? Severity.Success : Severity.Error);
            MudDialog!.Close(DialogResult.Ok(isSuccess)); // Close based on response success
        }

        public void Cancel() => MudDialog!.Cancel();

        public bool Disabled =>
            (InvoiceCreate != null && string.IsNullOrWhiteSpace(InvoiceCreate.InvoiceNumber)) ||
            (InvoiceUpdate != null && string.IsNullOrWhiteSpace(InvoiceUpdate.InvoiceNumber));
    }
}
