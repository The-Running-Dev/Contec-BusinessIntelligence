create table if not exists ErrorLogs
(
	`Date` datetime not null,
  	`Thread` varchar(255),
	`Level` varchar(50),
	`Logger` varchar(255),
  	`Message` varchar(4000),
	`Exception` varchar(4000),
	`Application` varchar(200),
	`Machine` varchar(255)
);