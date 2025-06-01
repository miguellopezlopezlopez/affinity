<?php
session_start();
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: http://localhost');
header('Access-Control-Allow-Methods: POST');
header('Access-Control-Allow-Headers: Content-Type');
header('Access-Control-Allow-Credentials: true');

// Obtener los datos del POST
$data = json_decode(file_get_contents('php://input'), true);

// Validar que se recibieron los datos necesarios
if (!isset($data['type']) || !isset($data['email']) || !isset($data['subject']) || !isset($data['message'])) {
    echo json_encode([
        'success' => false,
        'message' => 'Datos incompletos'
    ]);
    exit;
}

// Configuración del remitente
$sender_email = "noreply@affinity.com";
$sender_name = "Affinity";

// Procesar el tipo de correo y configurar el reply-to
switch ($data['type']) {
    case 'verification_code':
        $reply_to = $sender_email;
        break;
        
    case 'signup_request':
        // Para solicitudes de registro, usar el email del administrador
        $admin_email = "admin@affinity.com";
        $data['email'] = $admin_email;
        $reply_to = isset($data['from_email']) ? $data['from_email'] : $sender_email;
        break;
        
    case 'password_reset_confirmation':
        if (isset($data['to_email']) && !isset($data['email'])) {
            $data['email'] = $data['to_email'];
        }
        $reply_to = $sender_email;
        break;
        
    default:
        $reply_to = $sender_email;
}

// Sanitizar y validar el email
$email = filter_var($data['email'], FILTER_SANITIZE_EMAIL);
if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
    echo json_encode([
        'success' => false,
        'message' => 'Formato de email inválido'
    ]);
    exit;
}

// Preparar el mensaje HTML
$message = "
<html>
<head>
    <style>
        body { 
            font-family: Arial, sans-serif; 
            line-height: 1.6; 
            color: #333;
            margin: 0;
            padding: 0;
        }
        .container { 
            max-width: 600px; 
            margin: 0 auto; 
            padding: 20px;
            background-color: #f9f9f9;
        }
        .header {
            background-color: #667eea;
            color: white;
            padding: 20px;
            text-align: center;
            border-radius: 5px 5px 0 0;
        }
        .content {
            background-color: #ffffff;
            padding: 20px;
            border-radius: 0 0 5px 5px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .footer {
            margin-top: 20px;
            text-align: center;
            font-size: 12px;
            color: #666;
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>" . htmlspecialchars($data['subject']) . "</h2>
        </div>
        <div class='content'>
            " . nl2br(htmlspecialchars($data['message'])) . "
        </div>
        <div class='footer'>
            <p>Este es un correo automático, por favor no respondas a este mensaje.</p>
            <p>Affinity - Encuentra conexiones auténticas</p>
        </div>
    </div>
</body>
</html>";

// Configurar los headers
$headers = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=UTF-8\r\n";
$headers .= "From: {$sender_name} <{$sender_email}>\r\n";
$headers .= "Reply-To: {$reply_to}\r\n";
$headers .= "X-Mailer: PHP/" . phpversion();

// Intentar enviar el correo
if(mail($email, $data['subject'], $message, $headers)) {
    echo json_encode([
        'success' => true,
        'message' => 'Correo enviado correctamente'
    ]);
} else {
    // En desarrollo, mostrar más información sobre el fallo
    echo json_encode([
        'success' => false,
        'message' => 'Error al enviar el correo',
        'debug_info' => [
            'to' => $email,
            'subject' => $data['subject'],
            'headers' => $headers
        ]
    ]);
} 