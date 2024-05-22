-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 23, 2024 at 12:12 AM
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
  `Health` int(16) NOT NULL DEFAULT 1,
  `Damage` int(16) NOT NULL DEFAULT 1,
  `timelvl1` int(16) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tblleaderboard`
--

INSERT INTO `tblleaderboard` (`PlayerId`, `Coins`, `Health`, `Damage`, `timelvl1`) VALUES
(1, 0, 1, 1, NULL);

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
(1, 'admin', '$5$rounds=5000$eindwerksofoadmi$dLYELpjOSV2Wk2oOCUnkecWKxS6fuQn//CXFtg8dsDD', '$5$rounds=5000$eindwerksofoadmin$', 1, 0, 1, 1, 1);

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
  MODIFY `Id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
