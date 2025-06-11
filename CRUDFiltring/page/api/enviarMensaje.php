<?php
include "conexion.php";

$idMatch = $_POST['id_match'];
$contenido = $_POST['contenido'];
$idEmisor = $_POST['id_emisor'];
$idReceptor = $_POST['id_receptor'];

$stmt = $conn->prepare("INSERT INTO mensaje (ID_Match, Contenido, ID_Emisor, ID_Receptor) VALUES (?, ?, ?, ?)");
$stmt->bind_param("isii", $idMatch, $contenido, $idEmisor, $idReceptor);

if ($stmt->execute()) {
    echo json_encode(["status" => "ok"]);
} else {
    echo json_encode(["status" => "error", "error" => $stmt->error]);
}
?>
