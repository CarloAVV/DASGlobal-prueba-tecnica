insert into EMPRESA values ( 'nombre empresa prueba 1','guatemala')

insert into SUCURSAL values ('nombre sucursal prueba 1','direccion x','99999999',1)
insert into SUCURSAL values ('nombre sucursal prueba 2','direccion x','99999999',1)

insert into COLABORADOR values ('nombre colab 1','1234567001',1)
insert into COLABORADOR values ('nombre colab 2','1234567002',1)
insert into COLABORADOR values ('nombre colab 3','1234567003',2)
insert into COLABORADOR values ('nombre colab 4','1234567004',2)


insert into CheckLogin values(getdate()) select count(*) from checklogin
insert into CheckLogin values(getdate()) select count(*) from checklogin


select * from
COLABORADOR c inner join
SUCURSAL s on c.ID_Sucursal = s.ID inner join
EMPRESA e on s.ID_Empresa = e.ID

