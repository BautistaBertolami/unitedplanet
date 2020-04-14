<?php
	$inData = getRequestInfo();

	$contact = $inData["contact"];
	$userId = $inData["userId"];
	$phoneNumber = $inData["phoneNumber"];
	$email = $inData["email"];
	//$country = $inData["country"];
	//Nate 4/13 changed field country into fields address and coordinates
	$address = $inData["address"];
	$coordinates = $inData["coordinates"];

	$conn = new mysqli("localhost", "1112946", "Poosproject321", "1112946");
	if ($conn->connect_error)
	{
		returnWithError( $conn->connect_error );
	}
	else
	{
		// update database for two new columns
		$sql = "INSERT into Contacts (UserID,Name,PhoneNumber,Email,Address,Coordinates) VALUES ('" . $userId . "','" . $contact . "','" . $phoneNumber . "','" . $email . "','" . $address . "','" . $coordinates . "')";
		if( $result = $conn->query($sql) != TRUE )
		{
			returnWithError( $conn->error );
		}
		$conn->close();
	}

	returnWithError("");

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
 