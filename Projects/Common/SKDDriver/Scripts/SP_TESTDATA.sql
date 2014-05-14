USE [SKD]
SET DATEFORMAT dmy;
DECLARE @Uid uniqueidentifier;

DELETE FROM Organisation
delete from [dbo].[Employee]
delete from Card
delete from AccessTemplate
delete from AdditionalColumn
delete from AdditionalColumnType
delete from [dbo].[Holiday]
delete from [dbo].[Document]
delete from [dbo].[Interval]
delete from [dbo].[NamedInterval]
delete from [dbo].[Day]
delete from [dbo].[ScheduleScheme]
delete from [dbo].[Schedule]
delete from [dbo].[Position]
delete from [dbo].[Department]

DECLARE @Organisation1Uid uniqueidentifier;
SET @Organisation1Uid = 'D74D41A2-01FA-41DF-AE95-9E62A2F4BA99';
EXEC SaveOrganisation @Organisation1Uid, '����', '��� �����',0,'01/01/1900'

SET @Uid = NEWID(); 
EXEC [dbo].[SaveHoliday] @Uid, @Organisation1Uid, '����� ���', 2, '31/12/2013', '28/12/2013',0,0,'01/01/1900' 
SET @Uid = NEWID(); 
EXEC [dbo].[SaveHoliday] @Uid, @Organisation1Uid, '8 �����', 0, '08/03/2014','01/01/1900',0,0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveHoliday] @Uid, @Organisation1Uid, '������ ����� ���', 1, '13/01/2014', '01/01/1900', 7200,0,'01/01/1900'

SET @Uid = NEWID(); 
EXEC [dbo].[SaveDocument] @Uid, @Organisation1Uid, 123, '��������1', '��������1�����������1', '01/01/2013', '07/01/2013',0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDocument] @Uid, @Organisation1Uid, 258, '��������2', '��������2�����������1', '08/01/2014', '25/01/2013',0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDocument] @Uid, @Organisation1Uid, 753, '��������3', '��������3�����������1', '30/01/2014', '05/02/2013',0,'01/01/1900'

--������
DECLARE @GuardNamedIntervalUid uniqueidentifier;
SET @GuardNamedIntervalUid = NEWID();
EXEC [dbo].[SaveNamedInterval] @GuardNamedIntervalUid, @Organisation1Uid, '������'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 28800, 46800, @GuardNamedIntervalUid,0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 49500, 63900, @GuardNamedIntervalUid,0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 66600, 81000, @GuardNamedIntervalUid,0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 122400, 129600, @GuardNamedIntervalUid,0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 82800, 115200, @GuardNamedIntervalUid,0,'01/01/1900'
DECLARE @GuardScheduleSchemeUid uniqueidentifier;
SET @GuardScheduleSchemeUid = NEWID();
EXEC [dbo].[SaveScheduleScheme] @GuardScheduleSchemeUid, @Organisation1Uid, '������', 1
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @GuardNamedIntervalUid, @GuardScheduleSchemeUid, 0
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, NULL, @GuardScheduleSchemeUid, 1,0
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, NULL, @GuardScheduleSchemeUid, 2,0
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, NULL, @GuardScheduleSchemeUid, 3,0
--������
DECLARE @Montage1NamedIntervalUid uniqueidentifier;
SET @Montage1NamedIntervalUid = NEWID();
EXEC [dbo].[SaveNamedInterval] @Montage1NamedIntervalUid, @Organisation1Uid, '���������1'
DECLARE @Montage2NamedIntervalUid uniqueidentifier;
SET @Montage2NamedIntervalUid = NEWID();
EXEC [dbo].[SaveNamedInterval] @Montage2NamedIntervalUid, @Organisation1Uid, '���������2'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 27000, 39600, @Montage1NamedIntervalUid,0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 41400, 57600, @Montage1NamedIntervalUid,0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 28800, 41400, @Montage2NamedIntervalUid,0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 43200, 59400, @Montage2NamedIntervalUid,0,'01/01/1900'
DECLARE @MontageScheduleSchemeUid uniqueidentifier;
SET @MontageScheduleSchemeUid = NEWID();
EXEC [dbo].[SaveScheduleScheme] @MontageScheduleSchemeUid, @Organisation1Uid, '������', 2
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @Montage1NamedIntervalUid, @MontageScheduleSchemeUid, 0
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @Montage2NamedIntervalUid, @MontageScheduleSchemeUid, 1
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @Montage1NamedIntervalUid, @MontageScheduleSchemeUid, 2
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @Montage2NamedIntervalUid, @MontageScheduleSchemeUid, 3
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @Montage1NamedIntervalUid, @MontageScheduleSchemeUid, 4
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, NULL, @MontageScheduleSchemeUid, 5,0
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, NULL, @MontageScheduleSchemeUid, 6,0
--����������
DECLARE @WeeklyNamedIntervalUid uniqueidentifier;
SET @WeeklyNamedIntervalUid = NEWID();
EXEC [dbo].[SaveNamedInterval] @WeeklyNamedIntervalUid, @Organisation1Uid, '����������'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 28800, 43200, @WeeklyNamedIntervalUid,0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveInterval] @Uid, 46800, 61200, @WeeklyNamedIntervalUid,0,'01/01/1900'
DECLARE @WeeklyScheduleSchemeUid uniqueidentifier;
SET @WeeklyScheduleSchemeUid = NEWID();
EXEC [dbo].[SaveScheduleScheme] @WeeklyScheduleSchemeUid, @Organisation1Uid, '����������', 4
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @WeeklyNamedIntervalUid, @WeeklyScheduleSchemeUid, 0
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @WeeklyNamedIntervalUid, @WeeklyScheduleSchemeUid, 1
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @WeeklyNamedIntervalUid, @WeeklyScheduleSchemeUid, 2
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @WeeklyNamedIntervalUid, @WeeklyScheduleSchemeUid, 3
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, @WeeklyNamedIntervalUid, @WeeklyScheduleSchemeUid, 4
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, NULL, @WeeklyScheduleSchemeUid, 5,0
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDay] @Uid, NULL, @WeeklyScheduleSchemeUid, 6,0


DECLARE @GuardScheduleUid uniqueidentifier;
SET @GuardScheduleUid = NEWID();
EXEC [dbo].[SaveSchedule] @GuardScheduleUid, @Organisation1Uid, '������', @GuardScheduleSchemeUid,0,'01/01/1900'
DECLARE @MontageScheduleUid uniqueidentifier;
SET @MontageScheduleUid = NEWID();
EXEC [dbo].[SaveSchedule] @MontageScheduleUid, @Organisation1Uid, '������', @MontageScheduleSchemeUid,0,'01/01/1900' 
DECLARE @ITScheduleUid uniqueidentifier;
SET @ITScheduleUid = NEWID();
EXEC [dbo].[SaveSchedule] @ITScheduleUid , @Organisation1Uid, 'IT', @WeeklyScheduleSchemeUid,0,'01/01/1900' 
DECLARE @ConstructorshipScheduleUid uniqueidentifier;
SET @ConstructorshipScheduleUid = NEWID();
EXEC [dbo].[SaveSchedule] @ConstructorshipScheduleUid , @Organisation1Uid, '������������', @WeeklyScheduleSchemeUid,0,'01/01/1900'  
DECLARE @GovernanceScheduleUid uniqueidentifier;
SET @GovernanceScheduleUid = NEWID();
EXEC [dbo].[SaveSchedule] @GovernanceScheduleUid , @Organisation1Uid, '�����������', @WeeklyScheduleSchemeUid,0,'01/01/1900'

DECLARE @GuardPositionUid uniqueidentifier;
SET @GuardPositionUid = NEWID();
EXEC [dbo].[SavePosition] @GuardPositionUid, @Organisation1Uid, '��������', '�������������������',0,'01/01/1900'
DECLARE @MainGuardPositionUid uniqueidentifier;
SET @MainGuardPositionUid = NEWID();
EXEC [dbo].[SavePosition] @MainGuardPositionUid , @Organisation1Uid, '��. ��������', '������� �������������������',0,'01/01/1900'
DECLARE @MontagePositionUid uniqueidentifier;
SET @MontagePositionUid = NEWID();
EXEC [dbo].[SavePosition] @MontagePositionUid , @Organisation1Uid, '���������', '�������� �������',0,'01/01/1900'
DECLARE @MainMontagePositionUid uniqueidentifier;
SET @MainMontagePositionUid = NEWID();
EXEC [dbo].[SavePosition] @MainMontagePositionUid , @Organisation1Uid, '��. ���������', '������� �������� �������',0,'01/01/1900'
DECLARE @ProgrammerPositionUid uniqueidentifier;
SET @ProgrammerPositionUid = NEWID();
EXEC [dbo].[SavePosition] @ProgrammerPositionUid , @Organisation1Uid, '�����������', '����������� ���������� ��� ���',0,'01/01/1900'
DECLARE @MainProgrammerPositionUid uniqueidentifier;
SET @MainProgrammerPositionUid = NEWID();
EXEC [dbo].[SavePosition] @MainProgrammerPositionUid , @Organisation1Uid, '��. �����������', '������� ����������� ���������� ��� ���',0,'01/01/1900'
DECLARE @TesterPositionUid uniqueidentifier;
SET @TesterPositionUid = NEWID();
EXEC [dbo].[SavePosition] @TesterPositionUid , @Organisation1Uid, '�����������', '���������� ���������� ��� ���',0,'01/01/1900'
DECLARE @MainTesterPositionUid uniqueidentifier;
SET @MainTesterPositionUid = NEWID();
EXEC [dbo].[SavePosition] @MainTesterPositionUid , @Organisation1Uid, '��. �����������', '������� ���������� ���������� ��� ���',0,'01/01/1900'
DECLARE @ConstructorPositionUid uniqueidentifier;
SET @ConstructorPositionUid = NEWID();
EXEC [dbo].[SavePosition] @ConstructorPositionUid , @Organisation1Uid, '�����������', '�������-�����������',0,'01/01/1900'
DECLARE @MainConstructorPositionUid uniqueidentifier;
SET @MainConstructorPositionUid = NEWID();
EXEC [dbo].[SavePosition] @MainConstructorPositionUid , @Organisation1Uid, '��. �����������', '������� �������-�����������',0,'01/01/1900'
DECLARE @ProgrammistConstructorPositionUid uniqueidentifier;
SET @ProgrammistConstructorPositionUid = NEWID();
EXEC [dbo].[SavePosition] @ProgrammistConstructorPositionUid , @Organisation1Uid, '�����������-�����������', '�������������� � ��������� ��������',0,'01/01/1900'
DECLARE @AdministratorPositionUid uniqueidentifier;
SET @AdministratorPositionUid = NEWID();
EXEC [dbo].[SavePosition] @AdministratorPositionUid , @Organisation1Uid, '�������������', '������� �������������',0,'01/01/1900'
DECLARE @DirectorPositionUid uniqueidentifier;
SET @DirectorPositionUid = NEWID();
EXEC [dbo].[SavePosition] @DirectorPositionUid , @Organisation1Uid, '��������', '������������ ��������',0,'01/01/1900'


DECLARE @photo1 varbinary(MAX);
SET @photo1 = (SELECT * FROM OPENROWSET(BULK N'C:\image1.jpg', SINGLE_BLOB) as _file);

DECLARE @photo2 varbinary(MAX);
SET @photo2 = (SELECT * FROM OPENROWSET(BULK N'C:\image2.jpg', SINGLE_BLOB) as _file);


DECLARE @PhotoUID uniqueidentifier;


DECLARE @Guard1EmployeeUid uniqueidentifier;
SET @Guard1EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Guard1EmployeeUid , @Organisation1Uid, '������', '��������', '������', @GuardPositionUid, null , @GuardScheduleUid, '12/05/2005','01/01/1900',0,'01/01/1900'
SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @photo1 
UPDATE [dbo].[Employee] SET PhotoUID=@PhotoUID WHERE UID = @Guard1EmployeeUid
DECLARE @Guard2EmployeeUid uniqueidentifier;
SET @Guard2EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Guard2EmployeeUid , @Organisation1Uid, '����', '���������', '����������', @GuardPositionUid, null , @GuardScheduleUid, '12/05/2006','01/01/1900',0,'01/01/1900'
DECLARE @MainGuardEmployeeUid uniqueidentifier;
SET @MainGuardEmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @MainGuardEmployeeUid , @Organisation1Uid, '����', '���������', '����������', @MainGuardPositionUid, null , @GuardScheduleUid, '12/05/2007','01/01/1900',0,'01/01/1900'

DECLARE @Montage1EmployeeUid uniqueidentifier;
SET @Montage1EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Montage1EmployeeUid ,  @Organisation1Uid,'����', '���������', '������', @MontagePositionUid, null , @MontageScheduleUid, '12/05/2008','01/01/1900',0,'01/01/1900'
DECLARE @Montage2EmployeeUid uniqueidentifier;
SET @Montage2EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Montage2EmployeeUid , @Organisation1Uid, '������', '��������', '������������', @MontagePositionUid, null , @MontageScheduleUid, '12/05/2009','01/01/1900',0,'01/01/1900'
DECLARE @MainMontageEmployeeUid uniqueidentifier;
SET @MainMontageEmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @MainMontageEmployeeUid , @Organisation1Uid, '������', '���������', '����������', @MainMontagePositionUid, null , @MontageScheduleUid, '12/05/2010','01/01/1900',0,'01/01/1900'

DECLARE @Programmer1EmployeeUid uniqueidentifier;
SET @Programmer1EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Programmer1EmployeeUid , @Organisation1Uid, '����', '��������', '�������', @ProgrammerPositionUid, null , @ITScheduleUid, '12/05/2011','01/01/1900',0,'01/01/1900'
DECLARE @Programmer2EmployeeUid uniqueidentifier;
SET @Programmer2EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Programmer2EmployeeUid , @Organisation1Uid, '����', '�������', '���������', @ProgrammerPositionUid, null , @ITScheduleUid, '12/05/2012','01/01/1900',0,'01/01/1900'
DECLARE @MainProgrammistEmployeeUid uniqueidentifier;
SET @MainProgrammistEmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @MainProgrammistEmployeeUid , @Organisation1Uid, '����', '��������', '���������', @MainProgrammerPositionUid, null , @ITScheduleUid, '12/05/2013','01/01/1900',0,'01/01/1900'

DECLARE @Tester1EmployeeUid uniqueidentifier;
SET @Tester1EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Tester1EmployeeUid , @Organisation1Uid, '�����', '����������', '��������', @TesterPositionUid, null , @ITScheduleUid, '12/06/2013','01/01/1900',0,'01/01/1900'
DECLARE @Tester2EmployeeUid uniqueidentifier;
SET @Tester2EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Tester2EmployeeUid , @Organisation1Uid, '������', '���������', '�������', @TesterPositionUid, null , @ITScheduleUid, '12/07/2013','01/01/1900',0,'01/01/1900'
DECLARE @MainTesterEmployeeUid uniqueidentifier;
SET @MainTesterEmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @MainTesterEmployeeUid , @Organisation1Uid, '������', '����������', '���������', @MainTesterPositionUid, null , @ITScheduleUid, '12/08/2013','01/01/1900',0,'01/01/1900'

DECLARE @Constructor1EmployeeUid uniqueidentifier;
SET @Constructor1EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Constructor1EmployeeUid , @Organisation1Uid, '�����', '���������', '��������', @ConstructorPositionUid, null , @ConstructorshipScheduleUid, '12/09/2013','01/01/1900',0,'01/01/1900'
DECLARE @Constructor2EmployeeUid uniqueidentifier;
SET @Constructor2EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Constructor2EmployeeUid , @Organisation1Uid, '�����', '���������', '����������', @ConstructorPositionUid, null , @ConstructorshipScheduleUid, '12/10/2013','01/01/1900',0,'01/01/1900'
DECLARE @MainConstructorEmployeeUid uniqueidentifier;
SET @MainConstructorEmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @MainConstructorEmployeeUid , @Organisation1Uid, '�����', '���������', '����������', @MainConstructorPositionUid, null , @ConstructorshipScheduleUid, '12/11/2013','01/01/1900',0,'01/01/1900'

DECLARE @AdministratorEmployeeUid uniqueidentifier;
SET @AdministratorEmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @AdministratorEmployeeUid , @Organisation1Uid, '�����', '����������', '�������', @AdministratorPositionUid, null , @GovernanceScheduleUid, '12/12/2013','01/01/1900',0,'01/01/1900'
DECLARE @DirectorEmployeeUid uniqueidentifier;
SET @DirectorEmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @DirectorEmployeeUid , @Organisation1Uid, '������', '���������', '���������', @DirectorEmployeeUid , null , @GovernanceScheduleUid, '13/12/2013','01/01/1900',0,'01/01/1900'
DECLARE @ProgrammistConstructorEmployeeUid uniqueidentifier;
SET @ProgrammistConstructorEmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @ProgrammistConstructorEmployeeUid , @Organisation1Uid, '�������', '�������', '������', @ProgrammistConstructorPositionUid , null , @ConstructorshipScheduleUid, '13/12/2001','01/01/1900',0,'01/01/1900'
SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @photo2
UPDATE [dbo].[Employee] SET PhotoUID=@PhotoUID WHERE UID = @ProgrammistConstructorEmployeeUid

DECLARE @MainDepartmentUid uniqueidentifier;
SET @MainDepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @MainDepartmentUid , @Organisation1Uid, '��� "�����"', '����������� ����� 3 ������� ���������', null , @DirectorEmployeeUid, @AdministratorEmployeeUid,0,'01/01/1900'
DECLARE @GovernanceDepartmentUid uniqueidentifier;
SET @GovernanceDepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @GovernanceDepartmentUid ,  @Organisation1Uid,'�����������', '��������������� � ������ �����������', @MainDepartmentUid , @AdministratorEmployeeUid, @DirectorEmployeeUid ,0,'01/01/1900'
DECLARE @GuardDepartmentUid uniqueidentifier;
SET @GuardDepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @GuardDepartmentUid , @Organisation1Uid, '������', '���������� � ������������', @MainDepartmentUid , @MainGuardEmployeeUid, @Guard1EmployeeUid,0,'01/01/1900' 
DECLARE @MontageDepartmentUid uniqueidentifier;
SET @MontageDepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @MontageDepartmentUid , @Organisation1Uid, '������', '���������� � �����������', @MainDepartmentUid , @MainMontageEmployeeUid, @Montage1EmployeeUid,0,'01/01/1900' 
DECLARE @EngeneeringDepartmentUid uniqueidentifier;
SET @EngeneeringDepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @EngeneeringDepartmentUid , @Organisation1Uid, '��������', '��������� � ���� �����������', @MainDepartmentUid , @ProgrammistConstructorEmployeeUid,NULL,0,'01/01/1900' 
DECLARE @ITDepartmentUid uniqueidentifier;
SET @ITDepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @ITDepartmentUid , @Organisation1Uid, 'IT', '����������������� � ������������', @EngeneeringDepartmentUid , @MainProgrammistEmployeeUid, @MainTesterEmployeeUid,0,'01/01/1900' 
DECLARE @ProgrammerDepartmentUid uniqueidentifier;
SET @ProgrammerDepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @ProgrammerDepartmentUid , @Organisation1Uid, '������������', '�����������������', @ITDepartmentUid , @MainProgrammistEmployeeUid, @Programmer1EmployeeUid,0,'01/01/1900' 
DECLARE @TesterDepartmentUid uniqueidentifier;
SET @TesterDepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @TesterDepartmentUid , @Organisation1Uid, '������������', '���� ���������� �����������', @ITDepartmentUid , @MainTesterEmployeeUid , @Tester1EmployeeUid,0,'01/01/1900' 
DECLARE @ConstructorshipDepartmentUid uniqueidentifier;
SET @ConstructorshipDepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @ConstructorshipDepartmentUid , @Organisation1Uid, '������������', '����� ��������� ����������', @EngeneeringDepartmentUid , @MainConstructorEmployeeUid , @Constructor1EmployeeUid,0,'01/01/1900'

UPDATE [dbo].[Employee] SET [DepartmentUid]=@GovernanceDepartmentUid WHERE [Uid]=@DirectorEmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@GovernanceDepartmentUid WHERE [Uid]=@AdministratorEmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@GuardDepartmentUid WHERE [Uid]=@Guard1EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@GuardDepartmentUid WHERE [Uid]=@Guard2EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@GuardDepartmentUid WHERE [Uid]=@MainGuardEmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@MontageDepartmentUid WHERE [Uid]=@Montage1EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@MontageDepartmentUid WHERE [Uid]=@Montage2EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@MontageDepartmentUid WHERE [Uid]=@MainMontageEmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@EngeneeringDepartmentUid WHERE [Uid]=@ProgrammistConstructorEmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@ProgrammerDepartmentUid WHERE [Uid]=@Programmer1EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@ProgrammerDepartmentUid WHERE [Uid]=@Programmer2EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@ProgrammerDepartmentUid WHERE [Uid]=@MainProgrammistEmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@TesterDepartmentUid WHERE [Uid]=@Tester1EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@TesterDepartmentUid WHERE [Uid]=@Tester2EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@TesterDepartmentUid WHERE [Uid]=@MainTesterEmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@ConstructorshipDepartmentUid WHERE [Uid]=@Constructor1EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@ConstructorshipDepartmentUid WHERE [Uid]=@Constructor2EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@ConstructorshipDepartmentUid WHERE [Uid]=@MainConstructorEmployeeUid

DECLARE @Guest1UID uniqueidentifier;
SET @Guest1UID = NEWID(); 
EXEC [dbo].[SaveGuest] @Guest1UID, @Organisation1Uid, '�������', '�����������', '��������',0,'01/01/1900'
DECLARE @Guest2UID uniqueidentifier;
SET @Guest2UID = NEWID(); 
EXEC [dbo].[SaveGuest] @Guest2UID, @Organisation1Uid, '������', '�����������', '�����',0,'01/01/1900'

DECLARE @MontageAccessTemplateUID uniqueidentifier;
SET @MontageAccessTemplateUID = NEWID();
EXEC SaveAccessTemplate @MontageAccessTemplateUID, @Organisation1Uid, '������', '������ ���',0,'01/01/1900'
DECLARE @GuardAccessTemplateUID uniqueidentifier;
SET @GuardAccessTemplateUID = NEWID();
EXEC SaveAccessTemplate @GuardAccessTemplateUID, @Organisation1Uid, '������', '������ ���',0,'01/01/1900'
DECLARE @GovernanceAccessTemplateUID uniqueidentifier;
SET @GovernanceAccessTemplateUID = NEWID();
EXEC SaveAccessTemplate @GovernanceAccessTemplateUID, @Organisation1Uid, '�����������', '����������� ���',0,'01/01/1900'
DECLARE @ConstructorshipAccessTemplateUID uniqueidentifier;
SET @ConstructorshipAccessTemplateUID = NEWID();
EXEC SaveAccessTemplate @ConstructorshipAccessTemplateUID, @Organisation1Uid, '������������', '������������ ���',0,'01/01/1900'
DECLARE @ITAccessTemplateUID uniqueidentifier;
SET @ITAccessTemplateUID = NEWID();
EXEC SaveAccessTemplate @ITAccessTemplateUID, @Organisation1Uid, '�� ��', '�� �� ���',0,'01/01/1900'
DECLARE @FullAccessTemplateUID uniqueidentifier;
SET @FullAccessTemplateUID = NEWID();
EXEC SaveAccessTemplate @FullAccessTemplateUID, @Organisation1Uid, '������ ������', '������ ������ ���',0,'01/01/1900'

SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 0, 1, @Montage1EmployeeUid, @MontageAccessTemplateUID, '01/01/2014', '01/01/2015', 0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 1, 2, @Montage2EmployeeUid, @MontageAccessTemplateUID, '01/01/2014', '01/01/2015', 0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 2, 3, @MainMontageEmployeeUid, @MontageAccessTemplateUID, '01/01/2014', '01/01/2015',0

SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 3, 4, @Guard1EmployeeUid, @GuardAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 4, 5, @Guard2EmployeeUid, @GuardAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 5, 6, @MainGuardEmployeeUid, @GuardAccessTemplateUID, '01/01/2014', '01/01/2015',0

SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 6, 7, @Constructor1EmployeeUid, @ConstructorshipAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 7, 8, @Constructor2EmployeeUid, @ConstructorshipAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 8, 9, @MainConstructorEmployeeUid, @ConstructorshipAccessTemplateUID, '01/01/2014', '01/01/2015',0

SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 10, 11, @Programmer1EmployeeUid, @ITAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 11, 12, @Programmer2EmployeeUid, @ITAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 12, 13, @MainProgrammistEmployeeUid, @ITAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 13, 14, @Tester1EmployeeUid, @ITAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 14, 15, @Tester2EmployeeUid, @ITAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 15, 16, @MainTesterEmployeeUid, @ITAccessTemplateUID, '01/01/2014', '01/01/2015',0

SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 16, 17, @DirectorEmployeeUid, @FullAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 17, 18, @AdministratorEmployeeUid, @GovernanceAccessTemplateUID, '01/01/2014', '01/01/2015',0
SET @Uid = NEWID();
EXEC SaveCard @Uid, 0, '01/01/1900', 18, 19, @ProgrammistConstructorEmployeeUid, NULL, '01/01/2014', '01/01/2015',0

DECLARE @PassportAdditionalColumnTypeUID uniqueidentifier;
SET @PassportAdditionalColumnTypeUID = NEWID();
EXEC SaveAdditionalColumnType @PassportAdditionalColumnTypeUID, @Organisation1Uid, '���� ��������', '����������� ������ �������� ��������', 1, 0, 0, '01/01/1900'

DECLARE @CharacteristicsAdditionalColumnTypeUID uniqueidentifier;
SET @CharacteristicsAdditionalColumnTypeUID = NEWID();
EXEC SaveAdditionalColumnType @CharacteristicsAdditionalColumnTypeUID , @Organisation1Uid, '��������������', '������ ��������������', 0, 0, 0, '01/01/1900'

DECLARE @GuestTypeAdditionalColumnTypeUID uniqueidentifier;
SET @GuestTypeAdditionalColumnTypeUID = NEWID();
EXEC SaveAdditionalColumnType @GuestTypeAdditionalColumnTypeUID , @Organisation1Uid, '���', '��� ����������', 0, 1, 0, '01/01/1900'

DECLARE @PassportScan varbinary(MAX);
SET @PassportScan = (SELECT * FROM OPENROWSET(BULK N'C:\passportDesu.jpg', SINGLE_BLOB) as _file);

DECLARE @Characteristics nvarchar(MAX);
SET @Characteristics = '������ ��������������';

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Montage1EmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Montage2EmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @MainMontageEmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Guard1EmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Guard2EmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @MainGuardEmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Constructor1EmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Constructor2EmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @MainConstructorEmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Programmer1EmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Programmer2EmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @MainProgrammistEmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Tester1EmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Tester2EmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @MainTesterEmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @DirectorEmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @AdministratorEmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @PhotoUID = NEWID();
EXEC SavePhoto @PhotoUID, @PassportScan 
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @ProgrammistConstructorEmployeeUid, @PassportAdditionalColumnTypeUID, NULL, @PhotoUID

SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Montage1EmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Montage2EmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @MainMontageEmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Guard1EmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Guard2EmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @MainGuardEmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Constructor1EmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Constructor2EmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @MainConstructorEmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Programmer1EmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Programmer2EmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @MainProgrammistEmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Tester1EmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Tester2EmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @MainTesterEmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @DirectorEmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @AdministratorEmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @ProgrammistConstructorEmployeeUid, @CharacteristicsAdditionalColumnTypeUID, @Characteristics

SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Guest1UID, @GuestTypeAdditionalColumnTypeUID, '�����������'
SET @Uid = NEWID();
EXEC SaveAdditionalColumn @Uid, @Guest2UID, @GuestTypeAdditionalColumnTypeUID, '������'

DECLARE @Organisation2Uid uniqueidentifier;
SET @Organisation2Uid = '498F0C15-76E1-40D5-836E-908F638177AF';
EXEC SaveOrganisation @Organisation2Uid, 'McDonalds', 'McDonalds Restaurants Inc',0,'01/01/1900'

DECLARE @JanitorPositionUid uniqueidentifier;
SET @JanitorPositionUid = NEWID();
EXEC [dbo].[SavePosition] @JanitorPositionUid, @Organisation2Uid, '�������', '�������������������',0,'01/01/1900'
DECLARE @KitchenerPositionUid uniqueidentifier;
SET @KitchenerPositionUid = NEWID();
EXEC [dbo].[SavePosition] @KitchenerPositionUid, @Organisation2Uid, '�������', '�������� ��������',0,'01/01/1900'
DECLARE @ManagerPositionUid uniqueidentifier;
SET @ManagerPositionUid = NEWID();
EXEC [dbo].[SavePosition] @ManagerPositionUid , @Organisation2Uid, '��������', '�����������',0,'01/01/1900'

DECLARE @Janitor1EmployeeUid uniqueidentifier;
SET @Janitor1EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Janitor1EmployeeUid , @Organisation2Uid, '����', '��������', '���������', @JanitorPositionUid , null , null, '12/05/1995','01/01/1900',0,'01/01/1900'
DECLARE @Janitor2EmployeeUid uniqueidentifier;
SET @Janitor2EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Janitor2EmployeeUid , @Organisation2Uid, '����', '��������', '������������', @JanitorPositionUid , null , null, '12/05/1996','01/01/1900',0,'01/01/1900'
DECLARE @Janitor3EmployeeUid uniqueidentifier;
SET @Janitor3EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Janitor3EmployeeUid , @Organisation2Uid, '������', '���������', '��������', @JanitorPositionUid , null , null, '12/05/1997','01/01/1900',0,'01/01/1900'
DECLARE @Janitor4EmployeeUid uniqueidentifier;
SET @Janitor4EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Janitor4EmployeeUid , @Organisation2Uid, '������', '���������', '���������������', @JanitorPositionUid , null , null, '12/05/1998','01/01/1900',0,'01/01/1900'
DECLARE @Kitchener1EmployeeUid uniqueidentifier;
SET @Kitchener1EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Kitchener1EmployeeUid , @Organisation2Uid, '����', '��������', '��������', @KitchenerPositionUid , null , null, '12/05/1995','01/01/1900',0,'01/01/1900'
DECLARE @Kitchener2EmployeeUid uniqueidentifier;
SET @Kitchener2EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Kitchener2EmployeeUid , @Organisation2Uid, '����', '��������', '����������', @KitchenerPositionUid , null , null, '12/06/1995','01/01/1900',0,'01/01/1900'
DECLARE @Kitchener3EmployeeUid uniqueidentifier;
SET @Kitchener3EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Kitchener3EmployeeUid , @Organisation2Uid, '������', '���������', '��������', @KitchenerPositionUid , null , null, '12/07/1995','01/01/1900',0,'01/01/1900'
DECLARE @Kitchener4EmployeeUid uniqueidentifier;
SET @Kitchener4EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Kitchener4EmployeeUid , @Organisation2Uid, '������', '���������', '����������', @KitchenerPositionUid , null , null, '12/08/1995','01/01/1900',0,'01/01/1900'
DECLARE @Manager1EmployeeUid uniqueidentifier;
SET @Manager1EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Manager1EmployeeUid , @Organisation2Uid, '����', '���������', '�����������', @ManagerPositionUid , null , null, '22/08/1999','01/01/1900',0,'01/01/1900'
DECLARE @Manager2EmployeeUid uniqueidentifier;
SET @Manager2EmployeeUid = NEWID();
EXEC [dbo].[SaveEmployee] @Manager2EmployeeUid , @Organisation2Uid, '����', '���������', '�����������', @ManagerPositionUid , null , null, '31/12/2000','01/01/1900',0,'01/01/1900'

DECLARE @Restaurant1DepartmentUid uniqueidentifier;
SET @Restaurant1DepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @Restaurant1DepartmentUid , @Organisation2Uid, '��. ������', '������ ���� �������', null , @Manager1EmployeeUid, @Kitchener1EmployeeUid,0,'01/01/1900'
DECLARE @Janitors1DepartmentUid uniqueidentifier;
SET @Janitors1DepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @Janitors1DepartmentUid , @Organisation2Uid, '��������', '�������� �� ������', @Restaurant1DepartmentUid,NULL,NULL,0,'01/01/1900'
DECLARE @Kitcheners1DepartmentUid uniqueidentifier;
SET @Kitcheners1DepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @Kitcheners1DepartmentUid , @Organisation2Uid, '�������', '������� �� ������', @Restaurant1DepartmentUid,NULL,NULL,0,'01/01/1900'
DECLARE @Management1DepartmentUid uniqueidentifier;
SET @Management1DepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @Management1DepartmentUid , @Organisation2Uid, '����������', '���������� �� ������', @Restaurant1DepartmentUid,NULL,NULL,0,'01/01/1900'
UPDATE [dbo].[Employee] SET [DepartmentUid]=@Management1DepartmentUid WHERE [Uid]=@Manager1EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@Kitcheners1DepartmentUid WHERE [Uid]=@Kitchener1EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@Kitcheners1DepartmentUid WHERE [Uid]=@Kitchener2EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@Janitors1DepartmentUid WHERE [Uid]=@Janitor1EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@Janitors1DepartmentUid WHERE [Uid]=@Janitor2EmployeeUid

DECLARE @Restaurant2DepartmentUid uniqueidentifier;
SET @Restaurant2DepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @Restaurant2DepartmentUid , @Organisation2Uid, '��. �������������', '������������� ����� "�����"', null , @Manager2EmployeeUid, @Kitchener2EmployeeUid,0,'01/01/1900'
DECLARE @Janitors2DepartmentUid uniqueidentifier;
SET @Janitors2DepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @Janitors2DepartmentUid , @Organisation2Uid, '��������', '�������� �� �������������', @Restaurant2DepartmentUid,NULL,NULL,0,'01/01/1900'
DECLARE @Kitcheners2DepartmentUid uniqueidentifier;
SET @Kitcheners2DepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @Kitcheners2DepartmentUid , @Organisation2Uid, '�������', '������� �� �������������', @Restaurant2DepartmentUid,NULL,NULL,0,'01/01/1900'
DECLARE @Management2DepartmentUid uniqueidentifier;
SET @Management2DepartmentUid = NEWID();
EXEC [dbo].[SaveDepartment] @Management1DepartmentUid , @Organisation2Uid, '����������', '���������� �� �������������', @Restaurant2DepartmentUid,NULL,NULL,0,'01/01/1900'
UPDATE [dbo].[Employee] SET [DepartmentUid]=@Management2DepartmentUid WHERE [Uid]=@Manager2EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@Kitcheners2DepartmentUid WHERE [Uid]=@Kitchener3EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@Kitcheners2DepartmentUid WHERE [Uid]=@Kitchener4EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@Janitors2DepartmentUid WHERE [Uid]=@Janitor3EmployeeUid
UPDATE [dbo].[Employee] SET [DepartmentUid]=@Janitors2DepartmentUid WHERE [Uid]=@Janitor4EmployeeUid

SET @UID = NEWID();
EXEC [dbo].[SaveEmployeeReplacement] @UID, @Organisation2Uid, '01/01/1900', '01/01/9000', @Janitor2EmployeeUid, @Janitors2DepartmentUid, NULL, 0,'01/01/1900'

SET @Uid = NEWID(); 
EXEC [dbo].[SaveGuest] @Uid, @Organisation2Uid, '��������', '�������������', '�����������',0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveGuest] @Uid, @Organisation2Uid, '�����', '���������', '���������',0,'01/01/1900'

SET @Uid = NEWID(); 
EXEC [dbo].[SaveDocument] @Uid, @Organisation2Uid, 486, '��������1', '��������1�����������2', '01/01/2013', '07/01/2013',0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDocument] @Uid, @Organisation2Uid, 729, '��������2', '��������2�����������2', '08/01/2014', '25/01/2013',0,'01/01/1900'
SET @Uid = NEWID(); 
EXEC [dbo].[SaveDocument] @Uid, @Organisation2Uid, 123, '��������3', '��������3�����������2', '30/01/2014', '05/02/2013',0,'01/01/1900'

