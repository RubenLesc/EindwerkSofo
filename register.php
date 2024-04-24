<?php
// Connect to the database
// verbinden met de database
$con = mysqli_connect('localhost', 'root', '', 'eindwerksofo');

// Check connection
if (mysqli_connect_errno()) {
    echo "1: Connection failed"; // Error 1: verbinding met de database mislukt
    exit();
}

// verkrijgen van username en wachtwoord via post --> url
$username = $_POST["name"];
$password = $_POST["password"];

$namecheckquery = "SELECT username FROM tblplayers WHERE Username ='". $username ."';";
$namecheck = mysqli_query($con, $namecheckquery) or die("2"); // Error 2: gebruikersnaam check mislukt

// Check if the username already exists
if (mysqli_num_rows($namecheck) > 0) {
    echo "3: Name already exists"; // Error 3: Name already exists
    exit();
}

// Insert user into database 
$salt =  "\$5\$rounds=5000\$". "eindwerksofo". $username . "\$";
$hash = crypt($password, $salt);

$insertuserquery = "INSERT INTO tblplayers (username, hashh, salt) VALUES ('". $username ."','" . $hash . "','" . $salt . "');";
mysqli_query($con, $insertuserquery) or die(" 4: Insert player query failed"); // Error 4: Insert player query failed

// User registration successful
echo "0";

// Close database connection
mysqli_close($con);
?>