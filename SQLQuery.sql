create database ManagerStoreWPF
go
use ManagerStoreWPF
go
create table RoleTable(
	ID int IDENTITY(1,1) primary key not null,
	Role nvarchar(max) not null
)
go
create table CustomerTable(
	ID int IDenTITY(1,1) primary key not null,
	FullName nvarchar(max) not null,
	Phone varchar(10) not null,
	Address nvarchar(max) not null
)
go
create table UserTable(
	ID int Identity(1,1) primary key not null,
	ID_Role int,
	DisplayName nvarchar(max) not null,
	UserName varchar(max) not null,
	Password varchar(max) not null,
	foreign key(ID_Role) references RoleTable(ID)
)
go
create table SupplierTable(
	ID int identity(1,1) primary key not null,
	DisplayName nvarchar(max) not null,
	Address nvarchar(max) not null,
	Phone varchar(10) not null,
	ContractDay date not null,
)
go
create table ProductTable(
	ID int identity(1,1) primary key not null,
	DisplayName nvarchar(max) not null,
	Image varchar(max) not null,
	ID_Supplier int not null,
	foreign key(ID_Supplier) references SupplierTable(ID),
)
go
create table InputTable(
	ID int identity(1,1) primary key not null,
	ID_User int not null,
	DateInput date not null,
	foreign key(ID_User) references UserTable(ID)
)
go
create table InputDetailTable(
	ID int identity(1,1) primary key not null,
	ID_Product int not null,
	ID_Supplier int not null,
	PriceInput int not null,
	PriceSale int not null,
	Amount int not null,
	Status nvarchar(max),
	foreign key(ID_Product) references ProductTable(ID),
	foreign key(ID_Supplier) references SupplierTable(ID)
)
go
create table OutputTable(
	ID int identity(1,1) primary key not null,
	ID_User int not null,
	DateOutput date not null,
	foreign key(ID_User) references UserTable(ID)
)
go 
create table OutPutDetailTable(
	ID int identity(1,1) primary key not null,
	ID_Product int not null,
	ID_Customer int not null,
	ID_InputDetail int not null,
	Count int not null,
	Status nvarchar(max),
	foreign key(ID_Product) references ProductTable(ID),
	foreign key(ID_Customer) references CustomerTable(ID),
	foreign key(ID_InputDetail) references InputDeTailTable(ID)
)