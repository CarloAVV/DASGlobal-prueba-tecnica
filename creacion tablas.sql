create table CheckLogin
(
	LoginID int primary key identity,
	LoginDate date
)
create table EMPRESA
(
	Nombre varchar(50),
	Pais varchar(20),
	ID int primary key identity
)
create table SUCURSAL
(
	Nombre varchar(50),
	Direccion varchar(250),
	Telefono varchar(15),
	ID int primary key identity,
	ID_Empresa int foreign key references EMPRESA(ID)
)

create table COLABORADOR
(
	Nombre varchar(50),
	CUI varchar(13) primary key,
	ID_Sucursal int foreign key references SUCURSAL(ID)
)
