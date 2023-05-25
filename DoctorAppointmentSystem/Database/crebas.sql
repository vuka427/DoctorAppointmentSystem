/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     5/18/2023 10:04:12 PM                        */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('APPOINTMENT') and o.name = 'FK_APPOINTM_RELATIONS_PATIENT')
alter table APPOINTMENT
   drop constraint FK_APPOINTM_RELATIONS_PATIENT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('APPOINTMENT') and o.name = 'FK_APPOINTM_RELATIONS_SCHEDULE')
alter table APPOINTMENT
   drop constraint FK_APPOINTM_RELATIONS_SCHEDULE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('APPOINTMENT') and o.name = 'FK_APPOINTM_RELATIONS_CONSULTA')
alter table APPOINTMENT
   drop constraint FK_APPOINTM_RELATIONS_CONSULTA
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('APPOINTMENT') and o.name = 'FK_APPOINTM_RELATIONS_MODE_OF_')
alter table APPOINTMENT
   drop constraint FK_APPOINTM_RELATIONS_MODE_OF_
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('APPOINTMENT_NOTE') and o.name = 'FK_APPOINTM_RELATIONS_PRESCRIP')
alter table APPOINTMENT_NOTE
   drop constraint FK_APPOINTM_RELATIONS_PRESCRIP
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('APPOINTMENT_NOTE') and o.name = 'FK_APPOINTM_RELATIONS_APPOINTM')
alter table APPOINTMENT_NOTE
   drop constraint FK_APPOINTM_RELATIONS_APPOINTM
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('DOCTOR') and o.name = 'FK_DOCTOR_RELATIONS_DEPARTME')
alter table DOCTOR
   drop constraint FK_DOCTOR_RELATIONS_DEPARTME
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('DOCTOR') and o.name = 'FK_DOCTOR_RELATIONS_USER')
alter table DOCTOR
   drop constraint FK_DOCTOR_RELATIONS_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('PATIENT') and o.name = 'FK_PATIENT_RELATIONS_USER')
alter table PATIENT
   drop constraint FK_PATIENT_RELATIONS_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('QUALIFICATION') and o.name = 'FK_QUALIFIC_RELATIONS_DOCTOR')
alter table QUALIFICATION
   drop constraint FK_QUALIFIC_RELATIONS_DOCTOR
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SCHEDULE') and o.name = 'FK_SCHEDULE_RELATIONS_DOCTOR')
alter table SCHEDULE
   drop constraint FK_SCHEDULE_RELATIONS_DOCTOR
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('"USER"') and o.name = 'FK_USER_RELATIONS_ROLE')
alter table "USER"
   drop constraint FK_USER_RELATIONS_ROLE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('APPOINTMENT')
            and   name  = 'RELATIONSHIP_14_FK'
            and   indid > 0
            and   indid < 255)
   drop index APPOINTMENT.RELATIONSHIP_14_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('APPOINTMENT')
            and   name  = 'RELATIONSHIP_17_FK'
            and   indid > 0
            and   indid < 255)
   drop index APPOINTMENT.RELATIONSHIP_17_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('APPOINTMENT')
            and   name  = 'RELATIONSHIP_16_FK'
            and   indid > 0
            and   indid < 255)
   drop index APPOINTMENT.RELATIONSHIP_16_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('APPOINTMENT')
            and   name  = 'RELATIONSHIP_13_FK'
            and   indid > 0
            and   indid < 255)
   drop index APPOINTMENT.RELATIONSHIP_13_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('APPOINTMENT')
            and   type = 'U')
   drop table APPOINTMENT
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('APPOINTMENT_NOTE')
            and   name  = 'RELATIONSHIP_15_FK'
            and   indid > 0
            and   indid < 255)
   drop index APPOINTMENT_NOTE.RELATIONSHIP_15_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('APPOINTMENT_NOTE')
            and   name  = 'RELATIONSHIP_12_FK'
            and   indid > 0
            and   indid < 255)
   drop index APPOINTMENT_NOTE.RELATIONSHIP_12_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('APPOINTMENT_NOTE')
            and   type = 'U')
   drop table APPOINTMENT_NOTE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CONSULTANT_TYPE')
            and   type = 'U')
   drop table CONSULTANT_TYPE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEPARTMENT')
            and   type = 'U')
   drop table DEPARTMENT
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('DOCTOR')
            and   name  = 'RELATIONSHIP_22_FK'
            and   indid > 0
            and   indid < 255)
   drop index DOCTOR.RELATIONSHIP_22_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('DOCTOR')
            and   name  = 'RELATIONSHIP_20_FK'
            and   indid > 0
            and   indid < 255)
   drop index DOCTOR.RELATIONSHIP_20_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DOCTOR')
            and   type = 'U')
   drop table DOCTOR
go

if exists (select 1
            from  sysobjects
           where  id = object_id('MODE_OF_CONSULTING')
            and   type = 'U')
   drop table MODE_OF_CONSULTING
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('PATIENT')
            and   name  = 'RELATIONSHIP_19_FK'
            and   indid > 0
            and   indid < 255)
   drop index PATIENT.RELATIONSHIP_19_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('PATIENT')
            and   type = 'U')
   drop table PATIENT
go

if exists (select 1
            from  sysobjects
           where  id = object_id('PRESCRIPTION')
            and   type = 'U')
   drop table PRESCRIPTION
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('QUALIFICATION')
            and   name  = 'RELATIONSHIP_1_FK'
            and   indid > 0
            and   indid < 255)
   drop index QUALIFICATION.RELATIONSHIP_1_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('QUALIFICATION')
            and   type = 'U')
   drop table QUALIFICATION
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ROLE')
            and   type = 'U')
   drop table ROLE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('SCHEDULE')
            and   name  = 'RELATIONSHIP_7_FK'
            and   indid > 0
            and   indid < 255)
   drop index SCHEDULE.RELATIONSHIP_7_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SCHEDULE')
            and   type = 'U')
   drop table SCHEDULE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('"USER"')
            and   name  = 'RELATIONSHIP_18_FK'
            and   indid > 0
            and   indid < 255)
   drop index "USER".RELATIONSHIP_18_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"USER"')
            and   type = 'U')
   drop table "USER"
go

/*==============================================================*/
/* Table: APPOINTMENT                                           */
/*==============================================================*/
create table APPOINTMENT (
   APPOIMENTNO          int                  identity(1011,1),
   PATIENTID            int                  not null,
   MODEID              int                  not null,
   DOCTORID             int                  not null,
   WORKINGDAY           date	             not null,
   CONSULTANTTYPEID     int                  not null,
   APPOINTMENTNAME      nvarchar(256)         not null,
   DATEOFCONSULTATION   datetime             not null,
   APPOINTMENTDATE      datetime             not null,
   APPOIMENTSTATUS      nvarchar(256)         not null,
   CLOSEDDATE           datetime             null,
   CLOSEDBY             nvarchar(256)         null,
   SYMTOMS              nvarchar(256)         null,
   EXISTIONGILLNESS     nvarchar(256)         null,
   DRUGALLERGLES        nvarchar(256)         null,
   NOTE                 nvarchar(256)         null,
   CASENOTE             nvarchar(256)         null,
   DIAGNOSIS            nvarchar(256)         null,
   ADVICETOPATIENT      nvarchar(256)         null,
   LABTESTS             nvarchar(256)         null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_APPOINTMENT primary key nonclustered (APPOIMENTNO)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_13_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_13_FK on APPOINTMENT (
PATIENTID ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_16_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_16_FK on APPOINTMENT (
CONSULTANTTYPEID ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_17_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_17_FK on APPOINTMENT (
MODEID ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_14_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_14_FK on APPOINTMENT (
DOCTORID ASC,
WORKINGDAY ASC
)
go

/*==============================================================*/
/* Table: APPOINTMENT_NOTE                                      */
/*==============================================================*/
create table APPOINTMENT_NOTE (
   PRECRIPTIONID        int					 not null,
   APPOIMENTNO          int                  not null,
   NOTE                 nvarchar(256)         null,
   constraint PK_APPOINTMENT_NOTE primary key (PRECRIPTIONID, APPOIMENTNO)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_12_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_12_FK on APPOINTMENT_NOTE (
PRECRIPTIONID ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_15_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_15_FK on APPOINTMENT_NOTE (
APPOIMENTNO ASC
)
go

/*==============================================================*/
/* Table: CONSULTANT_TYPE                                       */
/*==============================================================*/
create table CONSULTANT_TYPE (
   CONSULTANTTYPEID     int                  identity(1601,1),
   CONSULTANTTYPENAME   nvarchar(256)            not null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_CONSULTANT_TYPE primary key nonclustered (CONSULTANTTYPEID)
)
go

/*==============================================================*/
/* Table: DEPARTMENT                                            */
/*==============================================================*/
create table DEPARTMENT (
   DEPARTMENTID         int                  identity(101,1),
   DEPARTMENTNAME       nvarchar(256)         not null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_DEPARTMENT primary key nonclustered (DEPARTMENTID)
)
go

/*==============================================================*/
/* Table: DOCTOR                                                */
/*==============================================================*/
create table DOCTOR (
   DOCTORID             int                  identity(101101,1),
   USERID               int                  not null,
   DEPARTMENTID         int                  not null,
   DOCTORNAME           nvarchar(50)          not null,
   DOCTORNATIONALID     char(20)             not null	unique,
   DOCTORGENDER         nvarchar(20)          not null,
   DOCTORDATEOFBIRTH    date	             not null,
   DOCTORMOBILENO       char(10)             not null,
   DOCTORADDRESS        nvarchar(256)         not null,
   SPECIALITY           nvarchar(256)         not null,
   WORKINGSTARTDATE     date	             not null,
   WORKINGENDDATE       date	             not null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_DOCTOR primary key nonclustered (DOCTORID)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_20_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_20_FK on DOCTOR (
DEPARTMENTID ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_22_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_22_FK on DOCTOR (
USERID ASC
)
go

/*==============================================================*/
/* Table: MODE_OF_CONSULTING                                    */
/*==============================================================*/
create table MODE_OF_CONSULTING (
   MODEID              int                  identity(201,1),
   MODENAME            nvarchar(256)         not null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_MODE_OF_CONSULTING primary key nonclustered (MODEID)
)
go

/*==============================================================*/
/* Table: PATIENT                                               */
/*==============================================================*/
create table PATIENT (
   PATIENTID            int                  identity(1301101,1),
   USERID               int                  not null,
   PATIENTNAME          nvarchar(50)          not null,
   PATIENTNATIONALID    char(20)             not null	unique,
   PATIENTGENDER        nvarchar(20)          not null, 	
   PATIENTMOBILENO      char(10)             not null,
   PATIENTDATEOFBIRTH   date	             not null,
   PATIENTADDRESS       char(256)            not null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_PATIENT primary key nonclustered (PATIENTID)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_19_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_19_FK on PATIENT (
USERID ASC
)
go

/*==============================================================*/
/* Table: PRESCRIPTION                                          */
/*==============================================================*/
create table PRESCRIPTION (
   PRECRIPTIONID        int                  identity(1101,1),
   DRUG                 nvarchar(256)         not null,
   NOTE                 nvarchar(256)         null,
   PATIENTNAME          nvarchar(50)          not null,
   MEDICATIONDAYS       int                  not null,
   QUANTITY             int                  not null,
   UNIT                 nvarchar(50)          not null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_PRESCRIPTION primary key nonclustered (PRECRIPTIONID)
)
go

/*==============================================================*/
/* Table: QUALIFICATION                                         */
/*==============================================================*/
create table QUALIFICATION (
   QUALIFICATIONID      int                  identity(1201,1),
   DOCTORID             int                  not null,
   QUALIFICATIONNAME    nvarchar(256)         not null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_QUALIFICATION primary key nonclustered (QUALIFICATIONID)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_1_FK                                     */
/*==============================================================*/
create index RELATIONSHIP_1_FK on QUALIFICATION (
DOCTORID ASC
)
go

/*==============================================================*/
/* Table: ROLE                                                  */
/*==============================================================*/
create table ROLE (
   ROLEID               int                  identity(1301,1),
   ROLENAME             nvarchar(50)          not null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_ROLE primary key nonclustered (ROLEID)
)
go

/*==============================================================*/
/* Table: SCHEDULE                                              */
/*==============================================================*/
create table SCHEDULE (
   DOCTORID             int                  not null,
   WORKINGDAY           date	             not null,
   SHIFTTIME            time	             not null,
   BREAKTIME            time	             not null,
   AVAILABLETIME        time	             not null,
   CONSULTANTTIME       time	             not null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_SCHEDULE primary key nonclustered (DOCTORID, WORKINGDAY)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_7_FK                                     */
/*==============================================================*/
create index RELATIONSHIP_7_FK on SCHEDULE (
DOCTORID ASC
)
go

/*==============================================================*/
/* Table: "USER"                                                */
/*==============================================================*/
create table "USER" (
   USERID               int                  identity(1401,1),
   ROLEID               int                  not null,
   USERNAME             char(50)             not null	unique,
   PASSWORDHASH         char(256)            not null,
   EMAIL                char(50)             not null	unique,
   LASTLOGIN            datetime             null,
   USERTYPE             nvarchar(100)         not null,
   AVATARURL            image                null,
   PASSWORDRECOVERY1    char(256)            null,
   PASSWORDRECOVERY2    char(256)            null,
   STATUS               nvarchar(256)         not null,
   LOGINRETRYCOUNT      int                  null,
   LOGINLOCKDATE        datetime             null,
   CREATEDBY            nvarchar(50)          null,
   CREATEDDATE          datetime             null,
   UPDATEDBY            nvarchar(50)          null,
   UPDATEDDATE          datetime             null,
   DELETEDFLAG          bit                  not null,
   constraint PK_USER primary key nonclustered (USERID)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_18_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_18_FK on "USER" (
ROLEID ASC
)
go

alter table APPOINTMENT
   add constraint FK_APPOINTM_RELATIONS_PATIENT foreign key (PATIENTID)
      references PATIENT (PATIENTID)
go

alter table APPOINTMENT
   add constraint FK_APPOINTM_RELATIONS_SCHEDULE foreign key (DOCTORID, WORKINGDAY)
      references SCHEDULE (DOCTORID, WORKINGDAY)
go

alter table APPOINTMENT
   add constraint FK_APPOINTM_RELATIONS_CONSULTA foreign key (CONSULTANTTYPEID)
      references CONSULTANT_TYPE (CONSULTANTTYPEID)
go

alter table APPOINTMENT
   add constraint FK_APPOINTM_RELATIONS_MODE_OF_ foreign key (MODEID)
      references MODE_OF_CONSULTING (MODEID)
go

alter table APPOINTMENT_NOTE
   add constraint FK_APPOINTM_RELATIONS_PRESCRIP foreign key (PRECRIPTIONID)
      references PRESCRIPTION (PRECRIPTIONID)
go

alter table APPOINTMENT_NOTE
   add constraint FK_APPOINTM_RELATIONS_APPOINTM foreign key (APPOIMENTNO)
      references APPOINTMENT (APPOIMENTNO)
go

alter table DOCTOR
   add constraint FK_DOCTOR_RELATIONS_DEPARTME foreign key (DEPARTMENTID)
      references DEPARTMENT (DEPARTMENTID)
go

alter table DOCTOR
   add constraint FK_DOCTOR_RELATIONS_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table PATIENT
   add constraint FK_PATIENT_RELATIONS_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table QUALIFICATION
   add constraint FK_QUALIFIC_RELATIONS_DOCTOR foreign key (DOCTORID)
      references DOCTOR (DOCTORID)
go

alter table SCHEDULE
   add constraint FK_SCHEDULE_RELATIONS_DOCTOR foreign key (DOCTORID)
      references DOCTOR (DOCTORID)
go

alter table "USER"
   add constraint FK_USER_RELATIONS_ROLE foreign key (ROLEID)
      references ROLE (ROLEID)
go


/*==============================================================*/
/* Add Data: ROLE													*/
/*==============================================================*/
INSERT INTO [ROLE] VALUES('Admin', 'Admin', GETDATE(), 'Admin', CONVERT(DATE, GETDATE()), 'False')
INSERT INTO [ROLE] VALUES('Doctor', 'Admin', GETDATE(), 'Admin', CONVERT(DATE, GETDATE()), 'False')
INSERT INTO [ROLE] VALUES('Patient', 'Admin', GETDATE(), 'Admin', CONVERT(DATE, GETDATE()), 'False')

GO

/*==============================================================*/
/* Add Data: USER												*/
/*==============================================================*/

--INSERT 20 USERs WITH THE ROLE OF PATIENT
DECLARE @patient INT = 1;

WHILE @patient <= 20
BEGIN
 INSERT INTO [USER] VALUES(1302, 'patient'+CONVERT(nvarchar, @patient), '1234', 'patient'+CONVERT(nvarchar, @patient)+'@gmail.com', GETDATE(), 'Patient', NULL, 'passwrdrecovery1', 'passwrdrecovery2', 'active', 5, NULL, 'Admin', GETDATE(), 'Admin', GETDATE(), 'False')
 SET @patient = @patient + 1;
END;

GO

--INSERT 20 USERs WITH THE ROLE OF DOCTOR
DECLARE @doctor INT = 1;

WHILE @doctor <= 20
BEGIN
 INSERT INTO [USER] VALUES(1303, 'doctor'+CONVERT(nvarchar, @doctor), '1234', 'doctor'+CONVERT(nvarchar, @doctor)+'@gmail.com', GETDATE(), 'Patient', NULL, 'passwrdrecovery1', 'passwrdrecovery2', 'active', 5, NULL, 'Admin', GETDATE(), 'Admin', GETDATE(), 'False')
 SET @doctor = @doctor + 1;
END;

GO

--INSERT USER WITH THE ROLE OF ADMIN
INSERT INTO [USER] VALUES(1301, 'Admin', '1234', 'admin'+'@gmail.com', GETDATE(), 'Admin', NULL, 'passwrdrecovery1', 'passwrdrecovery2', 'active', 5, NULL, 'Admin', GETDATE(), 'Admin', GETDATE(), 'False')
GO
/* SYNTAX
DECLARE @cnt INT = 2;

WHILE @cnt <= 100
BEGIN
 INSERT INTO [USER] VALUES(CONVERT(INT,FLOOR(RAND()*(1305-1301+1)+1301)), 'user'+CONVERT(nvarchar, @cnt), '1234', 'user'+CONVERT(nvarchar, @cnt)+'@gmail.com', GETDATE(), 
		CASE 
          WHEN RAND() < 0.3333 THEN 'Admin'
          WHEN RAND() < 0.6666 THEN 'Doctor'
          ELSE 'Patient'
        END,
		NULL, 'passwrdrecovery1', 'passwrdrecovery2', 'active', 5, NULL, 'Admin', GETDATE(), 'Admin', GETDATE(), 'False'
 )
 SET @cnt = @cnt + 1;
END;
*/


/*==============================================================*/
/* Add Data: PATIENT											*/
/*==============================================================*/
DECLARE @patient INT = 2, @usrid INT = 1402

WHILE @patient <= 20
BEGIN
	INSERT INTO [PATIENT] VALUES(@usrid,'Patient' + CONVERT(nvarchar, @patient), 'B19060' + CONVERT(nvarchar, @patient),
	CASE 
          WHEN RAND() < 0.3333 THEN 'Male'
          WHEN RAND() < 0.6666 THEN 'Female'
          ELSE 'Other'
    END, '0946' + CONVERT(nvarchar, FLOOR(RAND()*(999999-100000+1)+100000)), CONVERT(DATE, GETDATE()), 'Can Tho City', 'Admin', GETDATE(),'Admin', GETDATE(), 'False')
    
    SET @patient = @patient + 1
    SET @usrid = @usrid + 1
END;

GO

INSERT INTO [PATIENT] VALUES(1401,'Patient1', 'B190600', 'Male', '0946362123', CONVERT(DATE, GETDATE()), 'Can Tho City', 'Admin', GETDATE(),'Admin', GETDATE(), 'False')
GO


/*==============================================================*/
/* Add Data: DEPARTMENT											*/
/*==============================================================*/
INSERT INTO [DEPARTMENT] VALUES('General Medicine', 'Admin', GETDATE(), 'Admin', GETDATE(), 'False')
INSERT INTO [DEPARTMENT] VALUES('Pediatrics ', 'Admin', GETDATE(), 'Admin', GETDATE(), 'False')
INSERT INTO [DEPARTMENT] VALUES('Surgery', 'Admin', GETDATE(), 'Admin', GETDATE(), 'False')
INSERT INTO [DEPARTMENT] VALUES('Cardiology', 'Admin', GETDATE(), 'Admin', GETDATE(), 'False')
INSERT INTO [DEPARTMENT] VALUES('Dermatology', 'Admin', GETDATE(), 'Admin', GETDATE(), 'False')

GO