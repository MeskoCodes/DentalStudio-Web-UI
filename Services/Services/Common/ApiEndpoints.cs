namespace Services
{
    public static class ApiEndpoints
    {
        public const string BaseUrl = "https://localhost:5000/api";

        public const string AccountController = $"{BaseUrl}/accounts";
        public const string AppointmentController = $"{BaseUrl}/appointments";
        public const string EmployeeController = $"{BaseUrl}/employees";
        public const string PatientController = $"{BaseUrl}/patients";
        public const string TreatmentController = $"{BaseUrl}/treatments";
        public const string InvoiceController = $"{BaseUrl}/invoices";
        public const string PaymentController = $"{BaseUrl}/payments";
    }
}
