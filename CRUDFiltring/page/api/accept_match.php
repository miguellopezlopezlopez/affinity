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

if (!isset($_GET['match_id'])) {
    echo json_encode(['success' => false, 'error' => 'ID de match no especificado']);
    exit;
}

$matchId = intval($_GET['match_id']);

// Validar que el ID sea válido
if ($matchId <= 0) {
    echo json_encode(['success' => false, 'error' => 'ID de match inválido: ' . $matchId]);
    exit;
}

try {
    // Verificar que el match existe y no ha sido aceptado aún
    $checkQuery = "SELECT m.ID_Match, m.ID_Sol, m.ID_Acept, m.Fecha_Aceptado,
                         u1.User as Solicitante, u2.User as Receptor
                  FROM Matches m
                  JOIN Usuario u1 ON m.ID_Sol = u1.ID
                  JOIN Usuario u2 ON m.ID_Acept = u2.ID
                  WHERE m.ID_Match = ?";
    
    $stmt = $conn->prepare($checkQuery);
    $stmt->bind_param("i", $matchId);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows === 0) {
        echo json_encode([
            'success' => false, 
            'error' => 'Match no encontrado. ID: ' . $matchId
        ]);
        exit;
    }

    $row = $result->fetch_assoc();
    
    if ($row['Fecha_Aceptado'] !== null) {
        echo json_encode([
            'success' => false, 
            'error' => sprintf(
                'Match ya fue aceptado previamente. Solicitante: %s, Receptor: %s',
                $row['Solicitante'],
                $row['Receptor']
            )
        ]);
        exit;
    }

    // Actualizar el match con la fecha de aceptación
    $updateQuery = "UPDATE Matches SET Fecha_Aceptado = CURRENT_TIMESTAMP WHERE ID_Match = ?";
    $stmt = $conn->prepare($updateQuery);
    $stmt->bind_param("i", $matchId);
    
    if ($stmt->execute()) {
        echo json_encode([
            'success' => true,
            'message' => sprintf(
                'Match aceptado exitosamente entre %s y %s',
                $row['Solicitante'],
                $row['Receptor']
            ),
            'data' => [
                'match_id' => $matchId,
                'solicitante' => $row['Solicitante'],
                'receptor' => $row['Receptor']
            ]
        ]);
    } else {
        throw new Exception("Error al actualizar el match en la base de datos");
    }

} catch (Exception $e) {
    echo json_encode([
        'success' => false,
        'error' => 'Error al aceptar match: ' . $e->getMessage()
    ]);
} finally {
    if (isset($stmt)) {
        $stmt->close();
    }
    $conn->close();
}
?> 