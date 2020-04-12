<?php

    $inData = getRequestInfo();
    $email = $inData["email"];
    $validation = $inData["validation"];
    $password = $inData["password"];

    $conn = new mysqli("localhost", "1109270", "Poosproject321", "1109270");
    if ($conn->connect_error)
    {
      returnWithError( $conn->connect_error );
    }

    if($password == ""){
      $error = "Enter new password";
      returnWithError( $error );
      //please enter password
    } else {
      $search = mysqli_query($conn, "SELECT Email, Validation FROM Users WHERE Email='".$email."' AND Validation='".$validation."'") or die();
      $match  = mysqli_num_rows($search);
      if($match > 0){
        mysqli_query($conn, "UPDATE Users SET Password='".$password."' WHERE Email='".$email."' AND Validation='".$validation."'") or die();
        $conn->close();
        // success
      } else {
        $error = "Account does not exist";
        returnWithError( $error );
      }
    }

    function getRequestInfo()
  	{
  		return json_decode(file_get_contents('php://input'), true);
  	}

  	function sendResultInfoAsJson( $obj )
  	{
  		header('Content-type: application/json');
  		echo $obj;
  	}

  	function returnWithError( $err )
  	{
  		$retValue = '{"error":"' . $err . '"}';
  		sendResultInfoAsJson( $retValue );
  	}
?>
