<?php

	$inData = getRequestInfo();

	$searchResults = "";
	$searchCount = 0;

	$conn = new mysqli("localhost", "1112946", "Poosproject321", "1112946");
	if ($conn->connect_error)
	{
		returnWithError( $conn->connect_error );
	}
	else
	{
		// added two new columns
                $sql = "update Contacts Set Name ='" . $inData["contact"] . "',  PhoneNumber ='" . $inData["phoneNumber"] . "', Email ='" . $inData["email"] . "', Address ='" . $inData["address"] . "', Coordinates ='" . $inData["coordinates"] . "' where ID = '" . $inData["userId"] . "'";
		$conn->query($sql);
		$sql = "select * from Contacts where ID = '" . $inData["userId"] . "'";
		$result = $conn->query($sql);
			while($row = $result->fetch_assoc())
			{
				if( $searchCount > 0 )
				{
					$searchResults .= ",";
				}
				$searchCount++;
				// added two new columns
				$searchResults .= '"' . $row["Name"] . ' | ' . $row["PhoneNumber"] . ' | ' . $row["Email"] . ' | ' . $row["Address"] . ' | ' . $row["Coordinates"] . " | " . $row["ID"] . '"';
			}
		$conn->close();
	}

	returnWithInfo( $searchResults );

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
		$retValue = '{"results":["what the heck"],"error":"' . $err . '"}';
		sendResultInfoAsJson( $retValue );
	}

	function returnWithInfo( $searchResults )
	{
		$retValue = '{"results":[' . $searchResults . '],"error":""}';
		sendResultInfoAsJson( $retValue );
	}

?>
