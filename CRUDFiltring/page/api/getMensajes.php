<?php
require_once "conexion.php";
require_once "utils/session.php";

$userId = checkSession();

$idMatch = $_GET['id_match'] ?? null;
$lastId = $_GET['last_id'] ?? 0;

if (!$idMatch) {
    http_response_code(400);
    echo json_encode(["status" => "error", "message" => "Falta id_match"]);
    exit();
}

$stmt = $conn->prepare("SELECT * FROM mensaje WHERE ID_Match = ? AND ID_Mensaje > ? ORDER BY Fecha_Hora ASC");
$stmt->bind_param("ii", $idMatch, $lastId);
$stmt->execute();

$result = $stmt->get_result();
$mensajes = [];

while ($row = $result->fetch_assoc()) {
    $mensajes[] = $row;
}

echo json_encode($mensajes);
?>
