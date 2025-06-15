<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: POST, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type');
header('Access-Control-Allow-Credentials: true');

// Log de inicio
error_log("=== UPDATE_USER.PHP INICIADO ===");

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    error_log("Método no permitido: " . $_SERVER['REQUEST_METHOD']);
    http_response_code(405);
    echo json_encode(['success' => false, 'message' => 'Método no permitido']);
    exit();
}

// Obtener datos del cuerpo de la solicitud
$data = json_decode(file_get_contents('php://input'), true);
error_log("Datos recibidos: " . print_r($data, true));

// Validar datos requeridos
if (!isset($data['userId'])) {
    error_log("ID de usuario no proporcionado");
    http_response_code(400);
    echo json_encode(['success' => false, 'message' => 'ID de usuario no proporcionado']);
    exit();
}

try {
    error_log("Conectando a la base de datos...");
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if ($conn->connect_error) {
        error_log("Error de conexión: " . $conn->connect_error);
        throw new Exception('Error de conexión a la base de datos');
    }

    // Escapar y validar datos
    $userId = $conn->real_escape_string($data['userId']);
    $user = isset($data['user']) ? $conn->real_escape_string($data['user']) : null;
    $email = isset($data['email']) ? $conn->real_escape_string($data['email']) : null;
    $nombre = isset($data['nombre']) ? $conn->real_escape_string($data['nombre']) : null;
    $apellido = isset($data['apellido']) ? $conn->real_escape_string($data['apellido']) : null;
    $genero = isset($data['genero']) ? $conn->real_escape_string($data['genero']) : null;
    $ubicacion = isset($data['ubicacion']) ? $conn->real_escape_string($data['ubicacion']) : null;
    
    // Verificar si el nuevo username o email ya existen (si se están actualizando)
    if ($user !== null) {
        $checkUser = $conn->prepare("SELECT ID FROM usuario WHERE User = ? AND ID != ?");
        $checkUser->bind_param("si", $user, $userId);
        $checkUser->execute();
        if ($checkUser->get_result()->num_rows > 0) {
            throw new Exception('El nombre de usuario ya está en uso');
        }
    }

    if ($email !== null) {
        $checkEmail = $conn->prepare("SELECT ID FROM usuario WHERE Email = ? AND ID != ?");
        $checkEmail->bind_param("si", $email, $userId);
        $checkEmail->execute();
        if ($checkEmail->get_result()->num_rows > 0) {
            throw new Exception('El email ya está registrado');
        }
    }

    // Construir la consulta SQL dinámicamente
    $updateFields = [];
    $types = "";
    $params = [];

    if ($user !== null) {
        $updateFields[] = "User = ?";
        $types .= "s";
        $params[] = $user;
    }
    if ($email !== null) {
        $updateFields[] = "Email = ?";
        $types .= "s";
        $params[] = $email;
    }
    if ($nombre !== null) {
        $updateFields[] = "Nombre = ?";
        $types .= "s";
        $params[] = $nombre;
    }
    if ($apellido !== null) {
        $updateFields[] = "Apellido = ?";
        $types .= "s";
        $params[] = $apellido;
    }
    if ($genero !== null) {
        $updateFields[] = "Genero = ?";
        $types .= "s";
        $params[] = $genero;
    }
    if ($ubicacion !== null) {
        $updateFields[] = "Ubicacion = ?";
        $types .= "s";
        $params[] = $ubicacion;
    }

    // Añadir el ID al final de los parámetros
    $types .= "i";
    $params[] = $userId;

    if (empty($updateFields)) {
        throw new Exception('No se proporcionaron campos para actualizar');
    }

    $query = "UPDATE usuario SET " . implode(", ", $updateFields) . " WHERE ID = ?";
    $stmt = $conn->prepare($query);

    // Bind parameters dinámicamente
    $bindParams = array_merge([$types], $params);
    $stmt->bind_param(...$bindParams);

    if ($stmt->execute()) {
        error_log("Usuario actualizado correctamente");
        echo json_encode([
            'success' => true,
            'message' => 'Usuario actualizado correctamente'
        ]);
    } else {
        error_log("Error al actualizar el usuario: " . $stmt->error);
        throw new Exception('Error al actualizar el usuario');
    }

} catch (Exception $e) {
    error_log("Error en update_user.php: " . $e->getMessage());
    http_response_code(500);
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
    error_log("=== UPDATE_USER.PHP TERMINADO ===");
}