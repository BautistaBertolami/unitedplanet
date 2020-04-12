<?php

	require_once __DIR__.'/vendor/autoload.php';

	$inData = getRequestInfo();


	$firstName = $inData["FirstName"];
	$lastName = $inData["LastName"];
	$login = $inData["Login"];
	$passwrod = $inData["Password"];
	$email = $inData["Email"];
	$validation = $inData["Validation"];
	$validationStatus = $inData["ValidationStatus"];
	$err = "";

	$conn = new mysqli("localhost", "1109270", "Poosproject321", "1109270");
	if ($conn->connect_error)
	{
		returnWithError( $conn->connect_error );
	}
	else
	{
		if($email == ""){
			$err .= "Please enter an email";
			returnWithError( $err );
		} else if($login == ""){
			$err .= "Please enter a username";
			returnWithError( $err );
		}else if($passwrod == ""){
			$err .= "Please enter a password";
			returnWithError( $err );
		}else {
					$sql_u = "SELECT * FROM Users WHERE Login='" . $login . "'";
					$sql_e = "SELECT * FROM Users WHERE Email='" . $email . "'";
					$res_u = mysqli_query($conn, $sql_u);
			  	$res_e = mysqli_query($conn, $sql_e);

					if (mysqli_num_rows($res_u) > 0)
					{
						//display username taken error
						$err .= "Username already exists";
						returnWithError( $err );
					}
					else if (mysqli_num_rows($res_u) > 0)
					{
						//display email taken error
						$err .= "Email already registered";
						returnWithError( $err );
					}
					else {
						//send email verification
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
								    $mail->setFrom('mailer@unitedplanet.gl', 'Validate account on unitedplanet.gl');
								    $mail->addAddress($email, 'United Planet Verification');
								    $mail->addReplyTo('poosproject@gmail.com', $firstName);

								    // Content
								    $mail->Subject = 'Signup | Verification';
								    $mail->Body    = 'Thanks for signing up!<br>Your account has been created, you can login with the following credentials after you have activated your account by pressing the url below.<br><br>------------------------<br>Username: '.$login.'<br>Password: '.$passwrod.'<br>------------------------<br><br>Please click this link to activate your account:<br><a href="http://localhost/project2/verify.php?email='.$email.'&validation='.$validation.'">http://localhost/project2/verify.php?email='.$email.'&validation='.$validation.'</a>';

										//http://localhost/project2/verify.php?email='.$email.'&validation='.$validation.'
										$mail->send();
										$sql = "INSERT INTO Users (FirstName, LastName, Login, Password, Email, Validation, ValidationStatus) VALUES ('" . $firstName . "','" . $lastName . "','" . $login . "','" . $passwrod . "','" . $email . "','" . $validation . "','" . $validationStatus . "')";
										if( $result = $conn->query($sql) != TRUE )
										{
											//echo "this sucks";
											returnWithError( $conn->error );
										}
										$conn->close();
						} catch (Exception $e) {
							// phpmailer could not send email
							$error = "Invalid email address";
							returnWithError($error);
						}
				}
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
