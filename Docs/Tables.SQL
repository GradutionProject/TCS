USE TCS
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_LocationStatus_Location') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE LocationStatus DROP CONSTRAINT FK_LocationStatus_Location
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_LocationStatus_SensorData') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE LocationStatus DROP CONSTRAINT FK_LocationStatus_SensorData
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_LocationStatusHistory_Location') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE LocationStatusHistory DROP CONSTRAINT FK_LocationStatusHistory_Location
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_LocationStatusHistory_SensorData') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE LocationStatusHistory DROP CONSTRAINT FK_LocationStatusHistory_SensorData
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_SensorData_Sensors') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE SensorData DROP CONSTRAINT FK_SensorData_Sensors
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_Sensors_Location') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE Sensors DROP CONSTRAINT FK_Sensors_Location
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_Sensors_SensorType') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE Sensors DROP CONSTRAINT FK_Sensors_SensorType
;


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('Location') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE Location
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('LocationStatus') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE LocationStatus
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('LocationStatusHistory') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE LocationStatusHistory
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('SensorData') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE SensorData
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('Sensors') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE Sensors
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('SensorType') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE SensorType
;


CREATE TABLE Location ( 
	LocationId nvarchar(128) NOT NULL,
	Name nvarchar(512) NOT NULL,
	Longitude decimal(38,10) NOT NULL,
	Latitude decimal(38,10) NOT NULL,
	SpatialReference int NOT NULL
)
;

CREATE TABLE LocationStatus ( 
	LocationStatusId nvarchar(128) NOT NULL,
	Count int NOT NULL,
	ModificationData datetime NOT NULL,
	LocationId nvarchar(128) NOT NULL,
	SensorDataId nvarchar(128) NOT NULL
)
;

CREATE TABLE LocationStatusHistory ( 
	LocationStatusHistoryId nvarchar(128) NOT NULL,
	LocationId nvarchar(128) NOT NULL,
	Count int NOT NULL,
	SensorDataId nvarchar(128) NOT NULL,
	Time datetime NOT NULL
)
;

CREATE TABLE SensorData ( 
	SensorDataId nvarchar(128) NOT NULL,
	Time datetime NOT NULL,
	SensorId nvarchar(128) NOT NULL
)
;

CREATE TABLE Sensors ( 
	SensorId nvarchar(128) NOT NULL,
	Name nvarchar(512) NOT NULL,
	TypeId nvarchar(128) NOT NULL,
	LocationId nvarchar(128) NOT NULL,
	InputOrOutput bit NOT NULL,
	Longitude decimal(38,10) NOT NULL,
	Latitude decimal(38,10) NOT NULL,
	SpatialReference int NOT NULL
)
;

CREATE TABLE SensorType ( 
	TypeId nvarchar(128) NOT NULL,
	Name nvarchar(512) NOT NULL
)
;


ALTER TABLE Location ADD CONSTRAINT PK_Location 
	PRIMARY KEY CLUSTERED (LocationId)
;

ALTER TABLE LocationStatus ADD CONSTRAINT PK_LocationStatus 
	PRIMARY KEY CLUSTERED (LocationStatusId)
;

ALTER TABLE LocationStatusHistory ADD CONSTRAINT PK_LocationStatusHistory 
	PRIMARY KEY CLUSTERED (LocationStatusHistoryId)
;

ALTER TABLE SensorData ADD CONSTRAINT PK_SensorData 
	PRIMARY KEY CLUSTERED (SensorDataId)
;

ALTER TABLE Sensors ADD CONSTRAINT PK_Sensors 
	PRIMARY KEY CLUSTERED (SensorId)
;

ALTER TABLE SensorType ADD CONSTRAINT PK_SensorType 
	PRIMARY KEY CLUSTERED (TypeId)
;



ALTER TABLE LocationStatus ADD CONSTRAINT FK_LocationStatus_Location 
	FOREIGN KEY (LocationId) REFERENCES Location (LocationId)
;

ALTER TABLE LocationStatus ADD CONSTRAINT FK_LocationStatus_SensorData 
	FOREIGN KEY (SensorDataId) REFERENCES SensorData (SensorDataId)
;

ALTER TABLE LocationStatusHistory ADD CONSTRAINT FK_LocationStatusHistory_Location 
	FOREIGN KEY (LocationId) REFERENCES Location (LocationId)
;

ALTER TABLE LocationStatusHistory ADD CONSTRAINT FK_LocationStatusHistory_SensorData 
	FOREIGN KEY (SensorDataId) REFERENCES SensorData (SensorDataId)
;

ALTER TABLE SensorData ADD CONSTRAINT FK_SensorData_Sensors 
	FOREIGN KEY (SensorId) REFERENCES Sensors (SensorId)
;

ALTER TABLE Sensors ADD CONSTRAINT FK_Sensors_Location 
	FOREIGN KEY (LocationId) REFERENCES Location (LocationId)
;

ALTER TABLE Sensors ADD CONSTRAINT FK_Sensors_SensorType 
	FOREIGN KEY (TypeId) REFERENCES SensorType (TypeId)
;
