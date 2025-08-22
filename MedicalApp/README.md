# Medical Appointment Booking System

## Overview

A Windows Forms application that allows patients to book appointments with doctors, view available doctors, and manage existing appointments using SQL Server database connectivity through ADO.NET.

## Database Setup

1. Execute the `DatabaseSetup.sql` script in SQL Server Management Studio to create the MedicalDB database with sample data.
2. Update the connection string in `App.config` if needed to match your SQL Server instance.

## Features

- **Main Form**: Navigation hub with buttons to access all features
- **Doctor List**: View and search available doctors by name or specialty
- **Book Appointment**: Schedule new appointments with validation
- **Manage Appointments**: View, update, and delete existing appointments

## Database Schema

- **Doctors**: DoctorID, FullName, Specialty, Availability
- **Patients**: PatientID, FullName, Email
- **Appointments**: AppointmentID, DoctorID, PatientID, AppointmentDate, Notes

## Technologies Used

- Windows Forms (.NET Framework 4.8)
- ADO.NET (SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter, DataSet)
- SQL Server with parameterized queries
- Event-driven programming with delegates

## Key ADO.NET Features Implemented

- **SqlConnection** for database connectivity
- **SqlCommand** with CommandType.Text for queries
- **SqlDataReader** for efficient data retrieval
- **SqlDataAdapter and DataSet** for data manipulation
- **Parameterized queries** to prevent SQL injection
- **Exception handling** with try-catch blocks
- **Connection management** with using statements

## Instructions

1. Run the database setup script first
2. Build and run the application
3. Use the main form to navigate between features
4. Book appointments, view doctors, and manage existing appointments
