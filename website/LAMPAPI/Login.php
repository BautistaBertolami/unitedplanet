<?php
	include_once 'Core.php';
	include_once '../php_jwt/src/JWT.php';
	include_once '../php_jwt/src/ExpiredException.php';
	include_once '../php_jwt/src/SignatureInvalidException.php';
	include_once '../php_jwt/src/BeforeValidException.php';
	use \Firebase\JWT\JWT;

	$inData = getRequestInfo();

	$id = 0;
	$firstName = "";
	$lastName = "";
	$validationStatus = 0;

	$conn = new mysqli("localhost", "1112946", "Poosproject321", "1112946");
	if ($conn->connect_error)
	{
		returnWithError( $conn->connect_error );
	}
	else
	{
		$sql = "SELECT ID,FirstName,LastName,ValidationStatus,Email FROM Users where Login='" . $inData["login"] . "' and Password='" . $inData["password"] . "'";
		$result = $conn->query($sql);
		if ($result->num_rows > 0)
		{

			$row = $result->fetch_assoc();
			$firstName = $row["FirstName"];
			$lastName = $row["LastName"];
			$validationStatus = $row["ValidationStatus"];
			$email =$row["Email"];
			$id = $row["ID"];
			// this is where we add the creation of a JWT
			$token = array(
				"iss" => $iss,
				"aud" => $aud,
				"iat" => $iat,
				"nbf" => $nbf,
				"exp" => $exp,
				"data" => array(
					"id" => $id,
					"firstname" => $firstName,
					"lastname" => $lastName,
					"email" => $email
					
				)
			 );
		  
			 // set response code
			 http_response_code(200);
		  
			 // generate jwt
			 $jwt = JWT::encode($token, $key);

			returnWithInfo($firstName, $lastName, $id, $validationStatus, $jwt );
		}
		else
		{
			returnWithError( "No Records Found" );
		}
		$conn->close();
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
		$retValue = '{"id":0,"firstName":"","lastName":"","error":"' . $err . '"}';
		sendResultInfoAsJson( $retValue );
	}

	function returnWithInfo( $firstName, $lastName, $id, $validationStatus, $jwt )
	{
		$retValue = '{"message":"Successful login.","id":' . $id . ',"firstName":"' . $firstName . '","lastName":"' . $lastName . '","validationStatus":"' . $validationStatus . '","jwt":"' . $jwt .'","error":""}';
		sendResultInfoAsJson( $retValue );
	}

?>
