<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: POST, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type');
header('Access-Control-Allow-Credentials: true');

// Log de inicio
error_log("=== UPLOAD_PHOTO.PHP INICIADO ===");
error_log("POST data: " . print_r($_POST, true));
error_log("FILES data: " . print_r($_FILES, true));

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

// Validar datos requeridos
$missingFields = [];
if (!isset($_POST['userId'])) $missingFields[] = 'userId';
if (!isset($_FILES['foto'])) $missingFields[] = 'foto';
if (!isset($_POST['tipo'])) $missingFields[] = 'tipo';

if (!empty($missingFields)) {
    error_log("Faltan campos requeridos: " . implode(', ', $missingFields));
    http_response_code(400);
    echo json_encode([
        'success' => false,
        'message' => 'Faltan datos requeridos: ' . implode(', ', $missingFields)
    ]);
    exit();
}

try {
    error_log("Conectando a la base de datos...");
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if ($conn->connect_error) {
        error_log("Error de conexión a la BD: " . $conn->connect_error);
        throw new Exception('Error de conexión: ' . $conn->connect_error);
    }

    $userId = $conn->real_escape_string($_POST['userId']);
    $tipo = $conn->real_escape_string($_POST['tipo']);

    error_log("userId: $userId, tipo: $tipo");

    // Verificar límite de fotos para galería
    if ($tipo === 'galeria') {
        $queryCount = "SELECT COUNT(*) as total FROM FotosPerfil WHERE ID_User = ? AND Tipo = 'galeria'";
        $stmtCount = $conn->prepare($queryCount);
        $stmtCount->bind_param("i", $userId);
        $stmtCount->execute();
        $resultCount = $stmtCount->get_result();
        $count = $resultCount->fetch_assoc()['total'];

        error_log("Número actual de fotos en galería: $count");

        if ($count >= 5) {
            error_log("Límite de fotos alcanzado para el usuario $userId");
            throw new Exception('Has alcanzado el límite máximo de 5 fotos en la galería');
        }
    }

    // Procesar la foto
    $file = $_FILES['foto'];
    $fileName = $file['name'];
    $fileType = strtolower(pathinfo($fileName, PATHINFO_EXTENSION));
    $allowedTypes = ['jpg', 'jpeg', 'png', 'gif'];

    error_log("Procesando archivo: $fileName, tipo: $fileType");

    if (!in_array($fileType, $allowedTypes)) {
        error_log("Tipo de archivo no permitido: $fileType");
        throw new Exception('Tipo de archivo no permitido. Solo se permiten: ' . implode(', ', $allowedTypes));
    }

    // Crear directorio si no existe
    $uploadDir = '../uploads/';
    if (!file_exists($uploadDir)) {
        error_log("Creando directorio de uploads...");
        if (!mkdir($uploadDir, 0777, true)) {
            error_log("Error al crear directorio de uploads");
            throw new Exception('Error al crear directorio para subidas');
        }
    }

    // Generar nombre único para el archivo
    $newFileName = uniqid() . '.' . $fileType;
    $targetPath = $uploadDir . $newFileName;
    $webPath = 'uploads/' . $newFileName;

    error_log("Intentando mover archivo a: $targetPath");

    if (move_uploaded_file($file['tmp_name'], $targetPath)) {
        error_log("Archivo movido exitosamente");

        if ($tipo === 'principal') {
            // Actualizar foto principal en la tabla Perfil
            $query = "UPDATE Perfil SET FotoPrincipal = ? WHERE ID_User = ?";
            $stmt = $conn->prepare($query);
            $stmt->bind_param("si", $webPath, $userId);
            $stmt->execute();
            error_log("Foto principal actualizada en la BD");

            // Actualizar foto en la tabla Usuario
            $queryUsuario = "UPDATE Usuario SET Foto = ? WHERE ID = ?";
            $stmtUsuario = $conn->prepare($queryUsuario);
            $stmtUsuario->bind_param("si", $webPath, $userId);
            $stmtUsuario->execute();
            error_log("Foto actualizada en la tabla Usuario");
        }

        // Siempre guardar en la tabla FotosPerfil
        $query = "INSERT INTO FotosPerfil (ID_User, URL, Tipo) VALUES (?, ?, ?)";
        $stmt = $conn->prepare($query);
        $stmt->bind_param("iss", $userId, $webPath, $tipo);
        
        if ($stmt->execute()) {
            error_log("Foto guardada en la BD exitosamente");
            echo json_encode([
                'success' => true,
                'message' => 'Foto subida correctamente',
                'url' => $webPath
            ]);
        } else {
            error_log("Error al guardar en la BD: " . $stmt->error);
            // Si falla la inserción en la BD, eliminar el archivo
            unlink($targetPath);
            throw new Exception('Error al guardar la foto en la base de datos');
        }
    } else {
        error_log("Error al mover el archivo");
        throw new Exception('Error al subir el archivo');
    }

} catch (Exception $e) {
    error_log("Error en upload_photo.php: " . $e->getMessage());
    http_response_code(500);
    echo json_encode([
        'success' => false,
        'message' => $e->getMessage()
    ]);
} finally {
    if (isset($conn)) {
        $conn->close();
    }
    error_log("=== UPLOAD_PHOTO.PHP TERMINADO ===");
} 