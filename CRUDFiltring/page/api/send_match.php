<?php
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: POST');
header('Access-Control-Allow-Headers: Content-Type');

require_once 'config.php';

if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    echo json_encode(['success' => false, 'error' => 'Método no permitido']);
    exit;
}

if (!isset($_GET['id_solicitante']) || !isset($_GET['id_receptor'])) {
    echo json_encode(['success' => false, 'error' => 'Datos incompletos']);
    exit;
}

$idSolicitante = intval($_GET['id_solicitante']);
$idReceptor = intval($_GET['id_receptor']);

try {
    // Verificar que los usuarios existen
    $checkQuery = "SELECT ID FROM Usuario WHERE ID IN (?, ?)";
    $stmt = $conn->prepare($checkQuery);
    $stmt->bind_param("ii", $idSolicitante, $idReceptor);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows !== 2) {
        echo json_encode(['success' => false, 'error' => 'Uno o ambos usuarios no existen']);
        exit;
    }

    // Verificar que no existe un match pendiente o aceptado entre estos usuarios
    $matchCheckQuery = "
        SELECT ID_Match 
        FROM Matches 
        WHERE (ID_Sol = ? AND ID_Acept = ?) 
           OR (ID_Sol = ? AND ID_Acept = ?)
    ";
    $stmt = $conn->prepare($matchCheckQuery);
    $stmt->bind_param("iiii", $idSolicitante, $idReceptor, $idReceptor, $idSolicitante);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows > 0) {
        echo json_encode(['success' => false, 'error' => 'Ya existe un match entre estos usuarios']);
        exit;
    }

    // Insertar el nuevo match
    $insertQuery = "
        INSERT INTO Matches (ID_Sol, ID_Acept, Fecha_Solicitud) 
        VALUES (?, ?, CURRENT_TIMESTAMP)
    ";
    $stmt = $conn->prepare($insertQuery);
    $stmt->bind_param("ii", $idSolicitante, $idReceptor);
    
    if ($stmt->execute()) {
        echo json_encode([
            'success' => true,
            'message' => 'Match enviado exitosamente',
            'matchId' => $stmt->insert_id
        ]);
    } else {
        throw new Exception("Error al insertar el match");
    }

} catch (Exception $e) {
    echo json_encode([
        'success' => false,
        'error' => 'Error al enviar match: ' . $e->getMessage()
    ]);
} finally {
    if (isset($stmt)) {
        $stmt->close();
    }
    $conn->close();
}
?>