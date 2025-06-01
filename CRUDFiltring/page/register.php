<?php
// Permitir solicitudes desde cualquier origen
header('Access-Control-Allow-Origin: *');
header('Content-Type: application/json; charset=UTF-8');
header('Access-Control-Allow-Methods: POST, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');

// Manejar la solicitud OPTIONS (preflight)
if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

// Solo procesar solicitudes POST
if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    http_response_code(405);
    echo json_encode(['success' => false, 'message' => 'Método no permitido']);
    exit();
}

// Recibir los datos del POST
$input = file_get_contents('php://input');
$data = json_decode($input);

// Verificar si los datos se decodificaron correctamente
if (json_last_error() !== JSON_ERROR_NONE) {
    http_response_code(400);
    echo json_encode(['success' => false, 'message' => 'Error en el formato JSON']);
    exit();
}

$response = array();

if(
    !empty($data->user) &&
    !empty($data->email) &&
    !empty($data->password) &&
    !empty($data->nombre) &&
    !empty($data->apellido) &&
    !empty($data->genero)
) {
    try {
        // Conexión a la base de datos
        $conn = new mysqli('localhost', 'root', '', 'filtring');

        // Verificar conexión
        if($conn->connect_error) {
            throw new Exception('Error de conexión: ' . $conn->connect_error);
        }

        // Verificar si el usuario o email ya existen
        $check_query = "SELECT * FROM Usuario WHERE User = ? OR Email = ?";
        $check_stmt = $conn->prepare($check_query);
        $check_stmt->bind_param("ss", $data->user, $data->email);
        $check_stmt->execute();
        $result = $check_stmt->get_result();

        if($result->num_rows > 0) {
            $response['success'] = false;
            $response['message'] = 'El usuario o correo electrónico ya están registrados';
            http_response_code(409); // Conflict
        } else {
            // Preparar la consulta de inserción
            $query = "INSERT INTO Usuario (User, Email, Password, Nombre, Apellido, Genero, Foto, Ubicacion) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
            
            $stmt = $conn->prepare($query);
            
            // La foto y ubicación pueden estar vacías inicialmente
            $foto = $data->foto ?? '';
            $ubicacion = $data->ubicacion ?? '';
            
            $stmt->bind_param("ssssssss", 
                $data->user,
                $data->email,
                $data->password,
                $data->nombre,
                $data->apellido,
                $data->genero,
                $foto,
                $ubicacion
            );

            if($stmt->execute()) {
                $response['success'] = true;
                $response['message'] = 'Usuario registrado exitosamente';
                http_response_code(201); // Created
            } else {
                throw new Exception('Error al registrar el usuario: ' . $stmt->error);
            }
        }

        $conn->close();
    } catch (Exception $e) {
        http_response_code(500);
        $response['success'] = false;
        $response['message'] = 'Error en el servidor: ' . $e->getMessage();
    }
} else {
    http_response_code(400);
    $response['success'] = false;
    $response['message'] = 'Datos incompletos';
}

echo json_encode($response); 