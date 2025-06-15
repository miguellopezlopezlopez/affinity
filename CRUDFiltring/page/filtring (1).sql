-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 01-06-2025 a las 19:42:56
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `filtring`
--

-- Función para hashear contraseñas usando SHA2-256 (como ejemplo seguro)
DELIMITER $$
CREATE FUNCTION hash_password(plain_password VARCHAR(255)) 
RETURNS VARCHAR(255)
DETERMINISTIC
BEGIN
    RETURN SHA2(plain_password, 256);
END$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `fotosperfil`
--

CREATE TABLE `fotosperfil` (
  `ID` int(11) NOT NULL,
  `ID_User` int(11) NOT NULL,
  `URL` varchar(255) NOT NULL,
  `Tipo` enum('principal','galeria') NOT NULL,
  `FechaSubida` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `matches`
--

CREATE TABLE `matches` (
  `ID_Match` int(11) NOT NULL,
  `ID_Acept` int(11) NOT NULL,
  `ID_Sol` int(11) NOT NULL,
  `Fecha_Solicitud` datetime NOT NULL DEFAULT current_timestamp(),
  `Fecha_Aceptado` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Disparadores `matches`
--
DELIMITER $$
CREATE TRIGGER `prevent_self_match` BEFORE INSERT ON `matches` FOR EACH ROW BEGIN
    IF NEW.ID_Acept = NEW.ID_Sol THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Un usuario no puede hacer match consigo mismo';
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `mensaje`
--

CREATE TABLE `mensaje` (
  `ID_Mensaje` int(11) NOT NULL,
  `ID_Match` int(11) NOT NULL,
  `Fecha_Hora` datetime NOT NULL DEFAULT current_timestamp(),
  `Contenido` varchar(200) NOT NULL,
  `ID_Emisor` int(11) NOT NULL,
  `ID_Receptor` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `perfil`
--

CREATE TABLE `perfil` (
  `ID_Perfil` int(11) NOT NULL,
  `ID_User` int(11) NOT NULL,
  `Biografia` text NOT NULL,
  `Intereses` text NOT NULL,
  `Preferencias` text NOT NULL,
  `FotoPrincipal` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `ID` int(11) NOT NULL,
  `User` varchar(20) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Genero` varchar(20) NOT NULL,
  `Foto` text NOT NULL,
  `Ubicacion` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Trigger para hashear contraseñas antes de insertar
DELIMITER $$
CREATE TRIGGER `hash_password_before_insert` 
BEFORE INSERT ON `usuario`
FOR EACH ROW
BEGIN
    SET NEW.Password = hash_password(NEW.Password);
END$$
DELIMITER ;

-- Trigger para hashear contraseñas antes de actualizar
DELIMITER $$
CREATE TRIGGER `hash_password_before_update` 
BEFORE UPDATE ON `usuario`
FOR EACH ROW
BEGIN
    IF NEW.Password != OLD.Password THEN
        SET NEW.Password = hash_password(NEW.Password);
    END IF;
END$$
DELIMITER ;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`ID`, `User`, `Email`, `Password`, `Nombre`, `Apellido`, `Genero`, `Foto`, `Ubicacion`) VALUES
(1, 'admin', 'admin@affinity.com', 'administrador', 'Admin', 'User', 'Otro', '', 'Default Location');

-- Actualizar la contraseña del admin existente con hash
UPDATE usuario SET Password = hash_password('administrador') WHERE ID = 1;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `fotosperfil`
--
ALTER TABLE `fotosperfil`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `ID_User` (`ID_User`);

--
-- Indices de la tabla `matches`
--
ALTER TABLE `matches`
  ADD PRIMARY KEY (`ID_Match`),
  ADD KEY `ID_Acept` (`ID_Acept`),
  ADD KEY `ID_Sol` (`ID_Sol`);

--
-- Indices de la tabla `mensaje`
--
ALTER TABLE `mensaje`
  ADD PRIMARY KEY (`ID_Mensaje`),
  ADD KEY `ID_Match` (`ID_Match`),
  ADD KEY `ID_Emisor` (`ID_Emisor`),
  ADD KEY `ID_Receptor` (`ID_Receptor`);

--
-- Indices de la tabla `perfil`
--
ALTER TABLE `perfil`
  ADD PRIMARY KEY (`ID_Perfil`),
  ADD KEY `ID_User` (`ID_User`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `User` (`User`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD UNIQUE KEY `unique_user` (`User`),
  ADD UNIQUE KEY `unique_email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `fotosperfil`
--
ALTER TABLE `fotosperfil`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `matches`
--
ALTER TABLE `matches`
  MODIFY `ID_Match` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `mensaje`
--
ALTER TABLE `mensaje`
  MODIFY `ID_Mensaje` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `perfil`
--
ALTER TABLE `perfil`
  MODIFY `ID_Perfil` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `fotosperfil`
--
ALTER TABLE `fotosperfil`
  ADD CONSTRAINT `fotosperfil_ibfk_1` FOREIGN KEY (`ID_User`) REFERENCES `usuario` (`ID`) ON DELETE CASCADE;

--
-- Filtros para la tabla `matches`
--
ALTER TABLE `matches`
  ADD CONSTRAINT `matches_ibfk_1` FOREIGN KEY (`ID_Acept`) REFERENCES `usuario` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `matches_ibfk_2` FOREIGN KEY (`ID_Sol`) REFERENCES `usuario` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `mensaje`
--
ALTER TABLE `mensaje`
  ADD CONSTRAINT `mensaje_ibfk_1` FOREIGN KEY (`ID_Match`) REFERENCES `matches` (`ID_Match`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `mensaje_ibfk_2` FOREIGN KEY (`ID_Emisor`) REFERENCES `usuario` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `mensaje_ibfk_3` FOREIGN KEY (`ID_Receptor`) REFERENCES `usuario` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `perfil`
--
ALTER TABLE `perfil`
  ADD CONSTRAINT `perfil_ibfk_1` FOREIGN KEY (`ID_User`) REFERENCES `usuario` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */; 