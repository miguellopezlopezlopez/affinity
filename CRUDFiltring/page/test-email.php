<?php
$to = "tu_email@gmail.com"; // Cambia esto por tu email real
$subject = "Prueba de correo desde XAMPP";
$message = "Este es un correo de prueba enviado desde XAMPP con Mercury.";
$headers = "From: noreply@affinity.com\r\n";
$headers .= "Reply-To: noreply@affinity.com\r\n";
$headers .= "X-Mailer: PHP/" . phpversion();

if(mail($to, $subject, $message, $headers)) {
    echo "Correo enviado correctamente";
} else {
    echo "Error al enviar el correo";
}
?> 