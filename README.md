# DCIT 318 Assignment 4 - Windows Forms Database Applications

## Overview

This repository contains two complete Windows Forms applications developed for DCIT 318 Assignment 4, demonstrating advanced database connectivity using ADO.NET with SQL Server.

## Projects

### 1. Medical Appointment Booking System (`MedicalApp/`)

A comprehensive medical appointment management system that allows patients to book appointments with doctors, view available doctors, and manage existing appointments.

**Key Features:**

- **Main Form**: Navigation hub with access to all features
- **Doctor List**: View and search available doctors by name or specialty
- **Book Appointment**: Schedule new appointments with validation and conflict checking
- **Manage Appointments**: View, update, and delete existing appointments with full CRUD operations

**Database:** MedicalDB with tables for Doctors, Patients, and Appointments
**ADO.NET Features:** SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter, DataSet

### 2. Pharmacy Management System (`PharmacyApp/`)

A complete pharmacy inventory management system using stored procedures for all database operations.

**Key Features:**

- **Add Medicine**: Add new medicines to inventory with validation
- **Search Medicines**: Search by name or category using stored procedures
- **Update Stock**: Modify stock quantities with real-time updates
- **Record Sales**: Process medicine sales with automatic stock adjustment
- **Inventory Management**: Complete medicine inventory display and management

**Database:** PharmacyDB with tables for Medicines and Sales
**ADO.NET Features:** Stored Procedures, SqlCommand with CommandType.StoredProcedure, ExecuteNonQuery, ExecuteReader, ExecuteScalar

## Technologies Used

- **Framework**: .NET Framework 4.8
- **UI**: Windows Forms
- **Database**: Microsoft SQL Server
- **Data Access**: ADO.NET
- **Language**: C#

## ADO.NET Features Demonstrated

### Medical App

- SqlConnection for database connectivity
- SqlCommand with CommandType.Text for SQL queries
- SqlDataReader for efficient data retrieval
- SqlDataAdapter and DataSet for data manipulation
- Parameterized queries for security
- Exception handling with try-catch blocks
- Connection management with using statements

### Pharmacy App

- Stored Procedures for all database operations
- CommandType.StoredProcedure implementation
- ExecuteNonQuery() for Insert/Update operations
- ExecuteReader() for data retrieval
- ExecuteScalar() for single value returns
- Parameter direction management
- Advanced error handling and validation

## Setup Instructions

### Prerequisites

- Visual Studio 2019/2022
- SQL Server (LocalDB, Express, or full version)
- .NET Framework 4.8

### Database Setup

1. **Medical App**: Execute `MedicalApp/DatabaseSetup.sql` in SQL Server Management Studio
2. **Pharmacy App**: Execute `PharmacyApp/DatabaseSetup.sql` in SQL Server Management Studio

### Configuration

Update connection strings in respective `App.config` files if needed:

- Default: `Data Source=(local);Initial Catalog=[DatabaseName];Integrated Security=True`

### Running the Applications

1. Open `MedicalApp/MedicalApp.sln` or `PharmacyApp/PharmacyApp.sln` in Visual Studio
2. Build and run the applications
3. Follow the intuitive user interfaces to explore all features

## Architecture & Design Patterns

### Event-Driven Programming

- EventHandler delegates for form control events
- Click, SelectedIndexChanged, TextChanged events
- Proper event handling with validation

### Data Layer Separation

- Connection string management in App.config
- Parameterized queries for security
- Proper exception handling and resource disposal

### User Interface Design

- Intuitive Windows Forms layouts
- GroupBox organization for logical feature grouping
- DataGridView for data display with formatting
- Input validation and user-friendly error messages

## Security Features

- **SQL Injection Prevention**: All queries use parameters
- **Input Validation**: Comprehensive validation for all user inputs
- **Error Handling**: Graceful error handling with user-friendly messages
- **Connection Security**: Integrated Windows Authentication by default

## Assignment Compliance

### Question 1 (Medical App) ✅

- [x] Complete database design with proper relationships
- [x] Windows Forms with MainForm, DoctorListForm, AppointmentForm, ManageAppointmentsForm
- [x] All required controls: TextBox, ComboBox, Button, DateTimePicker, DataGridView
- [x] Event-driven programming with proper delegates
- [x] SqlConnection with connection string in App.config
- [x] SqlCommand and ExecuteReader for data retrieval
- [x] Parameterized queries for booking appointments
- [x] SqlDataAdapter and DataSet for appointment management
- [x] Complete CRUD operations with exception handling

### Question 2 (Pharmacy App) ✅

- [x] PharmacyDB database with Medicines and Sales tables
- [x] All required stored procedures implemented
- [x] Windows Forms with organized UI using GroupBox controls
- [x] Event handlers for all buttons using delegates
- [x] SqlCommand with CommandType.StoredProcedure for all operations
- [x] ExecuteNonQuery(), ExecuteReader(), ExecuteScalar() implementations
- [x] SqlDataReader for loading medicine data into DataGridView
- [x] Comprehensive error handling with MessageBox displays

## Screenshots

_(Screenshots would be included here showing all working features as requested in the assignment)_

## Author

Student ID: 11146250
Course: DCIT 318 - Database Programming
Assignment: Assignment 4 - Windows Forms Database Applications
