<?php
// Povezava z bazo
$host = "localhost"; // Nastavi glede na tvoj strežnik
$dbname = "gps_data"; // Ime tvoje baze
$username = "root"; // Uporabniško ime za bazo
$password = ""; // Geslo za bazo

// Povezava z MySQL bazo
$conn = new mysqli($host, $username, $password, $dbname);

// Preveri, če je povezava uspela
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Preveri, če je zahteva POST in če vsebuje JSON telo
if ($_SERVER['REQUEST_METHOD'] == 'POST') {

    // Preberi podatke iz JSON telesa
    $input = file_get_contents("php://input");

    // Dekodiraj JSON podatke v PHP objekt
    $data = json_decode($input, true);

    // Preveri, če so podatki prisotni
    if (isset($data['latitude']) && isset($data['longitude'])) {

        // Pridobi latitude, longitude in časovni žig
        $latitude = $data['latitude'];
        $longitude = $data['longitude'];
        $timestamp = date('Y-m-d H:i:s'); // Samodejno nastavi časovni žig na trenutni čas

        // Vstavi podatke v bazo
        $stmt = $conn->prepare("INSERT INTO LocationData (latitude, longitude, timestamp) VALUES (?, ?, ?)");
        $stmt->bind_param("dds", $latitude, $longitude, $timestamp);

        // Izvedi poizvedbo in preveri, če je uspela
        if ($stmt->execute()) {
            echo json_encode(["status" => "success", "message" => "Location data saved successfully"]);
        } else {
            echo json_encode(["status" => "error", "message" => "Failed to save location data"]);
        }

        $stmt->close();
    } else {
        echo json_encode(["status" => "error", "message" => "Invalid input data"]);
    }
} else {
    echo json_encode(["status" => "error", "message" => "Invalid request method"]);
}

$conn->close();
?>