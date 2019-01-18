USE [project_db]
GO

delete from users
go

execute InsertUser 'admin', 'admin', 'Admin', '.Super', 'Super'
go

execute InsertUser 's', '1234', 'User', '.simple', 'Simple'
go

execute InsertUser 'v', '1234', 'User', '.View', 'View'
go

execute InsertUser 've', '1234', 'User', '.ViewEdit', 'ViewEdit'
go

execute InsertUser 'ved', '1234', 'User', '.ViewEditDelete', 'ViewEditDelete'
go

execute InsertUser 'geo.xiros', '1234', 'George', 'Xiros', 'view'
go

execute InsertUser 'geo.lyb', '1234', 'George', 'Lyberopoulos', 'ViewEditDelete'
go

execute InsertUser 'geo.kal', '1234', 'George', 'Kalomalos', 'View'
go

execute InsertUser 'afdemp.kstr', '1234', 'Kostas', 'Strogylos', 'Super'
go

execute InsertUser 'frini.boukouvala', '1234', 'George', 'Lyberopoulos', 'ViewEditDelete'
go

execute InsertUser 'geo.pasp', '1234', 'George', 'Pasparakis', 'ViewEditDelete'
go



execute InsertUser 'nick.diak', '1234', 'Nick', 'Diakos', 'ViewEdit'
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
,(@VEDId, @AdminId, '2019-01-01 12:40', 'I am your Boss...', 'Do it now!!!')
,(@AdminId, @VEDId, '2019-01-01 12:41', 'Done!!!', null)

,(@VId, @VEDId, '2019-01-03 11:20', 'Hello', 'how are you ?')
,(@VEDId, @VId, '2019-01-03 11:30', 'Hi there', 'I am really busy what do you want ?')
,(@VId, @VEDId, '2019-01-03 11:35', 'Sorry !!!', null)

,(@VEDId, @AdminId, '2019-01-04 14:10', 'I need to delete this user', 'Please delete user v...')
,(@AdminId, @VEDId, '2019-01-01 12:41', 'Sorry ', 'I cant delete tat user')


