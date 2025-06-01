<?php
session_start();
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: http://localhost');
header('Access-Control-Allow-Methods: POST');
header('Access-Control-Allow-Headers: Content-Type');
header('Access-Control-Allow-Credentials: true');

// Obtener los datos del POST
$data = json_decode(file_get_contents('php://input'), true);

if (!isset($data['email'])) {
    echo json_encode([
        'success' => false,
        'message' => 'Por favor, proporciona un email'
    ]);
    exit;
}

$email = filter_var($data['email'], FILTER_SANITIZE_EMAIL);

if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
    echo json_encode([
        'success' => false,
        'message' => 'Formato de email inválido'
    ]);
    exit;
}

try {
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if($conn->connect_error) {
        throw new Exception('Error de conexión: ' . $conn->connect_error);
    }

    // Verificar si el email existe
    $stmt = $conn->prepare('SELECT ID, Nombre FROM Usuario WHERE Email = ?');
    $stmt->bind_param('s', $email);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows === 0) {
        echo json_encode([
            'success' => false,
            'message' => 'No existe una cuenta con este email'
        ]);
        exit;
    }

    $usuario = $result->fetch_assoc();
    
    // Generar código de verificación
    $verificationCode = sprintf('%06d', mt_rand(0, 999999));
    $expiryTime = time() + (30 * 60); // 30 minutos

    // Limpiar códigos anteriores si existen
    if (isset($_SESSION['verification_code'])) {
        unset($_SESSION['verification_code']);
    }

    // Guardar en sesión
    $_SESSION['verification_code'] = [
        'email' => $email,
        'code' => $verificationCode,
        'expires' => $expiryTime,
        'attempts' => 0
    ];

    // Preparar el correo
    $to = $email;
    $subject = "Código de verificación - Affinity";
    $nombre = $usuario['Nombre'];
    
    $message = "
    <html>
    <head>
        <style>
            body { font-family: Arial, sans-serif; line-height: 1.6; }
            .container { max-width: 600px; margin: 0 auto; padding: 20px; }
            .code { font-size: 32px; font-weight: bold; color: #667eea; letter-spacing: 5px; text-align: center; margin: 20px 0; }
            .footer { font-size: 12px; color: #666; margin-top: 30px; }
        </style>
    </head>
    <body>
        <div class='container'>
            <h2>Hola $nombre,</h2>
            <p>Has solicitado restablecer tu contraseña en Affinity. Usa el siguiente código de verificación:</p>
            <div class='code'>$verificationCode</div>
            <p>Este código expirará en 30 minutos por razones de seguridad.</p>
            <p>Si no solicitaste este código, puedes ignorar este correo.</p>
            <div class='footer'>
                <p>Este es un correo automático, por favor no respondas a este mensaje.</p>
                <p>Affinity - Encuentra conexiones auténticas</p>
            </div>
        </div>
    </body>
    </html>";

    // Headers para correo HTML
    $headers = "MIME-Version: 1.0\r\n";
    $headers .= "Content-type: text/html; charset=UTF-8\r\n";
    $headers .= "From: Affinity <noreply@affinity.com>\r\n";
    $headers .= "Reply-To: noreply@affinity.com\r\n";
    $headers .= "X-Mailer: PHP/" . phpversion();

    // Intentar enviar el correo
    if(mail($to, $subject, $message, $headers)) {
        echo json_encode([
            'success' => true,
            'message' => 'Se ha enviado un código de verificación a tu email',
            'debug_code' => $verificationCode // Solo para desarrollo
        ]);
    } else {
        $error = error_get_last();
        echo json_encode([
            'success' => false,
            'message' => 'Error al enviar el correo. Por favor, inténtalo más tarde.',
            'debug_code' => $verificationCode, // Solo para desarrollo
            'error_details' => $error ? $error['message'] : 'No se pudo enviar el correo'
        ]);
    }

} catch (Exception $e) {
    echo json_encode([
        'success' => false,
        'message' => 'Error en el servidor: ' . $e->getMessage()
    ]);
} finally {
    if (isset($conn) && $conn->ping()) {
        $conn->close();
    }
} 