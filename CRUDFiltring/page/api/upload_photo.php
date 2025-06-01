<?php
header('Access-Control-Allow-Origin: *');
header('Content-Type: application/json; charset=UTF-8');
header('Access-Control-Allow-Methods: POST, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    http_response_code(405);
    echo json_encode(['success' => false, 'message' => 'Método no permitido']);
    exit();
}

if (!isset($_POST['userId']) || !isset($_FILES['foto']) || !isset($_POST['tipo'])) {
    http_response_code(400);
    echo json_encode(['success' => false, 'message' => 'Faltan datos requeridos']);
    exit();
}

try {
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if ($conn->connect_error) {
        throw new Exception('Error de conexión: ' . $conn->connect_error);
    }

    $userId = $conn->real_escape_string($_POST['userId']);
    $tipo = $conn->real_escape_string($_POST['tipo']);

    // Verificar límite de fotos para galería
    if ($tipo === 'galeria') {
        $queryCount = "SELECT COUNT(*) as total FROM FotosPerfil WHERE ID_User = ? AND Tipo = 'galeria'";
        $stmtCount = $conn->prepare($queryCount);
        $stmtCount->bind_param("i", $userId);
        $stmtCount->execute();
        $resultCount = $stmtCount->get_result();
        $count = $resultCount->fetch_assoc()['total'];

        if ($count >= 5) {
            throw new Exception('Has alcanzado el límite máximo de 5 fotos en la galería');
        }
    }

    // Procesar la foto
    $file = $_FILES['foto'];
    $fileName = $file['name'];
    $fileType = strtolower(pathinfo($fileName, PATHINFO_EXTENSION));
    $allowedTypes = ['jpg', 'jpeg', 'png', 'gif'];

    if (!in_array($fileType, $allowedTypes)) {
        throw new Exception('Tipo de archivo no permitido. Solo se permiten: ' . implode(', ', $allowedTypes));
    }

    // Crear directorio si no existe
    $uploadDir = '../uploads/';
    if (!file_exists($uploadDir)) {
        mkdir($uploadDir, 0777, true);
    }

    // Generar nombre único para el archivo
    $newFileName = uniqid() . '.' . $fileType;
    $targetPath = $uploadDir . $newFileName;
    $webPath = 'uploads/' . $newFileName;

    if (move_uploaded_file($file['tmp_name'], $targetPath)) {
        if ($tipo === 'principal') {
            // Actualizar foto principal en la tabla Perfil
            $query = "UPDATE Perfil SET FotoPrincipal = ? WHERE ID_User = ?";
            $stmt = $conn->prepare($query);
            $stmt->bind_param("si", $webPath, $userId);
            $stmt->execute();
        } else {
            // Insertar en la tabla FotosPerfil
            $query = "INSERT INTO FotosPerfil (ID_User, URL, Tipo) VALUES (?, ?, ?)";
            $stmt = $conn->prepare($query);
            $stmt->bind_param("iss", $userId, $webPath, $tipo);
            $stmt->execute();
        }

        echo json_encode([
            'success' => true,
            'message' => 'Foto subida correctamente',
            'url' => $webPath
        ]);
    } else {
        throw new Exception('Error al subir el archivo');
    }

} catch (Exception $e) {
    http_response_code(500);
    echo json_encode([
        'success' => false,
        'message' => $e->getMessage()
    ]);
} finally {
    if (isset($conn)) {
        $conn->close();
    }
} 