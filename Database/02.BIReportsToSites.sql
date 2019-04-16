create table if not exists BIReportsToSites
(
	Id char(36) not null primary key,
  	SiteId int(11) not null,
	ReportId char(36) not null,
	CreatedBy varchar(255),
  	CreatedOn datetime
);