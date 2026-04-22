Create table Country
(
	CountryId Serial Primary Key
	,CountryName Text Not Null
	,IsActive	Bool Not Null Default(true)
	,IsDeleted	Bool Not Null Default(false)
	,CreatedDate	Timestamp Not Null Default(now())
	,LastUpdateDate	Timestamp Not Null Default(now())
	,CreatedBy	Int Not Null Default(0)
	,LastUpdateBy	Int Not Null Default(0)
);

Create table State
(
	StateId Serial Primary key
	,CountryId Int not null
	,StateName Text Not Null
	,IsActive	Bool Not Null Default(true)
	,IsDeleted	Bool Not Null Default(false)
	,CreatedDate	Timestamp Not Null Default(now())
	,LastUpdateDate	Timestamp Not Null Default(now())
	,CreatedBy	Int Not Null Default(0)
	,LastUpdateBy	Int Not Null Default(0)
);


INSERT INTO public."role"
(rolename, isactive, isdeleted, createddate, lastupdatedate, createdby, lastupdateby)
VALUES('Admin', true, false, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 0, 0)
,('HR', true, false, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 0, 0)
,('DA', true, false, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 0, 0)
,('Sales', true, false, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 0, 0)
,('Default', true, false, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 0, 0)
,('DH', true, false, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 0, 0);


alter table "page" 
alter column IsActive set default true;

alter table "page" 
alter column IsDeleted set default false;

alter table "page" 
alter column CreatedDate set default CURRENT_TIMESTAMP;

alter table "page" 
alter column LastUpdatedDate set default CURRENT_TIMESTAMP;

alter table "page" 
alter column CreatedBy set default 0;

alter table "page" 
alter column LastUpdatedBy set default 0;


INSERT INTO public.page
(pagename, icon, moduleid, controllername, actionname)
values
('Role', '<span class="iconify" data-icon="eos-icons:cluster-role"></span>', 1, 'Role', 'Index')
,('Employees', '<span class="iconify" data-icon="mdi:users"></span>', 0, 'Employee', 'Index')
,('Customers', '<span class="iconify" data-icon="icon-park-outline:customer"></span>', 0, 'Customer', 'Index')
,('Projects', '<span class="iconify" data-icon="grommet-icons:projects"></span>', 0, 'Project', 'Index')
,('Contracts', '<span class="iconify" data-icon="mdi:contract"></span>', 0, 'Contract', 'Index')


alter table employeerole
add column EmployeeId int not null

INSERT INTO public.employeerole
(roleid, employeeid)
VALUES(2, 6148),(6,7207);


CREATE TABLE ProjectionRequests (
    ProjectionRequestId SERIAL PRIMARY KEY,    -- Auto-incrementing primary key
    ProjectionId INT NOT NULL,       -- Foreign key to the Projection table
    EmployeeId INT NOT NULL,         -- Employee ID handling the request
    Remarks TEXT,                    -- Remarks for the request
    Status VARCHAR(50),              -- Status of the request
    RequestSentBy INT NOT NULL,      -- ID of the person who sent the request
    RequestSentTo INT NOT NULL,      -- ID of the person receiving the request
    LastUpdatedBy INT,               -- ID of the last person to update the request
    LastUpdateDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Last updated timestamp
    CreatedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,    -- Creation date
    CONSTRAINT fk_projection
        FOREIGN KEY (ProjectionId) REFERENCES Projection(ProjectionId) -- Foreign key constraint
);

CREATE TABLE projectioninitialbilling (
    projectioninitialbillingid SERIAL PRIMARY KEY,
    projectionid INT NOT NULL,
    monthyear VARCHAR(7) NOT NULL,  -- Format: YYYY-MM
    amount NUMERIC(10, 2) NOT NULL, -- Adjust precision as needed
    FOREIGN KEY (projectionid) REFERENCES projection(projectionid) ON DELETE CASCADE
);



ALTER TABLE projectcontract
ADD COLUMN isprojectestimationdone BOOLEAN DEFAULT false;