<?php
// Habilitar TODOS los errores para debugging
error_reporting(E_ALL);
ini_set('display_errors', 1);

session_start();

// Log de debugging
error_log("=== LOGIN.PHP INICIADO ===");
error_log("Método: " . $_SERVER['REQUEST_METHOD']);
error_log("Headers recibidos: " . print_r(getallheaders(), true));

// Headers CORS más permisivos para desarrollo
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: http://localhost');
header('Access-Control-Allow-Methods: POST, OPTIONS, GET');
header('Access-Control-Allow-Headers: Content-Type, Accept, Authorization');
header('Access-Control-Allow-Credentials: true');

// Manejar preflight OPTIONS request
if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    error_log("Preflight OPTIONS request manejado");
    http_response_code(200);
    exit();
}

// Verificar que sea POST
if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    error_log("Método no permitido: " . $_SERVER['REQUEST_METHOD']);
    echo json_encode([
        'success' => false,
        'message' => 'Método no permitido'
    ]);
    exit;
}

// Obtener los datos del POST
$input = file_get_contents('php://input');
error_log("Raw input: " . $input);

$data = json_decode($input, true);
error_log("Datos decodificados: " . print_r($data, true));

if (!$data || !isset($data['email']) || !isset($data['password'])) {
    error_log("Datos faltantes - data: " . print_r($data, true));
    echo json_encode([
        'success' => false,
        'message' => 'Por favor, proporciona usuario/email y contraseña',
        'debug' => [
            'received_data' => $data,
            'raw_input' => $input
        ]
    ]);
    exit;
}

$emailOrUser = trim($data['email']);
$password = $data['password'];

if (empty($emailOrUser) || empty($password)) {
    error_log("Campos vacíos - email: '$emailOrUser', password: " . (empty($password) ? 'empty' : 'not empty'));
    echo json_encode([
        'success' => false,
        'message' => 'Usuario/Email y contraseña son requeridos'
    ]);
    exit;
}

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
    error_log("Intentando conectar a la base de datos...");
    $pdo = new PDO($dsn, $user, $pass, $options);
    error_log("Conexión a BD exitosa");

    // Buscar usuario por email o nombre de usuario y contraseña
    $stmt = $pdo->prepare('SELECT ID, User, Email, Nombre, Apellido FROM Usuario WHERE (Email = ? OR User = ?) AND Password = ?');
    error_log("Query preparada: SELECT ID, User, Email, Nombre, Apellido FROM Usuario WHERE (Email = ? OR User = ?) AND Password = ?");
    error_log("Parámetros: email/user = '$emailOrUser', password = '[OCULTO]'");
    
    $stmt->execute([$emailOrUser, $emailOrUser, $password]);
    $user = $stmt->fetch();
    
    error_log("Resultado de la consulta: " . print_r($user, true));

    if ($user) {
        // Guardar datos en la sesión
        $_SESSION['user_id'] = $user['ID'];
        $_SESSION['username'] = $user['User'];
        $_SESSION['email'] = $user['Email'];
        $_SESSION['nombre_completo'] = $user['Nombre'] . ' ' . $user['Apellido'];

        error_log("Sesión creada exitosamente para usuario ID: " . $user['ID']);
        error_log("Datos de sesión: " . print_r($_SESSION, true));

        $response = [
            'success' => true,
            'message' => '¡Inicio de sesión exitoso!',
            'user' => [
                'id' => $user['ID'],
                'username' => $user['User'],
                'email' => $user['Email'],
                'nombre_completo' => $user['Nombre'] . ' ' . $user['Apellido']
            ],
            'redirect' => 'profile.html?userId=' . $user['ID'],
            'session_id' => session_id()
        ];
        
        error_log("Respuesta exitosa: " . json_encode($response));
        echo json_encode($response);
    } else {
        error_log("Usuario no encontrado o contraseña incorrecta");
        echo json_encode([
            'success' => false,
            'message' => 'Usuario/Email o contraseña incorrectos'
        ]);
    }
} catch (PDOException $e) {
    // Log del error para debugging
    error_log("Error de base de datos: " . $e->getMessage());
    error_log("Stack trace: " . $e->getTraceAsString());

    echo json_encode([
        'success' => false,
        'message' => 'Error de conexión a la base de datos',
        'debug' => [
            'error' => $e->getMessage(),
            'dsn' => $dsn
        ]
    ]);
} catch (Exception $e) {
    // Log de errores generales
    error_log("Error general: " . $e->getMessage());
    error_log("Stack trace: " . $e->getTraceAsString());

    echo json_encode([
        'success' => false,
        'message' => 'Error interno del servidor',
        'debug' => [
            'error' => $e->getMessage()
        ]
    ]);
}

error_log("=== LOGIN.PHP TERMINADO ===");
?>