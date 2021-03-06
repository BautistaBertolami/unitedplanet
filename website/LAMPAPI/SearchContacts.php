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
		$sql = "select * from Contacts where Name like '%" . $inData["search"] . "%' and UserID=" . $inData["userId"];
		$result = $conn->query($sql);
		if ($result->num_rows > 0)
		{
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
				returnWithInfo( $searchResults );
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
		$retValue = '{"results":[""],"error":"' . $err . '"}';
		sendResultInfoAsJson( $retValue );
	}

	function returnWithInfo( $searchResults )
	{
		$retValue = '{"results":[' . $searchResults . '],"error":""}';
		sendResultInfoAsJson( $retValue );
	}

?>
