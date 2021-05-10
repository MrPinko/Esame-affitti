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
  `piano` varchar(45) DEFAULT NULL,
  `superficie` varchar(45) DEFAULT NULL,
  `costo` float DEFAULT NULL,
  PRIMARY KEY (`idappartamenti`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appartamenti`
--

LOCK TABLES `appartamenti` WRITE;
/*!40000 ALTER TABLE `appartamenti` DISABLE KEYS */;
/*!40000 ALTER TABLE `appartamenti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `att_turistiche`
--

DROP TABLE IF EXISTS `att_turistiche`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `att_turistiche` (
  `idatt_turistiche` int(11) NOT NULL AUTO_INCREMENT,
  `image` longtext,
  `via` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idatt_turistiche`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `att_turistiche`
--

LOCK TABLES `att_turistiche` WRITE;
/*!40000 ALTER TABLE `att_turistiche` DISABLE KEYS */;
INSERT INTO `att_turistiche` VALUES (1,'https://data.pixiz.com/output/user/frame/preview/400x400/4/6/6/8/3278664_d777c.jpg',NULL);
/*!40000 ALTER TABLE `att_turistiche` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `immobile_privato`
--

DROP TABLE IF EXISTS `immobile_privato`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `immobile_privato` (
  `idimmobile_privato` int(11) NOT NULL AUTO_INCREMENT,
  `via` varchar(45) DEFAULT NULL,
  `citta` varchar(45) DEFAULT NULL,
  `provincia` varchar(45) DEFAULT NULL,
  `anno` date DEFAULT NULL,
  PRIMARY KEY (`idimmobile_privato`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `immobile_privato`
--

LOCK TABLES `immobile_privato` WRITE;
/*!40000 ALTER TABLE `immobile_privato` DISABLE KEYS */;
/*!40000 ALTER TABLE `immobile_privato` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `proprietario`
--

DROP TABLE IF EXISTS `proprietario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `proprietario` (
  `idproprietario` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(45) DEFAULT NULL,
  `cognome` varchar(45) DEFAULT NULL,
  `cf` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `telefono` int(11) DEFAULT NULL,
  `social` json DEFAULT NULL,
  `iban` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idproprietario`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `proprietario`
--

LOCK TABLES `proprietario` WRITE;
/*!40000 ALTER TABLE `proprietario` DISABLE KEYS */;
INSERT INTO `proprietario` VALUES (1,NULL,NULL,NULL,NULL,NULL,'{\"age\": 24, \"name\": \"Cameron\"}',NULL);
/*!40000 ALTER TABLE `proprietario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recensioni`
--

DROP TABLE IF EXISTS `recensioni`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recensioni` (
  `idrecensioni` int(11) NOT NULL AUTO_INCREMENT,
  `panorama` int(11) DEFAULT NULL,
  `cibo` int(11) DEFAULT NULL,
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
-- Table structure for table `utente`
--

DROP TABLE IF EXISTS `utente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `utente` (
  `idutente` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(45) DEFAULT NULL,
  `pw` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `cell` varchar(45) DEFAULT NULL,
  `citta` varchar(45) DEFAULT NULL,
  `via` varchar(45) DEFAULT NULL,
  `dataN` date DEFAULT NULL,
  `sesso` varchar(1) DEFAULT NULL,
  `cf` varchar(45) DEFAULT NULL,
  `nome` varchar(45) DEFAULT NULL,
  `cognome` varchar(45) DEFAULT NULL,
  `m_pagamento` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idutente`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `utente`
--

LOCK TABLES `utente` WRITE;
/*!40000 ALTER TABLE `utente` DISABLE KEYS */;
/*!40000 ALTER TABLE `utente` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-05-10 11:47:16
