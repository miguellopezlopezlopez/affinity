<?php
$host = "localhost";
$port = 3306;
$user = "root";
$password = "";
$database = "filtring";

$conn = new mysqli($host, $user, $password, $database, $port);
if ($conn->connect_error) {
    die("Conexión fallida: " . $conn->connect_error);
}
?>
