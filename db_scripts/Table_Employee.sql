USE master;
GO

DROP DATABASE [AbnAssessment_Payroll];
GO

CREATE DATABASE [AbnAssessment_Payroll];
GO
USE [AbnAssessment_Payroll];
GO

CREATE TABLE [Employee](
	[Id] INT IDENTITY(1,1) NOT NULL,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[BirthDate] DATE NOT NULL,
	[JobTitle] NVARCHAR(50) NOT NULL,
	[JoiningDate] DATE NOT NULL,
	[LeavingDate] DATE NULL,
	[SalariedFlag] BIT NOT NULL,
	[VacationHours] SMALLINT NOT NULL,
	[SickLeaveHours] SMALLINT NOT NULL,
	[CurrentFlag] BIT NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[ModifiedDate] DATETIME	NULL,
	CONSTRAINT [PK_Employee_Id] PRIMARY KEY CLUSTERED ([Id])
);
GO
ALTER TABLE [Employee] ADD  CONSTRAINT [DF_Employee_SalariedFlag]  DEFAULT ((1)) FOR [SalariedFlag]
GO

ALTER TABLE [Employee] ADD  CONSTRAINT [DF_Employee_VacationHours]  DEFAULT ((0)) FOR [VacationHours]
GO

ALTER TABLE [Employee] ADD  CONSTRAINT [DF_Employee_SickLeaveHours]  DEFAULT ((0)) FOR [SickLeaveHours]
GO

ALTER TABLE [Employee] ADD  CONSTRAINT [DF_Employee_CurrentFlag]  DEFAULT ((1)) FOR [CurrentFlag]
GO

ALTER TABLE [Employee] ADD  CONSTRAINT [DF_Employee_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [Employee]  WITH CHECK ADD  CONSTRAINT [CK_Employee_BirthDate] CHECK  (([BirthDate]>=dateadd(year,(-60),getdate()) AND [BirthDate]<=dateadd(year,(-18),getdate())))
GO

ALTER TABLE [Employee] CHECK CONSTRAINT [CK_Employee_BirthDate]
GO

ALTER TABLE [Employee]  WITH CHECK ADD  CONSTRAINT [CK_Employee_HireDate] CHECK  (([HireDate]>='1991-09-21' AND [HireDate]<=dateadd(day,(1),getdate())))
GO

ALTER TABLE [Employee] CHECK CONSTRAINT [CK_Employee_HireDate]
GO

ALTER TABLE [Employee]  WITH CHECK ADD  CONSTRAINT [CK_Employee_SickLeaveHours] CHECK  (([SickLeaveHours]>=(0) AND [SickLeaveHours]<=(120)))
GO

ALTER TABLE [Employee] CHECK CONSTRAINT [CK_Employee_SickLeaveHours]
GO

ALTER TABLE [Employee]  WITH CHECK ADD  CONSTRAINT [CK_Employee_VacationHours] CHECK  (([VacationHours]>=(-40) AND [VacationHours]<=(240)))
GO

ALTER TABLE [Employee] CHECK CONSTRAINT [CK_Employee_VacationHours]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for Employee records.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'First name of the employee.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'FirstName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last name of the person..' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'LastName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of birth.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'BirthDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Job title such as Developer or Administrative Assisstant.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'JobTitle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Employee hired on this date.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'JoiningDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Employee leaved on this date.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'LeavingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Job classification. 0 = Hourly, not exempt from collective bargaining. 1 = Salaried, exempt from collective bargaining.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'SalariedFlag'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Default constraint value of 1 (TRUE)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'CONSTRAINT',@level2name=N'DF_Employee_SalariedFlag'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of available vacation hours.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'VacationHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Default constraint value of 0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'CONSTRAINT',@level2name=N'DF_Employee_VacationHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of available sick leave hours.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'SickLeaveHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Default constraint value of 0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'CONSTRAINT',@level2name=N'DF_Employee_SickLeaveHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time the record was created.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'CreatedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Default constraint value of GETDATE()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'CONSTRAINT',@level2name=N'DF_Employee_CreatedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time the record was last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'COLUMN',@level2name=N'ModifiedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key (clustered) constraint' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'CONSTRAINT',@level2name=N'PK_Employee_Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Employee information such as salary, and title.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check constraint [BirthDate] >= dateadd(year,(-60),GETDATE()) AND [BirthDate] <= dateadd(year,(-18),GETDATE())' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'CONSTRAINT',@level2name=N'CK_Employee_BirthDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check constraint [HireDate] >= ''1991-09-21'' (ABN AMRO Founded in this date) AND [HireDate] <= dateadd(day,(1),GETDATE())' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'CONSTRAINT',@level2name=N'CK_Employee_HireDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check constraint [SickLeaveHours] >= (0) AND [SickLeaveHours] <= (120)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'CONSTRAINT',@level2name=N'CK_Employee_SickLeaveHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check constraint [VacationHours] >= (-40) AND [VacationHours] <= (240)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Employee', @level2type=N'CONSTRAINT',@level2name=N'CK_Employee_VacationHours'
GO

