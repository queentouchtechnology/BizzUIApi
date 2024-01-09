CREATE TABLE Products
(
    Id    INT PRIMARY KEY IDENTITY,
    Name  NVARCHAR(100),
    Price DECIMAL(18, 2)
);

CREATE TABLE Users
(
    Id        INT IDENTITY (1,1) NOT NULL,
    Email     NVARCHAR(100)      NOT NULL,
    Password  NVARCHAR(MAX)      NOT NULL,
    Mobile    NVARCHAR(12)       NOT NULL,
    [Role]    NVARCHAR(20)       NOT NULL,
    Active    BIT                NOT NULL DEFAULT 1,
    CreatedAt SMALLDATETIME      NOT NULL DEFAULT GETDATE(),
    UpdatedAt SMALLDATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT Pk_Id PRIMARY KEY (Id),
    CONSTRAINT Uq_Email UNIQUE (Email),
    CONSTRAINT Chk_Email_len CHECK (LEN(Email) > 5),
    CONSTRAINT Chk_Mobile_len CHECK (LEN(Mobile) > 0),
);

CREATE TABLE Catalogs
(
    Id           INT IDENTITY (1,1) NOT NULL,
    Name         NVARCHAR(100)      NOT NULL,
    Description  NVARCHAR(250)      NULL,
    Platform     NVARCHAR(50)       NOT NULL DEFAULT 'linux',
    [Type]       NVARCHAR(20)       NOT NULL DEFAULT 'service',
    Price        INT                NOT NULL DEFAULT 0,
    ValidityDays INT                NOT NULL DEFAULT 10,
    DueDays      INT                NOT NULL DEFAULT 10,
    Active       BIT                NOT NULL DEFAULT 1,
    CreatedAt    SMALLDATETIME      NOT NULL DEFAULT GETDATE(),
    UpdatedAt    SMALLDATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT Pk_Id PRIMARY KEY (Id),
    CONSTRAINT Uq_Name UNIQUE (Name),
    CONSTRAINT Chk_Name_len CHECK (LEN(Name) > 0),
    CONSTRAINT Chk_Platform CHECK ([Type] IN ('windows', 'linux')),
    CONSTRAINT Chk_Type CHECK ([Type] IN ('service', 'product'))
);

CREATE TABLE Customers
(
    Id               INT IDENTITY (1,1) NOT NULL,
    Title            varchar(10)        NOT NULL,
    Email            varchar(255)       NOT NULL,
    Mobile           varchar(50)        NOT NULL,
    Address1         varchar(255)       NOT NULL,
    Address2         varchar(255)       NOT NULL,
    City             varchar(100)       NOT NULL,
    State            varchar(100)       NOT NULL,
    Country          varchar(100)       NOT NULL,
    Pincode          varchar(10)        NOT NULL,
    Location         varchar(100)       NOT NULL,
    AadharNo         varchar(12)        NOT NULL,
    CompanyName      varchar(255)       NOT NULL,
    GstNo            varchar(16)        NOT NULL,
    PanNo            varchar(10)        NOT NULL,
    RegistrationType varchar(20)        NOT NULL,
    FirstName        varchar(255)       NOT NULL,
    LastName         varchar(255)       NOT NULL,
    Active           BIT                NOT NULL DEFAULT 1,
    CreatedAt        SMALLDATETIME      NOT NULL DEFAULT GETDATE(),
    UpdatedAt        SMALLDATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT Pk_Id PRIMARY KEY (Id),
);

CREATE TABLE Invoices
(
    Id          INT IDENTITY (1,1) NOT NULL,
    CatalogId   INT                NOT NULL,
    CustomerId  int                NOT NULL,
    StartDate   DATE               NOT NULL,
    EndDate     DATE               NOT NULL,
    DueDate     DATE               NOT NULL,
    [Domain]    NVARCHAR(100)      NOT NULL,
    Status      NVARCHAR(10)       NOT NULL,
    PaymentType NVARCHAR(50)       NULL,
    CreatedAt   SMALLDATETIME      NOT NULL DEFAULT GETDATE(),
    UpdatedAt   SMALLDATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT Pk_Id PRIMARY KEY (Id),
    CONSTRAINT Fk_CatalogId FOREIGN KEY (CatalogId) REFERENCES Catalogs (Id),
    CONSTRAINT Fk_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customers (Id)
);

CREATE TABLE [dbo].FileStorage
(
    Id       INT IDENTITY (1,1) NOT NULL,
    UserId   INT                NOT NULL,
    FileName NVARCHAR(100)      NOT NULL,
    CONSTRAINT Pk_Id PRIMARY KEY (Id),
);
