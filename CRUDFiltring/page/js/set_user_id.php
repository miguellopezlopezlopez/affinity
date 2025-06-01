<?php
header('Content-Type: text/html');
$userId = isset($_GET['userId']) ? intval($_GET['userId']) : 0;
?>
<!DOCTYPE html>
<html>
<head>
    <title>Setting User ID</title>
</head>
<body>
    <script>
        localStorage.setItem('userId', '<?php echo $userId; ?>');
        window.close();
    </script>
</body>
</html> 