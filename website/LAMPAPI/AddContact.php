<?php
	$inData = getRequestInfo();

	$contact = $inData["contact"];
	$userId = $inData["userId"];
	$phoneNumber = $inData["phoneNumber"];
	$email = $inData["email"];
	$country = $inData["country"];
	//change country from string to integer with lookup table

	$conn = new mysqli("localhost", "1109270", "Poosproject321", "1109270");
	if ($conn->connect_error)
	{
		returnWithError( $conn->connect_error );
	}
	else
	{
		// change country to actual location integers
		$sql = "INSERT into Contacts (UserId,Name,PhoneNumber,Email,Country) VALUES ('" . $userId . "','" . $contact . "','" . $phoneNumber . "','" . $email . "','" . $country . "')";
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
