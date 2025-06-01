-- Añadir campo FotoPrincipal a la tabla Perfil
ALTER TABLE Perfil ADD COLUMN FotoPrincipal VARCHAR(255) NULL;

-- Crear tabla para las fotos adicionales del perfil
CREATE TABLE IF NOT EXISTS FotosPerfil (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    ID_User INT NOT NULL,
    URL VARCHAR(255) NOT NULL,
    Tipo ENUM('principal', 'galeria') NOT NULL,
    FechaSubida TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ID_User) REFERENCES Usuario(ID) ON DELETE CASCADE
); 