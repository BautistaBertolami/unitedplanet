<?php

  require_once __DIR__.'/vendor/autoload.php';

	$inData = getRequestInfo();
  $firstName = "";
  $lastName = "";
  $validationStatus = "";
  $err = "";

	$resetEmail = $inData["ResetEmail"];

	$conn = new mysqli("localhost", "1112946", "Poosproject321", "1112946");
  if ($conn->connect_error)
	{
		returnWithError( $conn->connect_error );
	}
  else
  {
    if($resetEmail == ""){
			$err .= "Please enter an email";
			returnWithError( $err );
		} else{
      $sql_e = "SELECT * FROM Users WHERE Email='" . $resetEmail . "'";
      $res_e = mysqli_query($conn, $sql_e);

      if (mysqli_num_rows($res_e) < 1)
      {
        //display username taken error
        $err .= "Email does not exist";
        returnWithError( $err );
      } else {

        $sql = "SELECT FirstName, LastName, Validation FROM Users where Email='" . $inData["ResetEmail"] . "'";
        $result = $conn->query($sql);
        $row = $result->fetch_assoc();
  			$firstName = $row["FirstName"];
  			$lastName = $row["LastName"];
  			$validation = $row["Validation"];

        $mail = new \PHPMailer\PHPMailer\PHPMailer(true);

        try {
          $mail->SMTPDebug = 0;
          $mail->isSMTP();
          $mail->Host = 'smtp.gmail.com';
          $mail->SMTPAuth = true;
          $mail->Username = 'poosproject@gmail.com';
          $mail->Password = 'vjpcylctkbednglp';
          $mail->SMTPSecure = 'tls';
          $mail->Port = 587;

          // Recipients
          $mail->setFrom('mailer@unitedplanet.gl', 'Reset Password on unitedplanet.gl');
          $mail->addAddress($resetEmail, 'Password Reset');
          $mail->addReplyTo('poosproject@gmail.com', $firstName);

          // Content
          $mail->Subject = 'Signup | Verification';
          $mail->Body    = 'Hello '.$firstName.' '.$lastName.' you can reset password your by clicking the link below<br><br>------------------------<br><br>Please click this link to reset your account password:<br><a href="http://www.unitedplanet.ga/resetPass.html?email='.$resetEmail.'&validation='.$validation.'">http://www.unitedplanet.ga/resetPass.html?email='.$resetEmail.'&validation='.$validation.'</a>';

          //http://localhost/project2/verify.php?email='.$email.'&validation='.$validation.'
          $mail->send();
          $conn->close();
        } catch (Exception $e) {
          $error = "Error sending email";
          returnWithError($error);
        }
      }
    }
  }

	function getRequestInfo()
	{
		return json_decode(file_get_contents('php://input'), true);
	}

  function returnWithError( $err )
	{
		$retValue = '{"error":"' . $err . '"}';
		sendResultInfoAsJson( $retValue );
	}

	function sendResultInfoAsJson( $obj )
	{
		header('Content-type: application/json');
		echo $obj;
	}
?>
