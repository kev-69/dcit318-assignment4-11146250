-- Medical Appointment Booking System Database Setup
-- Create Database
CREATE DATABASE MedicalDB;
GO

USE MedicalDB;
GO

-- Create Doctors Table
CREATE TABLE Doctors (
    DoctorID INT IDENTITY (1, 1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Specialty VARCHAR(100) NOT NULL,
    Availability BIT NOT NULL DEFAULT 1
);

-- Create Patients Table
CREATE TABLE Patients (
    PatientID INT IDENTITY (1, 1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL
);

-- Create Appointments Table
CREATE TABLE Appointments (
    AppointmentID INT IDENTITY (1, 1) PRIMARY KEY,
    DoctorID INT NOT NULL,
    PatientID INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Notes VARCHAR(500),
    FOREIGN KEY (DoctorID) REFERENCES Doctors (DoctorID),
    FOREIGN KEY (PatientID) REFERENCES Patients (PatientID)
);

-- Insert Sample Data for Doctors
INSERT INTO
    Doctors (
        FullName,
        Specialty,
        Availability
    )
VALUES (
        'Dr. John Smith',
        'Cardiology',
        1
    ),
    (
        'Dr. Sarah Johnson',
        'Dermatology',
        1
    ),
    (
        'Dr. Michael Brown',
        'Pediatrics',
        1
    ),
    (
        'Dr. Emily Davis',
        'Orthopedics',
        1
    ),
    (
        'Dr. David Wilson',
        'Neurology',
        0
    ),
    (
        'Dr. Lisa Anderson',
        'Oncology',
        1
    ),
    (
        'Dr. Robert Taylor',
        'Internal Medicine',
        1
    ),
    (
        'Dr. Jennifer Martinez',
        'Gynecology',
        1
    );

-- Insert Sample Data for Patients
INSERT INTO
    Patients (FullName, Email)
VALUES (
        'Alice Cooper',
        'alice.cooper@email.com'
    ),
    (
        'Bob Johnson',
        'bob.johnson@email.com'
    ),
    (
        'Carol Smith',
        'carol.smith@email.com'
    ),
    (
        'Daniel Brown',
        'daniel.brown@email.com'
    ),
    (
        'Eva Martinez',
        'eva.martinez@email.com'
    ),
    (
        'Frank Wilson',
        'frank.wilson@email.com'
    ),
    (
        'Grace Taylor',
        'grace.taylor@email.com'
    ),
    (
        'Henry Davis',
        'henry.davis@email.com'
    );

-- Insert Sample Appointments
INSERT INTO
    Appointments (
        DoctorID,
        PatientID,
        AppointmentDate,
        Notes
    )
VALUES (
        1,
        1,
        '2025-08-25 09:00:00',
        'Regular checkup'
    ),
    (
        2,
        2,
        '2025-08-26 10:30:00',
        'Skin consultation'
    ),
    (
        3,
        3,
        '2025-08-27 14:00:00',
        'Child vaccination'
    ),
    (
        4,
        4,
        '2025-08-28 11:15:00',
        'Joint pain examination'
    );

GO