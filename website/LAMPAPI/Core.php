<?php
// show error reporting
error_reporting(E_ALL);
 
// set your default time-zone
date_default_timezone_set('America/New_York');
 
// variables used for jwt
$key = "This_is_a_random_example_key_that_must_be_changed_immediately";
$iss = "http://unitedplanet.ga";
$aud = "http://unitedplanet.ga";
$iat = time();
$nbf = time();
$exp = time() + 3600;
?>