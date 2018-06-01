CREATE DATABASE `btbasedb` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `btbasedb`;
CREATE TABLE `BTAccount` (
  `AccountRawId` bigint(20) NOT NULL AUTO_INCREMENT,
  `AccountTypes` text,
  `Email` text,
  `Mobile` text,
  `Nick` text,
  `Password` varchar(2048) NOT NULL,
  `SignDateTs` double NOT NULL,
  `UserName` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`AccountRawId`),
  UNIQUE KEY `IX_BTAccount_UserName` (`UserName`)
) ENGINE=InnoDB AUTO_INCREMENT=666666 DEFAULT CHARSET=utf8;

CREATE TABLE `BTDeviceSession` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `AccountId` varchar(32) NOT NULL,
  `DeviceId` varchar(512) NOT NULL,
  `DeviceName` text,
  `IsValid` bit(1) NOT NULL,
  `LoginDateTs` double NOT NULL,
  `ReactiveDateTs` double NOT NULL,
  `SessionKey` varchar(512) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_BTDeviceSession_AccountId_DeviceId_SessionKey` (`AccountId`,`DeviceId`(255),`SessionKey`(255))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `BTMember` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `AccountId` varchar(32) NOT NULL,
  `ExpiredDateTs` double NOT NULL,
  `FirstChargeDateTs` double NOT NULL,
  `MemberType` int(11) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_BTMember_AccountId` (`AccountId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `BTMemberOrder` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `AccountId` varchar(32) NOT NULL,
  `ChargeTimes` double NOT NULL,
  `ChargedExpiredDateTime` datetime NOT NULL,
  `MemberType` int(11) NOT NULL,
  `OrderDateTs` double NOT NULL,
  `OrderKey` varchar(512) DEFAULT NULL,
  `ProductId` text,
  `ReceiptData` text,
  PRIMARY KEY (`ID`),
  KEY `IX_BTMemberOrder_AccountId` (`AccountId`),
  KEY `IX_BTMemberOrder_OrderKey` (`OrderKey`(255))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `BTSecurityCode` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `AccountId` varchar(32) NOT NULL,
  `Code` varchar(32) DEFAULT NULL,
  `ExpiredOn` datetime NOT NULL,
  `Receiver` varchar(128) DEFAULT NULL,
  `ReceiverType` int(11) NOT NULL,
  `RequestDate` datetime NOT NULL,
  `RequestFor` int(11) NOT NULL,
  `Status` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `SecurityKeychain` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `Algorithm` varchar(64) NOT NULL,
  `CreateDate` datetime NOT NULL,
  `Name` varchar(128) DEFAULT NULL,
  `Note` text,
  `PrivateKey` text,
  `PublicKey` text,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `UpdateEmailRecord` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `AccountId` varchar(32) NOT NULL,
  `ReplacedEmail` text NOT NULL,
  `RequestDate` datetime NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `UpdatePasswordRecord` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `AccountId` varchar(32) NOT NULL,
  `ExpiredOn` datetime NOT NULL,
  `Password` varchar(2048) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
