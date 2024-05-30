-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 30, 2024 at 02:04 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `eindwerksofo`
--

-- --------------------------------------------------------

--
-- Table structure for table `tblleaderboard`
--

CREATE TABLE `tblleaderboard` (
  `PlayerId` int(11) NOT NULL,
  `Coins` int(11) NOT NULL DEFAULT 0,
  `timelvl1` int(16) DEFAULT NULL,
  `timelvl2` int(16) DEFAULT NULL,
  `timelvl3` int(16) DEFAULT NULL,
  `timelvl4` int(16) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tblleaderboard`
--

INSERT INTO `tblleaderboard` (`PlayerId`, `Coins`, `timelvl1`, `timelvl2`, `timelvl3`, `timelvl4`) VALUES
(1, 10, 91, 15, NULL, NULL),
(2, 0, 54, NULL, NULL, NULL),
(3, 234, NULL, NULL, NULL, NULL),
(4, 5600, 80, NULL, NULL, NULL),
(5, 10000, 65, NULL, NULL, NULL),
(6, 3000, 44, NULL, NULL, NULL),
(7, 156, 39, 44, NULL, NULL),
(8, 0, NULL, NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `tblplayers`
--

CREATE TABLE `tblplayers` (
  `Id` int(10) NOT NULL,
  `Username` varchar(30) NOT NULL,
  `hashh` varchar(100) NOT NULL,
  `salt` varchar(50) NOT NULL,
  `adminn` int(10) NOT NULL DEFAULT 0,
  `coins` int(16) NOT NULL DEFAULT 0,
  `Damage` int(11) NOT NULL DEFAULT 1,
  `Speed` int(11) NOT NULL DEFAULT 1,
  `Health` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `tblplayers`
--

INSERT INTO `tblplayers` (`Id`, `Username`, `hashh`, `salt`, `adminn`, `coins`, `Damage`, `Speed`, `Health`) VALUES
(1, 'admin', '$5$rounds=5000$eindwerksofoadmi$dLYELpjOSV2Wk2oOCUnkecWKxS6fuQn//CXFtg8dsDD', '$5$rounds=5000$eindwerksofoadmin$', 1, 10, 10, 30, 6),
(4, 'cva', '$5$rounds=5000$eindwerksofocva$y7fr0azFY0x/ACyIGOmTF5KA6plvJ4HMMbaiM6da2J0', '$5$rounds=5000$eindwerksofocva$', 0, 5600, 3, 1, 4),
(5, 'snappie', '$5$rounds=5000$eindwerksofosnap$sPDYhBv.3xaiwLNBuk7eM/pE.xHugkJkD7JReCLqKZ5', '$5$rounds=5000$eindwerksofosnappie$', 1, 10000, 1, 1, 5),
(7, 'pompelmoes', '$5$rounds=5000$eindwerksofopomp$soLV3A/E8srmvSHed9ePctlJeclPkdSxEdYB8s39IY6', '$5$rounds=5000$eindwerksofopompelmoes$', 0, 156, 1, 1, 1),
(8, 'sarah', '$5$rounds=5000$eindwerksofosara$OaMV/jekaShAx9kbMGs/0cgYK0M35UPFhbAH100hUC7', '$5$rounds=5000$eindwerksofosarah$', 0, 0, 1, 1, 1);

--
-- Triggers `tblplayers`
--
DELIMITER $$
CREATE TRIGGER `update_leaderboard_coins` AFTER UPDATE ON `tblplayers` FOR EACH ROW BEGIN
            UPDATE tblleaderboard
            SET Coins = NEW.coins
            WHERE PlayerId = NEW.Id;
        END
$$
DELIMITER ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tblleaderboard`
--
ALTER TABLE `tblleaderboard`
  ADD PRIMARY KEY (`PlayerId`);

--
-- Indexes for table `tblplayers`
--
ALTER TABLE `tblplayers`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Username` (`Username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tblplayers`
--
ALTER TABLE `tblplayers`
  MODIFY `Id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
