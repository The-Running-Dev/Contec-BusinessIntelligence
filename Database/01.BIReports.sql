create table if not exists BIReports
(
  Id char(36) not null primary key,
  Name varchar(255) not null,
  Description varchar(1000) null,
  EmbedSource varchar(1000) not null,
  CreatedBy varchar(255),
  CreatedOn datetime
);