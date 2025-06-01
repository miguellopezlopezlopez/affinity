<?php
session_start();
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: http://localhost');
header('Access-Control-Allow-Methods: POST');
header('Access-Control-Allow-Headers: Content-Type');
header('Access-Control-Allow-Credentials: true');

// Obtener los datos del POST
$data = json_decode(file_get_contents('php://input'), true);

if (!$data || !isset($data['email']) || !isset($data['password'])) {
    echo json_encode([
        'success' => false,
        'message' => 'Por favor, proporciona usuario/email y contraseña'
    ]);
    exit;
}

$emailOrUser = $data['email'];
$password = $data['password'];

// Conexión a la base de datos
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
    
    // Buscar usuario por email o nombre de usuario y contraseña
    $stmt = $pdo->prepare('SELECT ID, User, Email, Nombre, Apellido FROM Usuario WHERE (Email = ? OR User = ?) AND Password = ?');
    $stmt->execute([$emailOrUser, $emailOrUser, $password]);
    $user = $stmt->fetch();

    if ($user) {
        // Guardar datos en la sesión
        $_SESSION['user_id'] = $user['ID'];
        $_SESSION['username'] = $user['User'];
        $_SESSION['email'] = $user['Email'];
        $_SESSION['nombre_completo'] = $user['Nombre'] . ' ' . $user['Apellido'];

        echo json_encode([
            'success' => true,
            'message' => '¡Inicio de sesión exitoso!',
            'user' => [
                'id' => $user['ID'],
                'username' => $user['User'],
                'email' => $user['Email'],
                'nombre_completo' => $user['Nombre'] . ' ' . $user['Apellido']
            ],
            'redirect' => 'stats.html?userId=' . $user['ID']
        ]);
    } else {
        echo json_encode([
            'success' => false,
            'message' => 'Usuario/Email o contraseña incorrectos'
        ]);
    }
} catch (PDOException $e) {
    echo json_encode([
        'success' => false,
        'message' => 'Error de conexión a la base de datos'
    ]);
} 