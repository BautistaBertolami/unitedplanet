<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Verify Email</title>
    <link href="css/styles.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <!-- start header div -->
    <div id="header">
        <h3>Verify Email</h3>
    </div>
    <!-- end header div -->

    <!-- start wrap div -->
    <div id="wrap">
        <!-- start PHP code -->
        <?php

            $conn = new mysqli("localhost", "1109270", "Poosproject321", "1109270");
          	if ($conn->connect_error)
          	{
          		returnWithError( $conn->connect_error );
          	}
            if(isset($_GET['email']) && !empty($_GET['email']) AND isset($_GET['validation']) && !empty($_GET['validation'])){
              // Verify data
              $email = mysqli_escape_string($conn, $_GET['email']); // Set email variable
              $validation = mysqli_escape_string($conn, $_GET['validation']); // Set hash variable
              $search = mysqli_query($conn, "SELECT Email, Validation, ValidationStatus FROM Users WHERE Email='".$email."' AND Validation='".$validation."' AND ValidationStatus='0'") or die();
              $match  = mysqli_num_rows($search);
              if($match > 0){
                mysqli_query($conn, "UPDATE Users SET ValidationStatus='1' WHERE Email='".$email."' AND Validation='".$validation."' AND ValidationStatus='0'") or die();
                echo '<div class="statusmsg">Your account has been activated, you can now login</div>';
                 // We have a match, activate the account
              }else{
                  // No match -> invalid url or account has already been activated.
                  echo '<div class="statusmsg">The url is either invalid or you already have activated your account.</div>';
              }
            }else{
                // Invalid approach
                echo '<div class="statusmsg">Invalid approach, please use the link that has been send to your email.</div>';
            }
        ?>
        <!-- stop PHP Code -->
    </div>
    <!-- end wrap div -->
</body>
</html>
