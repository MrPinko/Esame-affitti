-- phpMyAdmin SQL Dump
-- version 4.1.7
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Mag 23, 2021 alle 11:19
-- Versione del server: 8.0.21
-- PHP Version: 5.6.40

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `my_rosafedericoesame`
--

-- --------------------------------------------------------

--
-- Struttura della tabella `appartamenti`
--

CREATE TABLE IF NOT EXISTS `appartamenti` (
  `idappartamenti` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(45) DEFAULT NULL,
  `piano` varchar(45) DEFAULT NULL,
  `superficie` varchar(45) DEFAULT NULL,
  `costo` float DEFAULT NULL,
  `fk_immobilePrivato` int DEFAULT NULL,
  `fk_immagini` int DEFAULT NULL,
  PRIMARY KEY (`idappartamenti`),
  KEY `fk_immobilePrivato_idx` (`fk_immobilePrivato`),
  KEY `fk_immagini_idx` (`fk_immagini`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=5 ;

--
-- Dump dei dati per la tabella `appartamenti`
--

INSERT INTO `appartamenti` (`idappartamenti`, `nome`, `piano`, `superficie`, `costo`, `fk_immobilePrivato`, `fk_immagini`) VALUES
(0, 'heart milan', '1', '71', 251, 0, 0),
(1, 'vittoria corner', '3', '60', 200, 1, 1),
(2, 'Brera Apartments ', '2', '25', 100, 0, 2),
(3, 'La Perla by Sedar', '1', '100', 500, 2, 3),
(4, 'Vanvitelli''s Home', '1', '50', 200, 3, 4);

-- --------------------------------------------------------

--
-- Struttura della tabella `appartamenti_attturistiche`
--

CREATE TABLE IF NOT EXISTS `appartamenti_attturistiche` (
  `idappartamenti_attTuristiche` int NOT NULL,
  `fk_appartamenti` int DEFAULT NULL,
  `fk_attTuristiche` int DEFAULT NULL,
  PRIMARY KEY (`idappartamenti_attTuristiche`),
  KEY `fk_appartamenti_idx` (`fk_appartamenti`),
  KEY `fk_attTuristiche_idx` (`fk_attTuristiche`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Struttura della tabella `appartamenti_recensioni`
--

CREATE TABLE IF NOT EXISTS `appartamenti_recensioni` (
  `idappartamenti_recensioni` int NOT NULL,
  `fk_recensioni` int DEFAULT NULL,
  `fk_appartamenti` int DEFAULT NULL,
  PRIMARY KEY (`idappartamenti_recensioni`),
  KEY `fk_recensioni_idx` (`fk_recensioni`),
  KEY `fk_appartamenti_idx` (`fk_appartamenti`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dump dei dati per la tabella `appartamenti_recensioni`
--

INSERT INTO `appartamenti_recensioni` (`idappartamenti_recensioni`, `fk_recensioni`, `fk_appartamenti`) VALUES
(0, 0, 0),
(1, 3, 0),
(2, 1, 1),
(3, 2, 2),
(4, 4, 3),
(5, 5, 4);

-- --------------------------------------------------------

--
-- Struttura della tabella `att_turistiche`
--

CREATE TABLE IF NOT EXISTS `att_turistiche` (
  `idatt_turistiche` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(45) DEFAULT NULL,
  `descrizione` text NOT NULL,
  `lat` double DEFAULT NULL,
  `long` double DEFAULT NULL,
  `fk_immagini` int DEFAULT NULL,
  PRIMARY KEY (`idatt_turistiche`),
  KEY `fk_immagini_idx` (`fk_immagini`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=6 ;

--
-- Dump dei dati per la tabella `att_turistiche`
--

INSERT INTO `att_turistiche` (`idatt_turistiche`, `nome`, `descrizione`, `lat`, `long`, `fk_immagini`) VALUES
(0, 'parco del castello ducale degli aglie', 'Imponente castello con più di 300 camere e una sala da ballo affrescata, oltre a parco, fontane e giardini.', 45.36034073514196, 7.77013591317539, 0),
(1, 'palazzo anguissola', 'Il corpo interno del palazzo fu realizzato tra il 1775 e il 1778 su progetto dell''architetto Carlo Felice Soave, originario di Lugano su incarico di Antonio Anguissola. ', 45.46829856600218, 9.191056783220107, 1),
(2, 'orto botanico lorenzo rota', 'L''Orto botanico Lorenzo Rota di Bergamo che si trova sul Colle Aperto di Città Alta, è un piccolo laboratorio naturistico dove la passione e l''arte degli addetti fanno convivere piante esotiche con quelle indigene.', 45.70739090645334, 9.660151337194366, 2),
(3, 'cripta di san giovanni', 'La cripta di San Giovanni in Conca è un monumento situato in piazza Missori a Milano. Si tratta dei resti dell''antica basilica di San Giovanni in Conca, della quale rimangono oggi solo poche tracce risalenti all''XI secolo, vale a dire parte dell''abside e l''intera cripta, da cui il nome dei moderni resti', 45.46130950655222, 9.18885752925188, 3),
(4, 'terme di cavascura', 'Le terme di Cavascura sono delle sorgenti naturali conosciute fin dal periodo della colonizzazione greca dell''isola d''Ischia ma fu nel periodo romano che esse conobbero un momento di grande splendore. Si tratta di un bacino idrologico allo stato naturale scavato nella pietra viva di un vallone.', 40.7041479139664, 13.904566283104904, 4),
(5, 'villa di diomele', 'Si sviluppa scenograficamente su tre livelli aprendosi con giardini e piscine verso l''antica linea di costa. Uno degli spazi più suggestivi è il bellissimo giardino al centro del quale vi era un triclinio coperto da una pergola per i banchetti estivi e una piscina.', 40.75233667691031, 14.478324970937294, 5);

-- --------------------------------------------------------

--
-- Struttura della tabella `immagini`
--

CREATE TABLE IF NOT EXISTS `immagini` (
  `url` varchar(45) DEFAULT NULL,
  `id_immagini` int NOT NULL,
  PRIMARY KEY (`id_immagini`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dump dei dati per la tabella `immagini`
--

INSERT INTO `immagini` (`url`, `id_immagini`) VALUES
(NULL, 0),
(NULL, 1),
(NULL, 2),
(NULL, 3),
(NULL, 4);

-- --------------------------------------------------------

--
-- Struttura della tabella `immagini_per_appartamenti`
--

CREATE TABLE IF NOT EXISTS `immagini_per_appartamenti` (
  `id_immagini` int NOT NULL,
  `url` mediumtext,
  `fkAppartamento` int DEFAULT NULL,
  PRIMARY KEY (`id_immagini`),
  KEY `fk_immagini_apparamento_idx` (`fkAppartamento`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dump dei dati per la tabella `immagini_per_appartamenti`
--

INSERT INTO `immagini_per_appartamenti` (`id_immagini`, `url`, `fkAppartamento`) VALUES
(0, 'https://i.postimg.cc/4xrjdW6v/heart-milan1.jpg', 0),
(1, 'https://i.postimg.cc/fLsF7kgJ/heart-milan2.jpg', 0),
(2, 'https://i.postimg.cc/vZ1pbJVH/heart-milan3.jpg', 0),
(3, 'https://i.postimg.cc/3wmVD21s/vittoria-corner.jpg', 1),
(4, 'https://i.postimg.cc/fTqG4zkr/vittoria-corner2.jpg', 1),
(5, 'https://i.postimg.cc/pr2wvyDg/vittoria-corner3.jpg', 1),
(6, 'https://i.postimg.cc/QNJDzZp2/brera-appartments1.jpg', 2),
(7, 'https://i.postimg.cc/Zn4Sqwhs/brera-appartments2.jpg', 2),
(8, 'https://i.postimg.cc/sXFzkVHr/brera-appartments3.jpg', 2),
(9, 'https://i.postimg.cc/YSFKf5ry/la-perla1.jpg', 3),
(10, 'https://i.postimg.cc/ydFKBw2S/la-perla2.jpg', 3),
(11, 'https://i.postimg.cc/d36KZGss/la-perla3.jpg', 3),
(12, 'https://i.postimg.cc/85d2xLrR/vanitelli-home1.jpg', 4),
(13, 'https://i.postimg.cc/3Jrh5f1d/vanitelli-home2.jpg', 4),
(14, 'https://i.postimg.cc/c4DSs7kT/vanitelli-home3.jpg', 4);

-- --------------------------------------------------------

--
-- Struttura della tabella `immagini_per_attrazionituristiche`
--

CREATE TABLE IF NOT EXISTS `immagini_per_attrazionituristiche` (
  `idimmagini_per_attrazioniTuristiche` int NOT NULL,
  `url` mediumtext,
  PRIMARY KEY (`idimmagini_per_attrazioniTuristiche`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dump dei dati per la tabella `immagini_per_attrazionituristiche`
--

INSERT INTO `immagini_per_attrazionituristiche` (`idimmagini_per_attrazioniTuristiche`, `url`) VALUES
(0, 'https://i.postimg.cc/ZRJTdSd7/Parco-del-Castello-Ducale-di-Aglie.jpg'),
(1, 'https://i.postimg.cc/TPVd1wp5/Palazzo-Anguissola.jpg'),
(2, 'https://i.postimg.cc/gcSYn1ks/Orto-Botanico-Lorenzo-Rota.jpg'),
(3, 'https://i.postimg.cc/gJ4GyTyY/Cripta-di-San-Giovanni-in-Conca.jpg'),
(4, 'https://i.postimg.cc/zXyqWpyr/Terme-di-Cavascura.jpg'),
(5, 'https://i.postimg.cc/Gt5rVTSV/Villa-di-Diomede.jpg');

-- --------------------------------------------------------

--
-- Struttura della tabella `immobile_privato`
--

CREATE TABLE IF NOT EXISTS `immobile_privato` (
  `idimmobile` int NOT NULL,
  `via` varchar(45) DEFAULT NULL,
  `numeroC` int DEFAULT NULL,
  `cap` int DEFAULT NULL,
  `citta` varchar(45) DEFAULT NULL,
  `provincia` varchar(45) DEFAULT NULL,
  `lat` double DEFAULT NULL,
  `long` double DEFAULT NULL,
  `anno` year DEFAULT NULL,
  PRIMARY KEY (`idimmobile`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dump dei dati per la tabella `immobile_privato`
--

INSERT INTO `immobile_privato` (`idimmobile`, `via`, `numeroC`, `cap`, `citta`, `provincia`, `lat`, `long`, `anno`) VALUES
(0, 'del parco', 29, 20064, 'milano', 'MI', 45.526318538655474, 9.406340990479956, 1980),
(1, 'luigi sacco', 3, 22100, 'como', 'CO', 45.81019362511003, 9.08741389619552, 2000),
(2, 'fratelli rosselli', 2, 24068, 'Seriate', 'BG', 45.67703729517465, 9.714916419303293, 2010),
(3, 'giuseppe maria bosco', 111, 81100, 'Caserta', 'CE', 41.08052309544633, 14.336794799420527, 1946);

-- --------------------------------------------------------

--
-- Struttura della tabella `proprietario`
--

CREATE TABLE IF NOT EXISTS `proprietario` (
  `cf_proprietario` varchar(45) NOT NULL,
  `nome` varchar(45) DEFAULT NULL,
  `cognome` varchar(45) DEFAULT NULL,
  `dataN` date DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `telefono` varchar(45) DEFAULT NULL,
  `iban` varchar(45) DEFAULT NULL,
  `fk_immobilePrivato` int DEFAULT NULL,
  PRIMARY KEY (`cf_proprietario`),
  KEY `fk_immobilePrivato_idx` (`fk_immobilePrivato`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dump dei dati per la tabella `proprietario`
--

INSERT INTO `proprietario` (`cf_proprietario`, `nome`, `cognome`, `dataN`, `email`, `telefono`, `iban`, `fk_immobilePrivato`) VALUES
('DNZMLA95T49C088Y', 'amalia', 'iadanza', '1995-12-09', 'amalia95@hootmail.com', '03124985372', 'IT25B0300203280225412791151', 2),
('GNVCHR97P46E595H', 'chiara', 'genovesi', '1997-09-06', 'chiaraGenovesi@yahoo.com', '03453906573', 'IT19F0300203280148362476191', 3),
('GNVCMR64R10F492C', 'calimero', 'genovese', '1964-11-10', 'calimerogenovese@gmail.com', '00367905660', 'IT27E0300203280177977433552', 1),
('TRNDVD58A01B534L', 'davide', 'trentini', '1958-01-01', 'DavideTrentini@gmail.com', '03814962856', 'IT23X0300203280188444111643', 0);

-- --------------------------------------------------------

--
-- Struttura della tabella `proprietario_social`
--

CREATE TABLE IF NOT EXISTS `proprietario_social` (
  `idproprietario_social` int NOT NULL,
  `fk_proprietario` varchar(45) DEFAULT NULL,
  `fk_social` int DEFAULT NULL,
  PRIMARY KEY (`idproprietario_social`),
  KEY `fk_proprietario_idx` (`fk_proprietario`),
  KEY `fk_social_idx` (`fk_social`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dump dei dati per la tabella `proprietario_social`
--

INSERT INTO `proprietario_social` (`idproprietario_social`, `fk_proprietario`, `fk_social`) VALUES
(0, 'GNVCMR64R10F492C', 0),
(1, 'GNVCMR64R10F492C', 1),
(2, 'TRNDVD58A01B534L', 2),
(3, 'TRNDVD58A01B534L', 3),
(4, 'DNZMLA95T49C088Y', 6),
(5, 'GNVCHR97P46E595H', 4),
(6, 'GNVCHR97P46E595H', 5);

-- --------------------------------------------------------

--
-- Struttura della tabella `recensioni`
--

CREATE TABLE IF NOT EXISTS `recensioni` (
  `idrecensioni` int NOT NULL AUTO_INCREMENT,
  `posizione` int DEFAULT NULL,
  `qualita_prezzo` int DEFAULT NULL,
  `servizio` int DEFAULT NULL,
  PRIMARY KEY (`idrecensioni`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=6 ;

--
-- Dump dei dati per la tabella `recensioni`
--

INSERT INTO `recensioni` (`idrecensioni`, `posizione`, `qualita_prezzo`, `servizio`) VALUES
(0, 6, 7, 2),
(1, 9, 8, 9),
(2, 4, 8, 7),
(3, 3, 7, 6),
(4, 8, 7, 6),
(5, 5, 9, 7);

-- --------------------------------------------------------

--
-- Struttura della tabella `servizi`
--

CREATE TABLE IF NOT EXISTS `servizi` (
  `idservizi` int NOT NULL,
  `servizio` varchar(45) DEFAULT NULL,
  `fk_appartamenti` int DEFAULT NULL,
  PRIMARY KEY (`idservizi`),
  KEY `fk_appartamento_idx` (`fk_appartamenti`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dump dei dati per la tabella `servizi`
--

INSERT INTO `servizi` (`idservizi`, `servizio`, `fk_appartamenti`) VALUES
(0, 'wifi', 0),
(1, 'aria condizionata', 0),
(2, 'parcheggio privato', 1),
(3, 'wifi', 1),
(4, 'balcone', 2),
(5, 'wifi', 3),
(6, 'balcone', 3),
(7, 'divano letto', 4),
(8, 'wifi', 4);

-- --------------------------------------------------------

--
-- Struttura della tabella `social`
--

CREATE TABLE IF NOT EXISTS `social` (
  `idsocial` int NOT NULL,
  `provider` varchar(45) DEFAULT NULL,
  `nome` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idsocial`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dump dei dati per la tabella `social`
--

INSERT INTO `social` (`idsocial`, `provider`, `nome`) VALUES
(0, 'facebook', 'calimero genovese'),
(1, 'instagram', 'calimero64_official'),
(2, 'facebook', 'davide trentini'),
(3, 'twitter', 'davide trentini'),
(4, 'facebook', 'chiara genovesi'),
(5, 'instagram', 'chiara_case97'),
(6, 'twitter', 'amalia iadanza');

-- --------------------------------------------------------

--
-- Struttura della tabella `utente`
--

CREATE TABLE IF NOT EXISTS `utente` (
  `username` varchar(45) DEFAULT NULL,
  `pw` longtext CHARACTER SET utf8 COLLATE utf8_general_ci,
  `email` varchar(45) DEFAULT NULL,
  `cell` varchar(45) DEFAULT NULL,
  `citta` varchar(45) DEFAULT NULL,
  `via` varchar(45) DEFAULT NULL,
  `numero` int DEFAULT NULL,
  `cap` int DEFAULT NULL,
  `dataN` date DEFAULT NULL,
  `sesso` varchar(1) DEFAULT NULL,
  `cf_utente` varchar(45) NOT NULL,
  `nome` varchar(45) DEFAULT NULL,
  `cognome` varchar(45) DEFAULT NULL,
  `m_pagamento` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`cf_utente`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dump dei dati per la tabella `utente`
--

INSERT INTO `utente` (`username`, `pw`, `email`, `cell`, `citta`, `via`, `numero`, `cap`, `dataN`, `sesso`, `cf_utente`, `nome`, `cognome`, `m_pagamento`) VALUES
('federosa', 'D404559F602EAB6FD602AC7680DACBFAADD13630335E951F097AF3900E9DE176B6DB28512F2E000B9D04FBA5133E8B1C6E8DF59DB3A8AB9D60BE4B97CC9E81DB', 'test@gmail.com', '12341241', 'verceia', 'nazionale', 123, 23022, '2021-05-23', 'm', 'RSOFRC21E23C623Y', 'federico', 'rosa', ' carta prepagata');

-- --------------------------------------------------------

--
-- Struttura della tabella `utente_appartamenti`
--

CREATE TABLE IF NOT EXISTS `utente_appartamenti` (
  `idUtente_Apaprtamenti` int NOT NULL AUTO_INCREMENT,
  `dataInizio` date DEFAULT NULL,
  `dataFine` date DEFAULT NULL,
  `fk_appartamenti` int DEFAULT NULL,
  `fk_utente` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idUtente_Apaprtamenti`),
  KEY `fk_utente_idx` (`fk_utente`),
  KEY `fk_appartamenti_idx` (`fk_appartamenti`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

--
-- Limiti per le tabelle scaricate
--

--
-- Limiti per la tabella `appartamenti`
--
ALTER TABLE `appartamenti`
  ADD CONSTRAINT `fk_immagini2` FOREIGN KEY (`fk_immagini`) REFERENCES `immagini` (`id_immagini`),
  ADD CONSTRAINT `fk_immobilePrivato` FOREIGN KEY (`fk_immobilePrivato`) REFERENCES `immobile_privato` (`idimmobile`);

--
-- Limiti per la tabella `appartamenti_attturistiche`
--
ALTER TABLE `appartamenti_attturistiche`
  ADD CONSTRAINT `fk_appartamenti2` FOREIGN KEY (`fk_appartamenti`) REFERENCES `appartamenti` (`idappartamenti`),
  ADD CONSTRAINT `fk_attTuristiche` FOREIGN KEY (`fk_attTuristiche`) REFERENCES `att_turistiche` (`idatt_turistiche`);

--
-- Limiti per la tabella `appartamenti_recensioni`
--
ALTER TABLE `appartamenti_recensioni`
  ADD CONSTRAINT `fk_appartamenti3` FOREIGN KEY (`fk_appartamenti`) REFERENCES `appartamenti` (`idappartamenti`),
  ADD CONSTRAINT `fk_recensioni` FOREIGN KEY (`fk_recensioni`) REFERENCES `recensioni` (`idrecensioni`);

--
-- Limiti per la tabella `att_turistiche`
--
ALTER TABLE `att_turistiche`
  ADD CONSTRAINT `fk_immagini` FOREIGN KEY (`fk_immagini`) REFERENCES `immagini_per_attrazionituristiche` (`idimmagini_per_attrazioniTuristiche`);

--
-- Limiti per la tabella `immagini_per_appartamenti`
--
ALTER TABLE `immagini_per_appartamenti`
  ADD CONSTRAINT `fk_immagini_apparamento` FOREIGN KEY (`fkAppartamento`) REFERENCES `appartamenti` (`idappartamenti`);

--
-- Limiti per la tabella `proprietario`
--
ALTER TABLE `proprietario`
  ADD CONSTRAINT `fk_immobile_privato` FOREIGN KEY (`fk_immobilePrivato`) REFERENCES `immobile_privato` (`idimmobile`);

--
-- Limiti per la tabella `proprietario_social`
--
ALTER TABLE `proprietario_social`
  ADD CONSTRAINT `fk_proprietario` FOREIGN KEY (`fk_proprietario`) REFERENCES `proprietario` (`cf_proprietario`),
  ADD CONSTRAINT `fk_social` FOREIGN KEY (`fk_social`) REFERENCES `social` (`idsocial`);

--
-- Limiti per la tabella `servizi`
--
ALTER TABLE `servizi`
  ADD CONSTRAINT `fk_appartamenti_servizi` FOREIGN KEY (`fk_appartamenti`) REFERENCES `appartamenti` (`idappartamenti`);

--
-- Limiti per la tabella `utente_appartamenti`
--
ALTER TABLE `utente_appartamenti`
  ADD CONSTRAINT `fk_appartamenti` FOREIGN KEY (`fk_appartamenti`) REFERENCES `appartamenti` (`idappartamenti`),
  ADD CONSTRAINT `fk_utente` FOREIGN KEY (`fk_utente`) REFERENCES `utente` (`cf_utente`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
