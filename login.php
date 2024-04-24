<?php
    
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

//kijken of de naam bestaat
$namecheckquery = "SELECT username, salt, hashh, adminn, coins, Damage, Speed, Health FROM tblplayers WHERE Username ='". $username ."';";
$namecheck = mysqli_query($con, $namecheckquery) or die("2"); // Error 2: gebruikersnaam check mislukt

if(mysqli_num_rows($namecheck)!= 1)
{
    echo "5: Either no user with name, or more than one"; //error code #5 - hoeveelheid namen matched != 1
    exit();
}
//verkrijg login info van query
$existinginfo = mysqli_fetch_assoc($namecheck);
$salt = $existinginfo["salt"];
$hash = $existinginfo["hashh"];



$loginhash = crypt($password, $salt);
if ($hash != $loginhash)
{
    echo "6: Incorrect password"; //error code #6 - passwoord klopt niet met de hash in de database
    exit();
}

echo "01\t" . $existinginfo["adminn"] . "\t" . $existinginfo["coins"] . "\t" . $existinginfo["Damage"]. "\t" . $existinginfo["Speed"]. "\t" . $existinginfo["Health"] ;
    

    

?>