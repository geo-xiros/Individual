create database project_db collate greek_ci_as
go

use project_db
go

create table users (
	  userId int not null identity(1,1)
	, userName varchar(30) not null
	, userPassword BINARY(64) not null
	, salt uniqueidentifier not null 
	, firstName varchar(50) not null
	, lastName varchar(50) not null
	, userRole varchar(30) not null 
	, constraint pk_user primary key (UserID)
)
go

create unique index ix_Users_Username on users (username)
go

create table messages (
	  messageId int not null identity(1,1)
	, senderUserId int not null 
	, receiverUserId int not null
	, sendAt datetime not null
	, subject varchar(80) not null
	, body varchar(250) null
	, unread bit not null default(1)
	, constraint pk_messages primary key (messageId)
)
go


CREATE TRIGGER [dbo].[T_users_UTrig] ON [dbo].[users] FOR UPDATE AS
SET NOCOUNT ON
	/* * PREVENT UPDATES IF DEPENDENT RECORDS IN 'messages' */
	IF UPDATE(userId)
		BEGIN
			IF (SELECT COUNT(*) FROM deleted, messages WHERE (deleted.userId = messages.senderUserId)) > 0
				BEGIN
					RAISERROR (N'The record can''t be deleted or changed. Since related records exist in table ''messages'', referential integrity rules would be violated.',16,1)
					ROLLBACK TRANSACTION
				END
		END

	/* * PREVENT UPDATES IF DEPENDENT RECORDS IN 'messages' */
	IF UPDATE(userId)
		BEGIN
			IF (SELECT COUNT(*) FROM deleted, messages WHERE (deleted.userId = messages.receiverUserId)) > 0
				BEGIN
					RAISERROR (N'The record can''t be deleted or changed. Since related records exist in table ''messages'', referential integrity rules would be violated.',16,1)
					ROLLBACK TRANSACTION
				END
		END
GO

CREATE TRIGGER [dbo].[T_users_DTrig] ON [dbo].[users] FOR DELETE AS
SET NOCOUNT ON
	/* * CASCADE DELETES TO 'messages' */
	DELETE messages FROM deleted, messages WHERE deleted.userId = messages.senderUserId

	/* * CASCADE DELETES TO 'messages' */
	DELETE messages FROM deleted, messages WHERE deleted.userId = messages.receiverUserId
GO
	
CREATE TRIGGER [dbo].[T_messages_UTrig] ON [dbo].[messages] FOR UPDATE AS
SET NOCOUNT ON
	/* * PREVENT UPDATES IF NO MATCHING KEY IN 'users' */
	IF UPDATE(senderUserId)
		BEGIN
			IF (SELECT COUNT(*) FROM inserted) !=
			   (SELECT COUNT(*) FROM users, inserted WHERE (users.userId = inserted.senderUserId))
				BEGIN
					RAISERROR (N'The record can''t be added or changed. Referential integrity rules require a related record in table ''users''.',16,1)
					ROLLBACK TRANSACTION
				END
		END

	/* * PREVENT UPDATES IF NO MATCHING KEY IN 'users' */
	IF UPDATE(receiverUserId)
		BEGIN
			IF (SELECT COUNT(*) FROM inserted) !=
			   (SELECT COUNT(*) FROM users, inserted WHERE (users.userId = inserted.receiverUserId))
				BEGIN
					RAISERROR (N'The record can''t be added or changed. Referential integrity rules require a related record in table ''users''.',16,1)
					ROLLBACK TRANSACTION
				END
		END	
GO
		
CREATE TRIGGER [dbo].[T_messages_ITrig] ON [dbo].[messages] FOR INSERT AS
SET NOCOUNT ON
	/* * PREVENT INSERTS IF NO MATCHING KEY IN 'users' */
	IF (SELECT COUNT(*) FROM inserted) !=
	   (SELECT COUNT(*) FROM users, inserted WHERE (users.userId = inserted.senderUserId))
		BEGIN
			RAISERROR (N'The record can''t be added or changed. Referential integrity rules require a related record in table ''users''.',16,1)
			ROLLBACK TRANSACTION
		END

	/* * PREVENT INSERTS IF NO MATCHING KEY IN 'users' */
	IF (SELECT COUNT(*) FROM inserted) !=
	   (SELECT COUNT(*) FROM users, inserted WHERE (users.userId = inserted.receiverUserId))
		BEGIN
			RAISERROR (N'The record can''t be added or changed. Referential integrity rules require a related record in table ''users''.',16,1)
			ROLLBACK TRANSACTION
		END		
GO

create procedure Validate_User
	  @userName varchar(50)
	, @userPassword BINARY(64)
as
begin
		select userName, firstName, lastName
		from users 
		where  userPassword = HASHBYTES('SHA2_512', cast(@userPassword as nvarchar(200))+CAST(salt AS NVARCHAR(36)))
		   and userName = @userName
end
go

--Create function Validate_User( @userName varchar(50), @userPassword BINARY(64)) 
--	returns bit as 
--begin

--	return iif( exists(
--		select *
--		from users 
--		where  userPassword = HASHBYTES('SHA2_512', cast(@userPassword as nvarchar(200))+CAST(salt AS NVARCHAR(36)))
--		and userName = @userName)
--		,1,0)
--end
--go


create procedure InsertUser 
	  @userName varchar(50)
	, @userPassword BINARY(64) 
	, @firstName varchar(50)
	, @lastName varchar(50) 
	, @userRole varchar(30) 
aS
begin
	--set nocount on
	declare @salt uniqueidentifier = newid()
	declare @saltedPassword NVARCHAR(200)
	set @saltedPassword = cast(@userPassword as nvarchar(200))+CAST(@salt AS NVARCHAR(36))
	
	insert into Users (userName, userPassword, salt , firstName, lastName,  userRole) 
	values (@userName, HASHBYTES('SHA2_512', @saltedPassword), @salt , @firstName, @lastName,  @userRole)

end
go

create procedure DeleteUser
	  @superUserId int
	, @superUserPassword BINARY(64) 
	, @userId int

as 
begin
	if ( exists(
		select userId
		from users 
		where  userPassword = HASHBYTES('SHA2_512', cast(@superUserPassword as nvarchar(200))+CAST(salt AS NVARCHAR(36)))
		   and userId = @superUserId and userRole='Super'))
	begin
		delete from users where userId = @userId
	end
	
end
go

create procedure UpdateUser 
	  @userId int
	, @userName varchar(50)
	, @userPassword BINARY(64) 
	, @firstName varchar(50)
	, @lastName varchar(50) 
	, @userRole varchar(30) 
as 
begin
	--set nocount on 
	if (HASHBYTES('SHA2_512','') =  @userPassword)
		update Users set 
			  userName = @userName
			, firstName = @firstName 
			, lastName = @lastName 
			, userRole = @userRole
		where userId = @userId 
	else
		update Users set 
			  userName = @userName
			, userPassword = HASHBYTES('SHA2_512', cast(@userPassword as nvarchar(200))+CAST(salt AS NVARCHAR(36)))
			, firstName = @firstName 
			, lastName = @lastName 
			, userRole = @userRole
		where userId = @userId 
end
go


alter procedure InsertMessage
	  @senderUserId int
	, @receiverUserId int
	, @subject varchar(80)
	, @body varchar(255) 
	, @messageId int OUTPUT
aS
begin

	INSERT INTO [messages] ([senderUserId], [receiverUserId], [subject], [body], [sendAt])
     VALUES (@senderUserId, @receiverUserId, @subject, @body, getdate())

	SET @messageId = SCOPE_IDENTITY();
end
go

--declare @okToDelete bit
--declare @deleteUserId int
--set @deleteUserId=661

--declare @superUserPassword binary(64)
--set @superUserPassword = HASHBYTES('SHA2_512','1234hQw')

--select @okToDelete = count(*)
--from users 
--where  userPassword = HASHBYTES('SHA2_512', cast(@superUserPassword as nvarchar(200))+CAST(salt AS NVARCHAR(36)))
--	and userId = 3 and userRole like'%Delete%'

--print @okToDelete

create procedure DeleteMessage
	  @deleteUserId int
	, @deleteUserPassword BINARY(64) 
	, @messageId int

as 
begin
	declare @okToDelete bit;
	select @okToDelete=count(*) from messages where messageId=@messageId and senderUserId=@deleteUserId

	if @okToDelete=0
		select @okToDelete = count(*)
		from users 
		where  userPassword = HASHBYTES('SHA2_512', cast(@deleteUserPassword as nvarchar(200))+CAST(salt AS NVARCHAR(36)))
	  	    and userId = @deleteUserId and userRole like'%Delete%'
	
	if @okToDelete=1
		delete from messages where messageId=@messageId
	
end
go

create procedure UpdateMessage
	  @updateUserId int
	, @updateUserPassword BINARY(64) 
	, @messageId int
	, @subject varchar(80)
	, @body varchar(255) 

as 
begin
	declare @okToDelete bit;
	select @okToDelete=count(*) from messages where messageId=@messageId and senderUserId=@updateUserId

	if @okToDelete=0
		select @okToDelete = count(*)
		from users 
		where  userPassword = HASHBYTES('SHA2_512', cast(@updateUserPassword as nvarchar(200))+CAST(salt AS NVARCHAR(36)))
	  	    and userId = @updateUserId and userRole like'%Edit%'
	
	if @okToDelete=1
		update messages set
		  [subject] = @subject
		, [body] = @body
		where messageId=@messageId
	
end
go

create procedure GetMessages
	  @messageId int
	, @userId int

as 
begin
	if @messageId<>0
	begin
		select * from messages where messageId=@messageId
	end
	else
	begin
		select * from messages where senderUserId=@userId or receiverUserId=@userId	
	end
end
go

create procedure GetUsers
	    @userId int
	  , @userName varchar(30)
as 
begin
	if @userId<>0
	begin
		SELECT userId, userName, firstName, lastName, userRole FROM users where userId=@userId
	end
	else if Len(@userName)>0
	begin
		SELECT userId, userName, firstName, lastName, userRole FROM users  where userName=@userName
	end
	else
	begin
		SELECT userId, userName, firstName, lastName, userRole FROM users users 
	end
end


GO
