-- Pharmacy Management System Database Setup
-- Create Database
CREATE DATABASE PharmacyDB;
GO

USE PharmacyDB;
GO

-- Create Medicines Table
CREATE TABLE Medicines (
    MedicineID INT IDENTITY (1, 1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Quantity INT NOT NULL
);

-- Create Sales Table
CREATE TABLE Sales (
    SaleID INT IDENTITY (1, 1) PRIMARY KEY,
    MedicineID INT NOT NULL,
    QuantitySold INT NOT NULL,
    SaleDate DATETIME NOT NULL DEFAULT GETDATE (),
    FOREIGN KEY (MedicineID) REFERENCES Medicines (MedicineID)
);

-- Stored Procedure: Add Medicine
CREATE PROCEDURE AddMedicine
    @Name VARCHAR(100),
    @Category VARCHAR(50),
    @Price DECIMAL(10,2),
    @Quantity INT
AS
BEGIN
    INSERT INTO Medicines (Name, Category, Price, Quantity)
    VALUES (@Name, @Category, @Price, @Quantity)
    
    SELECT SCOPE_IDENTITY() AS NewMedicineID
END
GO

-- Stored Procedure: Search Medicine
CREATE PROCEDURE SearchMedicine
    @SearchTerm VARCHAR(100)
AS
BEGIN
    SELECT MedicineID, Name, Category, Price, Quantity
    FROM Medicines
    WHERE Name LIKE '%' + @SearchTerm + '%' OR Category LIKE '%' + @SearchTerm + '%'
    ORDER BY Name
END
GO

-- Stored Procedure: Update Stock
CREATE PROCEDURE UpdateStock
    @MedicineID INT,
    @Quantity INT
AS
BEGIN
    UPDATE Medicines
    SET Quantity = @Quantity
    WHERE MedicineID = @MedicineID
    
    SELECT @@ROWCOUNT AS RowsAffected
END
GO

-- Stored Procedure: Record Sale
CREATE PROCEDURE RecordSale
    @MedicineID INT,
    @QuantitySold INT
AS
BEGIN
    DECLARE @CurrentStock INT
    
    -- Check current stock
    SELECT @CurrentStock = Quantity FROM Medicines WHERE MedicineID = @MedicineID
    
    IF @CurrentStock >= @QuantitySold
    BEGIN
        -- Record the sale
        INSERT INTO Sales (MedicineID, QuantitySold, SaleDate)
        VALUES (@MedicineID, @QuantitySold, GETDATE())
        
        -- Update stock
        UPDATE Medicines
        SET Quantity = Quantity - @QuantitySold
        WHERE MedicineID = @MedicineID
        
        SELECT 1 AS Success, 'Sale recorded successfully' AS Message
    END
    ELSE
    BEGIN
        SELECT 0 AS Success, 'Insufficient stock' AS Message
    END
END
GO

-- Stored Procedure: Get All Medicines
CREATE PROCEDURE GetAllMedicines
AS
BEGIN
    SELECT MedicineID, Name, Category, Price, Quantity
    FROM Medicines
    ORDER BY Name
END
GO

-- Insert Sample Data
INSERT INTO
    Medicines (
        Name,
        Category,
        Price,
        Quantity
    )
VALUES (
        'Paracetamol 500mg',
        'Analgesic',
        2.50,
        100
    ),
    (
        'Ibuprofen 400mg',
        'Anti-inflammatory',
        3.25,
        75
    ),
    (
        'Amoxicillin 250mg',
        'Antibiotic',
        8.90,
        50
    ),
    (
        'Vitamin C 1000mg',
        'Vitamin',
        5.60,
        200
    ),
    (
        'Aspirin 75mg',
        'Analgesic',
        1.80,
        150
    ),
    (
        'Cetirizine 10mg',
        'Antihistamine',
        4.20,
        80
    ),
    (
        'Omeprazole 20mg',
        'Antacid',
        6.75,
        60
    ),
    (
        'Metformin 500mg',
        'Diabetic',
        7.40,
        90
    ),
    (
        'Salbutamol Inhaler',
        'Respiratory',
        12.50,
        30
    ),
    (
        'Multivitamin',
        'Vitamin',
        9.99,
        120
    );

-- Insert Sample Sales
INSERT INTO
    Sales (
        MedicineID,
        QuantitySold,
        SaleDate
    )
VALUES (1, 5, '2025-08-20 10:30:00'),
    (2, 3, '2025-08-20 14:15:00'),
    (4, 10, '2025-08-21 09:45:00'),
    (5, 7, '2025-08-21 16:20:00'),
    (1, 2, '2025-08-22 11:10:00');

GO