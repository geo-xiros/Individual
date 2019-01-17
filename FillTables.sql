USE [project_db]
GO

delete from users
go

execute InsertUser 'admin', 'admin', 'Admin', 'Super', 'Super'
go

execute InsertUser 's', '1234', 'User', 'simple', 'Simple'
go

execute InsertUser 'v', '1234', 'User', 'View', 'View'
go

execute InsertUser 've', '1234', 'User', 'ViewEdit', 'ViewEdit'
go

execute InsertUser 'ved', '1234', 'User', 'ViewEditDelete', 'ViewEditDelete'
go

delete from messages
go


declare @AdminId int
declare @SUId int
declare @VId int
declare @VEId int
declare @VEDId int

select @AdminId = userId from users where users.userName= 'admin'
select @SUId = userId from users where users.userName= 's'
select @VId = userId from users where users.userName= 'v'
select @VEId = userId from users where users.userName= 've'
select @VEDId = userId from users where users.userName= 'ved'

print @AdminId
print @SUId
print @VId
print @VEId
print @VEDId



INSERT INTO messages ([senderUserId], [receiverUserId], [sendAt], [subject], [body]) VALUES 
 (@VEDId, @AdminId, '2019-01-01 12:30', 'Need View Edit Delete Permissions', 'For testing purpose...')
,(@AdminId, @VEDId, '2019-01-01 12:35', 'Cant do that', 'For Administrator security purpose...')

