create table if not exists BIAudit
(
	`Id` char(36) not null primary key,
	`ActionId` int,
	`Action` varchar(50),
	`UpdatedBy` varchar(255),
	`UpdatedOn` datetime,
	`CreatedBy` varchar(255) not null,
	`CreatedOn` datetime not null
);