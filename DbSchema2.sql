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

create table message (
	  messageId int not null identity(1,1)
	, sendAt datetime not null
	, subject varchar(80) not null
	, body varchar(250) null
	, unread bit not null default(1)
	, constraint pk_messages primary key (messageId)
)
go

create table usersMessages (
	  messageId int not null 
	, UserId int not null
	, SenderOrReceiver int not null
	, constraint pk_usersMessages primary key (messageId, UserId, SenderOrReceiver)
)
go

create view messages as 
select m.messageId, sender.UserId as senderUserId, receiver.UserId as receiverUserId, m.sendAt, m.subject, m.body, m.unread
from message m 
	inner join usersMessages sender on m.messageId = sender.messageId and sender.SenderOrReceiver=1
	inner join usersMessages receiver on m.messageId = receiver.messageId and receiver.SenderOrReceiver=2 

go


create procedure Validate_User
	  @userName varchar(30)
	, @userPassword varchar(30)
as
begin
		select count(*)
		from users 
		where  userPassword = HASHBYTES('SHA2_512', @userPassword+CAST(salt AS NVARCHAR(36)))
		   and userName = @userName
end
go

create procedure InsertUser 
	  @userName varchar(30)
	, @userPassword varchar(30) 
	, @firstName varchar(50)
	, @lastName varchar(50) 
	, @userRole varchar(30) 
aS
begin

	declare @salt uniqueidentifier = newid()
	
	insert into Users (userName, userPassword, salt , firstName, lastName,  userRole) 
	values (@userName, HASHBYTES('SHA2_512', @userPassword+CAST(@salt AS NVARCHAR(36))), @salt , @firstName, @lastName,  @userRole)
	select SCOPE_IDENTITY();
end
go

create procedure DeleteUser
	  @superUserId int
	, @superUserPassword varchar(30) 
	, @userId int

as 
begin
	if ( exists(
		select userId
		from users 
		where  userPassword = HASHBYTES('SHA2_512', @superUserPassword+CAST(salt AS NVARCHAR(36)))
		   and userId = @superUserId and userRole='Super'))
	begin
		delete from users where userId = @userId
	end
	
end
go



create procedure InsertMessage
	  @senderUserId int
	, @receiverUserId int
	, @subject varchar(80)
	, @body varchar(255) 
aS
begin
	declare @messageId int

	insert into message ([subject], [body], [sendAt]) values(@subject,@body,GetDate())
	select @messageId=SCOPE_IDENTITY()

	insert into usersMessages (messageId,UserId,SenderOrReceiver) values 
	 (@messageId, @senderUserId, 1)
	,(@messageId, @receiverUserId, 2)

	select @messageId
end
go

create procedure DeleteMessage
	  @messageId int

as 
begin
	delete from messages where messageId=@messageId
end
go

create procedure UpdateMessage
	  @messageId int
	, @subject varchar(80)
	, @body varchar(255) 
as 
begin
		update messages set
		  [subject] = @subject
		, [body] = @body
		where messageId=@messageId
	
end
go

create procedure UpdateMessageAsRead
	  @messageId int
	, @unread bit
as 
begin
	update messages set [unread] = @unread
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
		select messages.*
		, concat(sender.[lastName],' ',sender.firstName) as senderUserName
		, concat(receiver.[lastName],' ',receiver.firstName) as receiverUserName
		from messages 
			inner join users sender on messages.senderUserId = sender.userId 
			inner join users receiver on messages.receiverUserId = receiver.userId 
		where messageId=@messageId
	end
	else
	begin
		select messages.*
		, concat(sender.[lastName],' ',sender.firstName) as senderUserName
		, concat(receiver.[lastName],' ',receiver.firstName) as receiverUserName
		from messages 
			inner join users sender on messages.senderUserId = sender.userId 
			inner join users receiver on messages.receiverUserId = receiver.userId 
		where senderUserId=@userId or receiverUserId=@userId	
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