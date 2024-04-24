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

// Insert coins into database 
$insertCoinsQuery = "UPDATE tblplayers SET coins = $coins WHERE Username = '$username';";
mysqli_query($con, $insertCoinsQuery) or die("2: Insert coins query failed"); // Error 2: Insert coins query failed

// Data saved successfully
echo "3 Opgeslagen";

// Close database connection
mysqli_close($con);
?>
