﻿namespace Services.Common.Dto
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int EmployeeId { get; set; }
        public int TreatmentId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        
        public string Status { get; set; } = string.Empty;
    }


    public class AppointmentCreateDto
    {
        public int AppointmentId { get; set; }
        public int EmployeeId { get; set; }

        public int PatientId { get; set; }
        public int TreatmentId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class AppointmentUpdateDto
    {
        public int AppointmentId { get; set; }
        public int EmployeeId { get; set; }
        public int TreatmentId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
