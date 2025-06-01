<?php
header('Content-Type: application/json');
session_start();

// Destruir la sesión
session_destroy();

// Eliminar la cookie
setcookie('userId', '', [
    'expires' => time() - 3600,
    'path' => '/',
    'secure' => true,
    'httponly' => false,
    'samesite' => 'Strict'
]);

echo json_encode(['success' => true, 'message' => 'Sesión cerrada correctamente']); 