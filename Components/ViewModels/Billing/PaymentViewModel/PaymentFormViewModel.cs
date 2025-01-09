using Services.Common.Dto;
using Services.Common.Dto.Billing;

namespace ViewModels
{
    public class PaymentFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public PaymentCreateDto? PaymentCreate { get; set; }

        [Parameter]
        public PaymentUpdateDto? PaymentUpdate { get; set; }

        public PaymentDto PaymentDto { get; set; } = new PaymentDto();

        public PaymentDto? SelectedPaymentMethod { get; set; }
        public InvoiceDto? SelectedInvoice { get; set; }

        public IEnumerable<PaymentDto> PaymentMethods { get; set; } = new List<PaymentDto>();
        public IEnumerable<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();

        protected override async Task OnInitializedAsync()
        {
            if (PaymentUpdate != null)
            {
                PaymentDto = new PaymentDto
                {
                    PaymentId = PaymentUpdate.PaymentId,
                    PaymentDate = PaymentUpdate.PaymentDate,
                    Amount = PaymentUpdate.Amount,
                    PaymentMethod = PaymentUpdate.PaymentMethod,
                    InvoiceId = PaymentUpdate.InvoiceId
                };

                SelectedPaymentMethod = PaymentMethods.FirstOrDefault(m => m.PaymentMethod == PaymentUpdate.PaymentMethod);
                SelectedInvoice = Invoices.FirstOrDefault(i => i.InvoiceId == PaymentUpdate.InvoiceId);

            }
            else if (PaymentCreate != null)
            {
                PaymentDto = new PaymentDto
                {
                    PaymentDate = PaymentCreate.PaymentDate,
                    Amount = PaymentCreate.Amount,
                    PaymentMethod = PaymentCreate.PaymentMethod,
                    InvoiceId = PaymentCreate.InvoiceId
                };
            }

            StateHasChanged();
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                if (PaymentDto.PaymentId == 0)
                {
                    var createDto = new PaymentCreateDto
                    {
                        PaymentDate = PaymentDto.PaymentDate,
                        Amount = PaymentDto.Amount,
                        PaymentMethod = SelectedPaymentMethod?.PaymentMethod ?? string.Empty,  // Koristi PaymentMethod kao string
                        InvoiceId = SelectedInvoice?.InvoiceId ?? 0
                    };

                    var response = await PaymentService!.Create(createDto);
                    HandleResponse(response);
                }
                else
                {
                    var updateDto = new PaymentUpdateDto
                    {
                        PaymentId = PaymentDto.PaymentId,
                        PaymentDate = PaymentDto.PaymentDate,
                        Amount = PaymentDto.Amount,
                        PaymentMethod = SelectedPaymentMethod?.PaymentMethod ?? string.Empty,  // Koristi PaymentMethod kao string
                        InvoiceId = SelectedInvoice?.InvoiceId ?? 0
                    };

                    var response = await PaymentService!.Update(updateDto.PaymentId, updateDto);
                    HandleResponse(response);
                }
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
            PaymentDto.PaymentDate == default ||
            PaymentDto.Amount <= 0 ||
            SelectedPaymentMethod == null ||
            SelectedInvoice == null;
    }
}
