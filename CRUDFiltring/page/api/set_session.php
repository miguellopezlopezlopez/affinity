<?php
header('Content-Type: application/json');
session_start();

// Verificar que se recibió el userId
if (!isset($_GET['userId'])) {
    echo json_encode(['success' => false, 'message' => 'UserId no proporcionado']);
    exit;
}

$userId = intval($_GET['userId']);

// Establecer la sesión
$_SESSION['userId'] = $userId;

// También establecer una cookie para JavaScript
setcookie('userId', $userId, [
    'expires' => time() + 86400,
    'path' => '/',
    'secure' => true,
    'httponly' => false,
    'samesite' => 'Strict'
]);

echo json_encode(['success' => true, 'message' => 'Sesión establecida correctamente']); 