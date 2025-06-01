<?php

function checkSession() {
    session_start();
    
    if (!isset($_SESSION['userId']) || empty($_SESSION['userId'])) {
        http_response_code(401);
        echo json_encode([
            'success' => false,
            'message' => 'No autenticado',
            'authenticated' => false
        ]);
        exit();
    }
    
    return $_SESSION['userId'];
}

function isAuthenticated() {
    session_start();
    return isset($_SESSION['userId']) && !empty($_SESSION['userId']);
}

function getCurrentUserId() {
    session_start();
    return isset($_SESSION['userId']) ? $_SESSION['userId'] : null;
} 