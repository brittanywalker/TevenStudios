-- MySQL Script generated by MySQL Workbench
-- 08/22/17 22:51:40
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema TevenStudios
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema TevenStudios
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `TevenStudios` DEFAULT CHARACTER SET utf8 ;
USE `TevenStudios` ;

-- -----------------------------------------------------
-- Table `TevenStudios`.`Role_Types`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `TevenStudios`.`Role_Types` (
  `ID` INT NOT NULL,
  `Type` TEXT(60) NULL,
  PRIMARY KEY (`ID`),
  UNIQUE INDEX `ID_UNIQUE` (`ID` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `TevenStudios`.`User`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `TevenStudios`.`User` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Name` TEXT(60) NULL,
  `Email` VARCHAR(60) NULL,
  `StartDate` DATETIME NULL,
  `ManagerId` INT NULL,
  `RoleId` INT NULL,
  `StartBudget` DOUBLE NULL,
  PRIMARY KEY (`ID`),
  UNIQUE INDEX `id_UNIQUE` (`ID` ASC),
  INDEX `Role_ID_idx` (`RoleId` ASC),
  INDEX `Manager_ID_idx` (`ManagerId` ASC),
  CONSTRAINT `Role_ID`
    FOREIGN KEY (`RoleId`)
    REFERENCES `TevenStudios`.`Role_Types` (`ID`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `Manager_ID`
    FOREIGN KEY (`ManagerId`)
    REFERENCES `TevenStudios`.`User` (`ID`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `TevenStudios`.`Status_Types`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `TevenStudios`.`Status_Types` (
  `ID` INT NOT NULL,
  `Type` TEXT(60) NULL,
  PRIMARY KEY (`ID`),
  UNIQUE INDEX `ID_UNIQUE` (`ID` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `TevenStudios`.`Transactions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `TevenStudios`.`Transactions` (
  `UserId` INT NOT NULL,
  `StartDate` DATETIME NULL,
  `Description` LONGTEXT NULL,
  `Amount` DOUBLE NULL,
  `StatusId` INT NULL,
  `ID` INT NOT NULL,
  PRIMARY KEY (`UserId`, `ID`),
  UNIQUE INDEX `User_ID_UNIQUE` (`UserId` ASC),
  INDEX `Status_ID_idx` (`StatusId` ASC),
  UNIQUE INDEX `ID_UNIQUE` (`ID` ASC),
  CONSTRAINT `User_ID`
    FOREIGN KEY (`UserId`)
    REFERENCES `TevenStudios`.`User` (`ID`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `Status_ID`
    FOREIGN KEY (`StatusId`)
    REFERENCES `TevenStudios`.`Status_Types` (`ID`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `TevenStudios`.`Role_Types`
-- -----------------------------------------------------
START TRANSACTION;
USE `TevenStudios`;
INSERT INTO `TevenStudios`.`Role_Types` (`ID`, `Type`) VALUES (0, 'Admin');
INSERT INTO `TevenStudios`.`Role_Types` (`ID`, `Type`) VALUES (1, 'Employee');
INSERT INTO `TevenStudios`.`Role_Types` (`ID`, `Type`) VALUES (2, 'Manager');

COMMIT;


-- -----------------------------------------------------
-- Data for table `TevenStudios`.`Status_Types`
-- -----------------------------------------------------
START TRANSACTION;
USE `TevenStudios`;
INSERT INTO `TevenStudios`.`Status_Types` (`ID`, `Type`) VALUES (0, 'Pending');
INSERT INTO `TevenStudios`.`Status_Types` (`ID`, `Type`) VALUES (1, 'Approved');
INSERT INTO `TevenStudios`.`Status_Types` (`ID`, `Type`) VALUES (2, 'Denied');

COMMIT;

