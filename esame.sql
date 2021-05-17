-- MySQL dump 10.13  Distrib 5.7.9, for Win64 (x86_64)
--
-- Host: localhost    Database: esame
-- ------------------------------------------------------
-- Server version	5.7.10-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `appartamenti`
--

DROP TABLE IF EXISTS `appartamenti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `appartamenti` (
  `idappartamenti` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(45) DEFAULT NULL,
  `piano` varchar(45) DEFAULT NULL,
  `superficie` varchar(45) DEFAULT NULL,
  `costo` float DEFAULT NULL,
  `fk_immobilePrivato` int(11) DEFAULT NULL,
  `fk_immagini` int(11) DEFAULT NULL,
  PRIMARY KEY (`idappartamenti`),
  KEY `fk_immobilePrivato_idx` (`fk_immobilePrivato`),
  KEY `fk_immagini_idx` (`fk_immagini`),
  CONSTRAINT `fk_immagini2` FOREIGN KEY (`fk_immagini`) REFERENCES `immagini` (`id_immagini`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_immobilePrivato` FOREIGN KEY (`fk_immobilePrivato`) REFERENCES `immobile_privato` (`idimmobile`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appartamenti`
--

LOCK TABLES `appartamenti` WRITE;
/*!40000 ALTER TABLE `appartamenti` DISABLE KEYS */;
INSERT INTO `appartamenti` VALUES (0,'heart milan','1','71',251,0,0),(1,'vittoria corner','3','60',200,1,1),(2,'Brera Apartments ','2','25',100,0,2),(3,'La Perla by Sedar','1','100',500,2,3),(4,'Vanvitelli\'s Home','1','50',200,3,4);
/*!40000 ALTER TABLE `appartamenti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `appartamenti_attturistiche`
--

DROP TABLE IF EXISTS `appartamenti_attturistiche`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `appartamenti_attturistiche` (
  `idappartamenti_attTuristiche` int(11) NOT NULL,
  `fk_appartamenti` int(11) DEFAULT NULL,
  `fk_attTuristiche` int(11) DEFAULT NULL,
  PRIMARY KEY (`idappartamenti_attTuristiche`),
  KEY `fk_appartamenti_idx` (`fk_appartamenti`),
  KEY `fk_attTuristiche_idx` (`fk_attTuristiche`),
  CONSTRAINT `fk_appartamenti2` FOREIGN KEY (`fk_appartamenti`) REFERENCES `appartamenti` (`idappartamenti`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_attTuristiche` FOREIGN KEY (`fk_attTuristiche`) REFERENCES `att_turistiche` (`idatt_turistiche`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appartamenti_attturistiche`
--

LOCK TABLES `appartamenti_attturistiche` WRITE;
/*!40000 ALTER TABLE `appartamenti_attturistiche` DISABLE KEYS */;
/*!40000 ALTER TABLE `appartamenti_attturistiche` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `appartamenti_recensioni`
--

DROP TABLE IF EXISTS `appartamenti_recensioni`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `appartamenti_recensioni` (
  `idappartamenti_recensioni` int(11) NOT NULL,
  `fk_recensioni` int(11) DEFAULT NULL,
  `fk_appartamenti` int(11) DEFAULT NULL,
  PRIMARY KEY (`idappartamenti_recensioni`),
  KEY `fk_recensioni_idx` (`fk_recensioni`),
  KEY `fk_appartamenti_idx` (`fk_appartamenti`),
  CONSTRAINT `fk_appartamenti3` FOREIGN KEY (`fk_appartamenti`) REFERENCES `appartamenti` (`idappartamenti`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_recensioni` FOREIGN KEY (`fk_recensioni`) REFERENCES `recensioni` (`idrecensioni`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appartamenti_recensioni`
--

LOCK TABLES `appartamenti_recensioni` WRITE;
/*!40000 ALTER TABLE `appartamenti_recensioni` DISABLE KEYS */;
/*!40000 ALTER TABLE `appartamenti_recensioni` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `att_turistiche`
--

DROP TABLE IF EXISTS `att_turistiche`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `att_turistiche` (
  `idatt_turistiche` int(11) NOT NULL AUTO_INCREMENT,
  `via` varchar(45) DEFAULT NULL,
  `fk_immagini` int(11) DEFAULT NULL,
  PRIMARY KEY (`idatt_turistiche`),
  KEY `fk_immagini_idx` (`fk_immagini`),
  CONSTRAINT `fk_immagini` FOREIGN KEY (`fk_immagini`) REFERENCES `immagini` (`id_immagini`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `att_turistiche`
--

LOCK TABLES `att_turistiche` WRITE;
/*!40000 ALTER TABLE `att_turistiche` DISABLE KEYS */;
INSERT INTO `att_turistiche` VALUES (1,NULL,NULL);
/*!40000 ALTER TABLE `att_turistiche` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `immagini`
--

DROP TABLE IF EXISTS `immagini`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `immagini` (
  `url` varchar(45) DEFAULT NULL,
  `id_immagini` int(11) NOT NULL,
  PRIMARY KEY (`id_immagini`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `immagini`
--

LOCK TABLES `immagini` WRITE;
/*!40000 ALTER TABLE `immagini` DISABLE KEYS */;
INSERT INTO `immagini` VALUES (NULL,0),(NULL,1),(NULL,2),(NULL,3),(NULL,4);
/*!40000 ALTER TABLE `immagini` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `immobile_privato`
--

DROP TABLE IF EXISTS `immobile_privato`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `immobile_privato` (
  `idimmobile` int(11) NOT NULL,
  `via` varchar(45) DEFAULT NULL,
  `numeroC` int(11) DEFAULT NULL,
  `cap` int(11) DEFAULT NULL,
  `citta` varchar(45) DEFAULT NULL,
  `provincia` varchar(45) DEFAULT NULL,
  `lat` double DEFAULT NULL,
  `long` double DEFAULT NULL,
  `anno` year(4) DEFAULT NULL,
  PRIMARY KEY (`idimmobile`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `immobile_privato`
--

LOCK TABLES `immobile_privato` WRITE;
/*!40000 ALTER TABLE `immobile_privato` DISABLE KEYS */;
INSERT INTO `immobile_privato` VALUES (0,'del parco',29,20064,'milano','MI',45.526318538655474,9.406340990479956,1980),(1,'luigi sacco',3,22100,'como','CO',45.81019362511003,9.08741389619552,2000),(2,'fratelli rosselli',2,24068,'Seriate','BG',45.67703729517465,9.714916419303293,2010),(3,'giuseppe maria bosco',111,81100,'Caserta','CE',41.08052309544633,14.336794799420527,1946);
/*!40000 ALTER TABLE `immobile_privato` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `proprietario`
--

DROP TABLE IF EXISTS `proprietario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `proprietario` (
  `cf_proprietario` varchar(45) NOT NULL,
  `nome` varchar(45) DEFAULT NULL,
  `cognome` varchar(45) DEFAULT NULL,
  `dataN` date DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `telefono` varchar(45) DEFAULT NULL,
  `iban` varchar(45) DEFAULT NULL,
  `fk_immobilePrivato` int(11) DEFAULT NULL,
  PRIMARY KEY (`cf_proprietario`),
  KEY `fk_immobilePrivato_idx` (`fk_immobilePrivato`),
  CONSTRAINT `fk_immobile_privato` FOREIGN KEY (`fk_immobilePrivato`) REFERENCES `immobile_privato` (`idimmobile`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `proprietario`
--

LOCK TABLES `proprietario` WRITE;
/*!40000 ALTER TABLE `proprietario` DISABLE KEYS */;
INSERT INTO `proprietario` VALUES ('DNZMLA95T49C088Y','amalia','iadanza','1995-12-09','amalia95@hootmail.com','03124985372','IT25B0300203280225412791151',2),('GNVCHR97P46E595H','chiara','genovesi','1997-09-06','chiaraGenovesi@yahoo.com','03453906573','IT19F0300203280148362476191',3),('GNVCMR64R10F492C','calimero','genovese','1964-11-10','calimerogenovese@gmail.com','00367905660','IT27E0300203280177977433552',1),('TRNDVD58A01B534L','davide','trentini','1958-01-01','DavideTrentini@gmail.com','03814962856','IT23X0300203280188444111643',0);
/*!40000 ALTER TABLE `proprietario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `proprietario_social`
--

DROP TABLE IF EXISTS `proprietario_social`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `proprietario_social` (
  `idproprietario_social` int(11) NOT NULL,
  `fk_proprietario` varchar(45) DEFAULT NULL,
  `fk_social` int(11) DEFAULT NULL,
  PRIMARY KEY (`idproprietario_social`),
  KEY `fk_proprietario_idx` (`fk_proprietario`),
  KEY `fk_social_idx` (`fk_social`),
  CONSTRAINT `fk_proprietario` FOREIGN KEY (`fk_proprietario`) REFERENCES `proprietario` (`cf_proprietario`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_social` FOREIGN KEY (`fk_social`) REFERENCES `social` (`idsocial`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `proprietario_social`
--

LOCK TABLES `proprietario_social` WRITE;
/*!40000 ALTER TABLE `proprietario_social` DISABLE KEYS */;
INSERT INTO `proprietario_social` VALUES (0,'GNVCMR64R10F492C',0),(1,'GNVCMR64R10F492C',1),(2,'TRNDVD58A01B534L',2),(3,'TRNDVD58A01B534L',3),(4,'DNZMLA95T49C088Y',6),(5,'GNVCHR97P46E595H',4),(6,'GNVCHR97P46E595H',5);
/*!40000 ALTER TABLE `proprietario_social` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recensioni`
--

DROP TABLE IF EXISTS `recensioni`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recensioni` (
  `idrecensioni` int(11) NOT NULL AUTO_INCREMENT,
  `posizione` int(11) DEFAULT NULL,
  `qualita_prezzo` int(11) DEFAULT NULL,
  `servizio` int(11) DEFAULT NULL,
  PRIMARY KEY (`idrecensioni`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recensioni`
--

LOCK TABLES `recensioni` WRITE;
/*!40000 ALTER TABLE `recensioni` DISABLE KEYS */;
/*!40000 ALTER TABLE `recensioni` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `servizi`
--

DROP TABLE IF EXISTS `servizi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `servizi` (
  `idservizi` int(11) NOT NULL,
  `servizio` varchar(45) DEFAULT NULL,
  `fk_appartamenti` int(11) DEFAULT NULL,
  PRIMARY KEY (`idservizi`),
  KEY `fk_appartamento_idx` (`fk_appartamenti`),
  CONSTRAINT `fk_appartamenti_servizi` FOREIGN KEY (`fk_appartamenti`) REFERENCES `appartamenti` (`idappartamenti`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `servizi`
--

LOCK TABLES `servizi` WRITE;
/*!40000 ALTER TABLE `servizi` DISABLE KEYS */;
INSERT INTO `servizi` VALUES (0,'wifi',0),(1,'aria condizionata',0),(2,'parcheggio privato',1),(3,'wifi',1),(4,'balcone',2),(5,'wifi',3),(6,'balcone',3),(7,'divano letto',4),(8,'wifi',4);
/*!40000 ALTER TABLE `servizi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `social`
--

DROP TABLE IF EXISTS `social`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `social` (
  `idsocial` int(11) NOT NULL,
  `provider` varchar(45) DEFAULT NULL,
  `nome` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idsocial`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `social`
--

LOCK TABLES `social` WRITE;
/*!40000 ALTER TABLE `social` DISABLE KEYS */;
INSERT INTO `social` VALUES (0,'facebook','calimero genovese'),(1,'instagram','calimero64_official'),(2,'facebook','davide trentini'),(3,'twitter','davide trentini'),(4,'facebook','chiara genovesi'),(5,'instagram','chiara_case97'),(6,'twitter','amalia iadanza');
/*!40000 ALTER TABLE `social` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `utente`
--

DROP TABLE IF EXISTS `utente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `utente` (
  `username` varchar(45) DEFAULT NULL,
  `pw` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `cell` varchar(45) DEFAULT NULL,
  `citta` varchar(45) DEFAULT NULL,
  `via` varchar(45) DEFAULT NULL,
  `dataN` date DEFAULT NULL,
  `sesso` varchar(1) DEFAULT NULL,
  `cf_utente` varchar(45) NOT NULL,
  `nome` varchar(45) DEFAULT NULL,
  `cognome` varchar(45) DEFAULT NULL,
  `m_pagamento` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`cf_utente`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `utente`
--

LOCK TABLES `utente` WRITE;
/*!40000 ALTER TABLE `utente` DISABLE KEYS */;
/*!40000 ALTER TABLE `utente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `utente_appartamenti`
--

DROP TABLE IF EXISTS `utente_appartamenti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `utente_appartamenti` (
  `idUtente_Apaprtamenti` int(11) NOT NULL AUTO_INCREMENT,
  `dataInizio` date DEFAULT NULL,
  `dataFine` date DEFAULT NULL,
  `fk_appartamenti` int(11) DEFAULT NULL,
  `fk_utente` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idUtente_Apaprtamenti`),
  KEY `fk_utente_idx` (`fk_utente`),
  KEY `fk_appartamenti_idx` (`fk_appartamenti`),
  CONSTRAINT `fk_appartamenti` FOREIGN KEY (`fk_appartamenti`) REFERENCES `appartamenti` (`idappartamenti`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_utente` FOREIGN KEY (`fk_utente`) REFERENCES `utente` (`cf_utente`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `utente_appartamenti`
--

LOCK TABLES `utente_appartamenti` WRITE;
/*!40000 ALTER TABLE `utente_appartamenti` DISABLE KEYS */;
/*!40000 ALTER TABLE `utente_appartamenti` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-05-17 12:40:14
