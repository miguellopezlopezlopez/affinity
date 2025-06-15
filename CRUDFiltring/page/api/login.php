<?php
// Habilitar TODOS los errores para debugging
error_reporting(E_ALL);
ini_set('display_errors', 0);

// Headers CORS y HTTP necesarios
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: POST, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type, Accept');
header('Content-Type: application/json; charset=utf-8');

// Log de inicio de solicitud
error_log("Iniciando solicitud de login");

// Manejar preflight requests
if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

// Verificar método POST
if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    error_log("Método no permitido: " . $_SERVER['REQUEST_METHOD']);
    http_response_code(405);
    echo json_encode([
        'success' => false,
        'message' => 'Método no permitido'
    ]);
    exit();
}

try {
    require_once 'config.php';
    
    // Obtener y loguear los datos del POST
    $input = file_get_contents('php://input');
    error_log("Datos recibidos: " . $input);
    
    $data = json_decode($input, true);

    if (!$data || !isset($data['user']) || !isset($data['password'])) {
        error_log("Datos inválidos recibidos");
        throw new Exception('Por favor, proporciona usuario/email y contraseña');
    }

    $user = trim($data['user']);
    $password = trim($data['password']);

    error_log("Procesando login para usuario: " . $user);

    if (empty($user) || empty($password)) {
        throw new Exception('Usuario/Email y contraseña son requeridos');
    }

    // Verificar si es email o nombre de usuario
    $isEmail = filter_var($user, FILTER_VALIDATE_EMAIL);
    
    // Preparar la consulta usando prepared statements
    $sql = "SELECT ID, User, Email, Nombre, Apellido, Genero, Ubicacion, Foto 
            FROM usuario 
            WHERE " . ($isEmail ? "Email = ?" : "User = ?") . 
            " AND Password = (SELECT hash_password(?))";

    error_log("SQL Query: " . $sql);

    $stmt = $conn->prepare($sql);
    if (!$stmt) {
        error_log("Error en prepare: " . $conn->error);
        throw new Exception('Error preparando la consulta: ' . $conn->error);
    }

    // Bind parameters
    $stmt->bind_param("ss", $user, $password);
    
    // Ejecutar la consulta
    if (!$stmt->execute()) {
        error_log("Error en execute: " . $stmt->error);
        throw new Exception('Error ejecutando la consulta: ' . $stmt->error);
    }

    $result = $stmt->get_result();
    error_log("Número de resultados: " . $result->num_rows);

    if ($result->num_rows === 1) {
        $user_data = $result->fetch_assoc();
        error_log("Datos del usuario encontrado: " . json_encode($user_data));
        
        // Construir URL completa para la foto si existe
        if (!empty($user_data['Foto']) && strpos($user_data['Foto'], 'http') !== 0) {
            $protocol = isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? 'https' : 'http';
            $host = $_SERVER['HTTP_HOST'];
            $user_data['Foto'] = "$protocol://$host/page/" . $user_data['Foto'];
        }

        // Iniciar sesión y guardar datos
        session_start();
        $_SESSION['user_id'] = $user_data['ID'];
        $_SESSION['user'] = $user_data['User'];
        
        // Preparar respuesta
        $response = [
            'success' => true, 
            'message' => 'Login exitoso',
            'user' => [
                'id' => intval($user_data['ID']),
                'username' => $user_data['User'],
                'email' => $user_data['Email'],
                'nombre' => $user_data['Nombre'],
                'apellido' => $user_data['Apellido'],
                'genero' => $user_data['Genero'],
                'ubicacion' => $user_data['Ubicacion'],
                'foto' => $user_data['Foto']
            ]
        ];

        // Log de la respuesta
        error_log("Enviando respuesta exitosa: " . json_encode($response));
        echo json_encode($response);

    } else {
        error_log("No se encontró el usuario: " . $user);
        http_response_code(401);
        echo json_encode([
            'success' => false, 
            'message' => 'Usuario o contraseña incorrectos'
        ]);
    }

} catch (Exception $e) {
    error_log("Error en login.php: " . $e->getMessage());
    http_response_code(400);
    echo json_encode([
        'success' => false,
        'message' => $e->getMessage()
    ]);
} finally {
    if (isset($stmt)) {
        $stmt->close();
    }
    if (isset($conn)) {
        $conn->close();
    }
}
?> 