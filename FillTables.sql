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

,(@fId, @lId, '2019-01-18 09:41', 'Good morning !!!', N'καλή επιτυχια στην παρουσιαση!!!')
,(@fId, @kId, '2019-01-18 09:41', 'Good morning !!!', N'καλή επιτυχια στην παρουσιαση!!!' )
,(@fId, @xId, '2019-01-18 09:41', 'Good morning !!!',  N'καλή επιτυχια στην παρουσιαση!!!')

,(@lId, @fId, '2019-01-18 09:43', 'Τώρα ξυπνησες εσύ ???', N'Ξενυχτισες με το project !!!')
,(@kId, @fId, '2019-01-18 09:46', 'Thanks !!!', N'καλή επιτυχια και σε σενα !!!' )
,(@xId, @fId, '2019-01-18 09:48', 'Ευχαριστω !!!',  N'Επίσης !!!')

,(@fId, @lId, '2019-01-18 09:55', 'Εγω φταιω που σου εστειλα και καλη επιτυχια !!!', null)

,(@fId, @AdminId, '2019-01-18 09:55', 'Αγαπητε Administrator', 'Θέλω να διαγαρη ο λογαριασμος του george lyberopoulos αμεσα !!!')
,(@AdminId, @fId, '2019-01-18 11:00', 'Cant do that', null)
,(@fId, @xId, '2019-01-18 11:35', 'Γιωργο Help', 'Θέλω να διαγραφη ο λογαριασμος του george lyberopoulos μπορεις να κανεις κατι ?')
,(@xId, @fId, '2019-01-18 11:40', 'Αυτη η βαση δεν hackarete με τιποτα οι κωδικοι ειναι με hash_kati ???', 'sorry!!!')

,(@dId, @xId, '2019-01-17 12:45', 'Γιωργος τι εκανες με τα mesages τελικα τα διαγραφεις ???', null)
,(@xId, @dId, '2019-01-17 12:47', 'Εαμ πως keep it simple !!!', 'χιχιχι')

,(@dId, @xId, '2019-01-17 12:48', 'Τα διαγραφεις και απο τον αποστολεα και παραληπτη ???', null)
,(@xId, @dId, '2019-01-17 12:49', 'Φυσικα... τι αλλο???', null)

,(@dId, @xId, '2019-01-17 21:48', 'μετα το μαθημα με τον πασπαρακη τι εχεις να πεις ???', null)
,(@xId, @dId, '2019-01-17 21:49', 'Refactoring Database big time!!!', 'χιχιχι')

,(@afdempkstr,@fId, '2019-01-15 18:00', 'Συμβουλη για το project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', 'Συμβουλη για το project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', 'Συμβουλη για το project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', 'Συμβουλη για το project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', 'Συμβουλη για το project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', 'Συμβουλη για το project!!!', 'keep it simple')
,(@afdempkstr,@fId, '2019-01-15 18:00', 'Συμβουλη για το project!!!', 'keep it simple')

,(@afdempkstr,@gpasp, '2019-01-18 10:00', 'Καλημερα πως πηγε η ομαδα χθες ???', null)
,(@gpasp,@afdempkstr, '2019-01-18 10:05', 'Πετάει.... στα συννεφα !!!', null)

,(@afdempkstr,@gpasp, '2019-01-18 10:06', 'ουπς ...', null)
,(@gpasp,@afdempkstr, '2019-01-18 10:07', 'Πλακα κανω οι C#δες τα σπανε...!!!', null)
,(@afdempkstr,@gpasp, '2019-01-18 10:08', 'Ειπα κι εγω... εχει μαλλιασει η γλωσσα μου !!!', null)

