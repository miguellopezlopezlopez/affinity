<?php
// Habilitar errores para debugging
error_reporting(E_ALL);
ini_set('display_errors', 1);

session_start();

// Headers CORS
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: http://localhost');
header('Access-Control-Allow-Methods: GET, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type, Accept, Authorization');
header('Access-Control-Allow-Credentials: true');

// Manejar preflight OPTIONS request
if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

// Log de debugging
error_log("=== CHECK_SESSION.PHP INICIADO ===");
error_log("Session ID: " . session_id());
error_log("Session data: " . print_r($_SESSION, true));

// Verificar si hay sesión activa
if (isset($_SESSION['user_id']) && !empty($_SESSION['user_id'])) {
    // Verificar que el usuario aún existe en la base de datos
    $host = 'localhost';
    $db   = 'filtring';
    $user = 'root';
    $pass = '';
    $charset = 'utf8mb4';

    $dsn = "mysql:host=$host;dbname=$db;charset=$charset";
    $options = [
        PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
        PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
        PDO::ATTR_EMULATE_PREPARES   => false,
    ];

    try {
        $pdo = new PDO($dsn, $user, $pass, $options);
        
        $stmt = $pdo->prepare('SELECT ID, User, Email, Nombre, Apellido FROM Usuario WHERE ID = ?');
        $stmt->execute([$_SESSION['user_id']]);
        $userData = $stmt->fetch();
        
        if ($userData) {
            error_log("Usuario autenticado: " . $userData['User']);
            echo json_encode([
                'authenticated' => true,
                'user' => [
                    'id' => $userData['ID'],
                    'username' => $userData['User'],
                    'email' => $userData['Email'],
                    'nombre_completo' => $userData['Nombre'] . ' ' . $userData['Apellido']
                ]
            ]);
        } else {
            error_log("Usuario no encontrado en BD, limpiando sesión");
            session_destroy();
            echo json_encode([
                'authenticated' => false,
                'message' => 'Usuario no encontrado'
            ]);
        }
    } catch (PDOException $e) {
        error_log("Error de BD en check_session: " . $e->getMessage());
        echo json_encode([
            'authenticated' => false,
            'message' => 'Error de base de datos'
        ]);
    }
} else {
    error_log("No hay sesión activa");
    echo json_encode([
        'authenticated' => false,
        'message' => 'No hay sesión activa'
    ]);
}

error_log("=== CHECK_SESSION.PHP TERMINADO ===");
?>