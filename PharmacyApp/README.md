# Pharmacy Management System

## Overview

A Windows Forms application that allows pharmacies to manage their inventory and record sales. The application supports adding new medicines, searching for medicines by name or category, updating stock levels, and recording purchases using stored procedures in SQL Server.

## Database Setup

1. Execute the `DatabaseSetup.sql` script in SQL Server Management Studio to create the PharmacyDB database with sample data and stored procedures.
2. Update the connection string in `App.config` if needed to match your SQL Server instance.

## Features

- **Add New Medicine**: Add medicines with name, category, price, and quantity
- **Search Medicines**: Search by name or category using stored procedures
- **Update Stock**: Modify stock quantities for existing medicines
- **Record Sales**: Process medicine sales with automatic stock updates
- **View All Medicines**: Display complete medicine inventory

## Database Schema

- **Medicines**: MedicineID, Name, Category, Price, Quantity
- **Sales**: SaleID, MedicineID, QuantitySold, SaleDate

## Stored Procedures

- **AddMedicine**: Adds new medicine to inventory
- **SearchMedicine**: Searches medicines by name or category
- **UpdateStock**: Updates medicine stock quantity
- **RecordSale**: Records sale and updates stock automatically
- **GetAllMedicines**: Retrieves all medicines

## Technologies Used

- Windows Forms (.NET Framework 4.8)
- ADO.NET with Stored Procedures
- SqlConnection, SqlCommand with CommandType.StoredProcedure
- SqlDataReader for data retrieval
- SQL Server with parameterized stored procedures
- Input validation and error handling

## Key ADO.NET Features Implemented

- **Stored Procedures** exclusively for all database operations
- **SqlCommand** with CommandType.StoredProcedure
- **ExecuteNonQuery()** for Add/Update/Sale operations
- **ExecuteReader()** for search and view operations
- **ExecuteScalar()** for returning single values
- **Parameterized queries** to prevent SQL injection
- **Exception handling** with user-friendly messages
- **Connection management** with using statements

## Instructions

1. Run the database setup script to create PharmacyDB with stored procedures
2. Build and run the application
3. Add medicines using the form fields
4. Search for medicines by name or category
5. Select medicines from the grid to update stock or record sales
6. Use the intuitive interface to manage pharmacy inventory efficiently

## Validation Features

- Input validation for all fields
- Stock availability checking before sales
- Price and quantity format validation
- User-friendly error messages
- Automatic stock updates after sales
