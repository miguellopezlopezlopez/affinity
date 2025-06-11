<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: POST, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type');
header('Access-Control-Allow-Credentials: true');

// Log de inicio
error_log("=== DELETE_PHOTO.PHP INICIADO ===");

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
if (!isset($data['photoId'])) {
    error_log("ID de foto no proporcionado");
    http_response_code(400);
    echo json_encode(['success' => false, 'message' => 'ID de foto no proporcionado']);
    exit();
}

try {
    error_log("Conectando a la base de datos...");
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if ($conn->connect_error) {
        error_log("Error de conexión: " . $conn->connect_error);
        throw new Exception('Error de conexión a la base de datos');
    }

    // Obtener la URL de la foto antes de eliminarla
    $query = "SELECT URL FROM FotosPerfil WHERE ID = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("i", $data['photoId']);
    $stmt->execute();
    $result = $stmt->get_result();
    
    if ($row = $result->fetch_assoc()) {
        $photoUrl = $row['URL'];
        $filePath = '../' . $photoUrl;
        
        // Eliminar el registro de la base de datos
        $deleteQuery = "DELETE FROM FotosPerfil WHERE ID = ?";
        $stmt = $conn->prepare($deleteQuery);
        $stmt->bind_param("i", $data['photoId']);
        
        if ($stmt->execute()) {
            // Intentar eliminar el archivo físico
            if (file_exists($filePath)) {
                unlink($filePath);
                error_log("Archivo físico eliminado: $filePath");
            }
            
            echo json_encode([
                'success' => true,
                'message' => 'Foto eliminada correctamente'
            ]);
        } else {
            throw new Exception('Error al eliminar la foto de la base de datos');
        }
    } else {
        throw new Exception('Foto no encontrada');
    }

} catch (Exception $e) {
    error_log("Error en delete_photo.php: " . $e->getMessage());
    http_response_code(500);
    echo json_encode([
        'success' => false,
        'message' => $e->getMessage()
    ]);
} finally {
    if (isset($conn)) {
        $conn->close();
    }
    error_log("=== DELETE_PHOTO.PHP TERMINADO ===");
} 