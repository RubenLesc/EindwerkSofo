<?php
// Connect to the database
$con = mysqli_connect('localhost', 'root', '', 'eindwerksofo');

// Check connection
if (mysqli_connect_errno()) {
    echo "1: Connection failed"; // Error 1: Connection failed
    exit();
}

// Retrieve coins from Unity
$coins = $_POST["coins"];
$username = $_POST["username"];
$damage = $_POST["damage"];
$health = $_POST["health"];
$speed = $_POST["speed"];

// Insert coins into database 
$insertCoinsQuery = "UPDATE tblplayers SET coins = $coins , Damage = $damage , Health = $health , Speed = $speed WHERE Username = '$username';";
mysqli_query($con, $insertCoinsQuery) or die("2: Insert coins query failed"); // Error 2: Insert coins query failed

// Data saved successfully
echo "3 Opgeslagen";

// Close database connection
mysqli_close($con);
?>
