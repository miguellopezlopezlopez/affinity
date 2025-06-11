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

        // Iniciar transacción
        $conn->begin_transaction();

        try {
            // Verificar si el usuario o email ya existen
            $check_query = "SELECT * FROM Usuario WHERE User = ? OR Email = ?";
            $check_stmt = $conn->prepare($check_query);
            $check_stmt->bind_param("ss", $data->user, $data->email);
            $check_stmt->execute();
            $result = $check_stmt->get_result();

            if($result->num_rows > 0) {
                throw new Exception('El usuario o correo electrónico ya están registrados');
            }

            // Preparar la consulta de inserción de usuario
            $query = "INSERT INTO Usuario (User, Email, Password, Nombre, Apellido, Genero, Ubicacion) VALUES (?, ?, ?, ?, ?, ?, ?)";
            
            $stmt = $conn->prepare($query);
            
            // La ubicación puede estar vacía inicialmente
            $ubicacion = $data->ubicacion ?? '';
            
            $stmt->bind_param("sssssss", 
                $data->user,
                $data->email,
                $data->password,
                $data->nombre,
                $data->apellido,
                $data->genero,
                $ubicacion
            );

            if(!$stmt->execute()) {
                throw new Exception('Error al registrar el usuario: ' . $stmt->error);
            }

            // Obtener el ID del usuario recién insertado
            $userId = $conn->insert_id;

            // Crear entrada en la tabla Perfil
            $perfil_query = "INSERT INTO Perfil (ID_User) VALUES (?)";
            $perfil_stmt = $conn->prepare($perfil_query);
            $perfil_stmt->bind_param("i", $userId);
            
            if(!$perfil_stmt->execute()) {
                throw new Exception('Error al crear el perfil del usuario');
            }

            // Si todo salió bien, confirmar la transacción
            $conn->commit();

            $response['success'] = true;
            $response['message'] = 'Usuario registrado exitosamente';
            $response['userId'] = $userId;
            http_response_code(201); // Created

        } catch (Exception $e) {
            // Si algo salió mal, revertir la transacción
            $conn->rollback();
            throw $e;
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