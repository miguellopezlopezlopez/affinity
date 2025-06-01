<?php
session_start();
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: http://localhost');
header('Access-Control-Allow-Methods: POST');
header('Access-Control-Allow-Headers: Content-Type');
header('Access-Control-Allow-Credentials: true');

// Obtener los datos del POST
$data = json_decode(file_get_contents('php://input'), true);

if (!isset($data['email']) || !isset($data['newPassword']) || !isset($data['verificationCode'])) {
    echo json_encode([
        'success' => false,
        'message' => 'Datos incompletos'
    ]);
    exit;
}

$email = filter_var($data['email'], FILTER_SANITIZE_EMAIL);
$newPassword = $data['newPassword'];
$verificationCode = $data['verificationCode'];

// Verificar el código
if (!isset($_SESSION['verification_code'])) {
    echo json_encode([
        'success' => false,
        'message' => 'No hay un código de verificación activo'
    ]);
    exit;
}

$verification = $_SESSION['verification_code'];

// Verificar si el código ha expirado
if ($verification['expires'] < time()) {
    unset($_SESSION['verification_code']);
    echo json_encode([
        'success' => false,
        'message' => 'El código de verificación ha expirado. Por favor, solicita uno nuevo.'
    ]);
    exit;
}

// Verificar el email
if ($verification['email'] !== $email) {
    echo json_encode([
        'success' => false,
        'message' => 'El email no coincide con el código de verificación'
    ]);
    exit;
}

// Incrementar contador de intentos
$_SESSION['verification_code']['attempts']++;

// Verificar número de intentos (máximo 3)
if ($_SESSION['verification_code']['attempts'] > 3) {
    unset($_SESSION['verification_code']);
    echo json_encode([
        'success' => false,
        'message' => 'Demasiados intentos fallidos. Por favor, solicita un nuevo código.'
    ]);
    exit;
}

// Verificar el código
if ($verification['code'] != $verificationCode) {
    echo json_encode([
        'success' => false,
        'message' => 'Código de verificación incorrecto'
    ]);
    exit;
}

try {
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if($conn->connect_error) {
        throw new Exception('Error de conexión: ' . $conn->connect_error);
    }

    // Validar que el email existe
    $checkStmt = $conn->prepare('SELECT ID FROM Usuario WHERE Email = ?');
    $checkStmt->bind_param('s', $email);
    $checkStmt->execute();
    $result = $checkStmt->get_result();
    
    if ($result->num_rows === 0) {
        throw new Exception('No existe una cuenta con este email');
    }

    // Actualizar la contraseña
    $stmt = $conn->prepare('UPDATE Usuario SET Password = ? WHERE Email = ?');
    $stmt->bind_param('ss', $newPassword, $email);
    
    if ($stmt->execute()) {
        // Borrar el código de verificación una vez usado
        unset($_SESSION['verification_code']);
        
        echo json_encode([
            'success' => true,
            'message' => 'Contraseña actualizada correctamente'
        ]);
    } else {
        throw new Exception('Error al actualizar la contraseña');
    }

} catch (Exception $e) {
    echo json_encode([
        'success' => false,
        'message' => 'Error en el servidor: ' . $e->getMessage()
    ]);
} 