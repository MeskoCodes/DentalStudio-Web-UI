namespace ViewModels
{
    public class PaymentFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public InvoiceCreateDto? InvoiceCreate { get; set; }

        [Parameter]
        public InvoiceUpdateDto? InvoiceUpdate { get; set; }

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
                if (InvoiceCreate != null && InvoiceCreate.Id == 0)
                {
                    response = await BillingService!.CreateInvoice(InvoiceCreate);
                }
                else
                {
                    response = await BillingService!.UpdateInvoice(InvoiceUpdate!.Id, InvoiceUpdate);
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
            (InvoiceCreate != null && string.IsNullOrWhiteSpace(InvoiceCreate.InvoiceNumber)) ||
            (InvoiceUpdate != null && string.IsNullOrWhiteSpace(InvoiceUpdate.InvoiceNumber));
    }
}
