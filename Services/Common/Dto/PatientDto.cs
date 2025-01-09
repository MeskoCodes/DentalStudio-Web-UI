﻿namespace Services.Common.Dto
{
    public class PatientDto
    {
        public int PatientId { get; set; } // ID pacijenta
        public string FirstName { get; set; } = string.Empty; // Ime
        public string? LastName { get; set; } // Prezime
        public DateTime DateOfBirth { get; set; } // Datum rođenja
        public string? MobileNumber { get; set; } // Broj telefona
        public string? Email { get; set; } // Email
        public DateTime RegistrationDate { get; set; } // Datum registracije
    }

    public class PatientCreateDto
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; } = string.Empty; // Ime
        public string? LastName { get; set; } // Prezime
        public DateTime DateOfBirth { get; set; } // Datum rođenja
        public string? MobileNumber { get; set; } // Broj telefona
        public string? Email { get; set; } // Email
    }

    public class PatientUpdateDto
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
    }
}

