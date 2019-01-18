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



execute InsertUser 'geo.kal', '1234', 'george', 'kalomalos', 'Simple'
go
execute InsertUser 'fr.bou', '1234', 'fryni', 'boukouvala', 'Simple'
go
execute InsertUser 'geo.lyb', '1234', 'george', 'lyberopoulos', 'Simple'
go
execute InsertUser 'nick.diak', '1234', 'nick', 'diakos', 'Simple'
go
execute InsertUser 'afdempkstr', '1234', 'costas', 'stroggylos', 'Simple'
go
execute InsertUser 'geo.pasp', '1234', 'george', 'pasparakis', 'Simple'
go
execute InsertUser 'geo.xiros', '1234', 'george', 'xiros', 'Simple'
go



delete from messages
go


declare @AdminId int
declare @SUId int
declare @VId int
declare @VEId int
declare @VEDId int
declare @afdempkstr int 
declare @gpasp int 

declare @fId int
declare @lId int
declare @kId int
declare @xId int
declare @dId int

select @fId = userId from users where users.userName= 'fr.bou'
select @lId = userId from users where users.userName= 'geo.lyb'
select @kId = userId from users where users.userName= 'geo.kal'
select @xId = userId from users where users.userName= 'geo.xiros'
select @dId = userId from users where users.userName= 'nick.diak'
select @afdempkstr = userId from users where users.userName= 'afdempkstr'
select @gpasp = userId from users where users.userName= 'geo.pasp'


select @AdminId = userId from users where users.userName= 'admin'
select @SUId = userId from users where users.userName= 's'
select @VId = userId from users where users.userName= 'v'
select @VEId = userId from users where users.userName= 've'
select @VEDId = userId from users where users.userName= 'ved'




INSERT INTO messages ([senderUserId], [receiverUserId], [sendAt], [subject], [body]) VALUES 
 (@VEDId, @AdminId, '2019-01-01 12:30', 'Need View Edit Delete Permissions', 'For testing purpose...')
,(@AdminId, @VEDId, '2019-01-01 12:35', 'Cant do that', 'For Administrator security purpose...')
,(@VEDId, @AdminId, '2019-01-01 12:40', 'I am your Boss...', 'Do it now!!!')
,(@AdminId, @VEDId, '2019-01-01 12:41', 'Done!!!', null)
,(@VId, @VEDId, '2019-01-02 11:41', 'happy new year !!!', null)
,(@VEDId, @VId, '2019-01-02 11:41', 'Please leave me alone!!!', null)
,(@VId, @VEDId, '2019-01-02 11:45', 'Sorry !!!', null)
,(@VEDId, @VId, '2019-01-02 11:41', 'Please leave me alone!!!', null)

,(@fId, @lId, '2019-01-18 09:41', 'Good morning !!!', N'���� �������� ���� ����������!!!')
,(@fId, @kId, '2019-01-18 09:41', 'Good morning !!!', N'���� �������� ���� ����������!!!' )
,(@fId, @xId, '2019-01-18 09:41', 'Good morning !!!',  N'���� �������� ���� ����������!!!')

,(@lId, @fId, '2019-01-18 09:43', '���� �������� ��� ???', N'���������� �� �� project !!!')
,(@kId, @fId, '2019-01-18 09:46', 'Thanks !!!', N'���� �������� ��� �� ���� !!!' )
,(@xId, @fId, '2019-01-18 09:48', '��������� !!!',  N'������ !!!')

,(@fId, @lId, '2019-01-18 09:55', '��� ����� ��� ��� ������� ��� ���� �������� !!!', null)

,(@fId, @AdminId, '2019-01-18 09:55', '������� Administrator', '���� �� ������� � ����������� ��� george lyberopoulos ����� !!!')
,(@AdminId, @fId, '2019-01-18 11:00', 'Cant do that', null)
,(@fId, @xId, '2019-01-18 11:35', '������ Help', '���� �� �������� � ����������� ��� george lyberopoulos ������� �� ������ ���� ?')
,(@xId, @fId, '2019-01-18 11:40', '���� � ���� ��� hackarete �� ������ �� ������� ����� �� hash_kati ???', 'sorry!!!')

,(@dId, @xId, '2019-01-17 12:45', '������� �� ������ �� �� mesages ������ �� ���������� ???', null)
,(@xId, @dId, '2019-01-17 12:47', '��� ��� keep it simple !!!', '������')

,(@dId, @xId, '2019-01-17 12:48', '�� ���������� ��� ��� ��� ��������� ��� ��������� ???', null)
,(@xId, @dId, '2019-01-17 12:49', '������... �� ����???', null)

,(@dId, @xId, '2019-01-17 21:48', '���� �� ������ �� ��� ��������� �� ����� �� ���� ???', null)
,(@xId, @dId, '2019-01-17 21:49', 'Refactoring Database big time!!!', '������')

,(@afdempkstr,@fId, '2019-01-15 18:00', '�������� ��� �� project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', '�������� ��� �� project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', '�������� ��� �� project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', '�������� ��� �� project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', '�������� ��� �� project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', '�������� ��� �� project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', '�������� ��� �� project!!!', 'keep it simple')

,(@afdempkstr,@gpasp, '2019-01-18 10:00', '�������� ��� ���� � ����� ���� ???', null)
,(@gpasp,@afdempkstr, '2019-01-18 10:05', '������.... ��� ������� !!!', null)

,(@afdempkstr,@gpasp, '2019-01-18 10:06', '���� ...', null)
,(@gpasp,@afdempkstr, '2019-01-18 10:07', '����� ���� �� C#��� �� �����...!!!', null)
,(@afdempkstr,@gpasp, '2019-01-18 10:08', '���� �� ���... ���� ��������� � ������ ��� !!!', null)

