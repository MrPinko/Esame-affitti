<?php

include "DatabaseConnector.php";
include "Controller.php";

$dbConnection = (new DatabaseConnector())->getConnection();

$uri = parse_url($_SERVER['REQUEST_URI'], PHP_URL_PATH);
$uri = explode('/', $uri);

$userId = null;
if (isset($uri[3])) {
    if($uri[3] == "user" ){
        $firstPar = $uri[4];          //può essere registerUser, loginUser
    }
    
}

print_r($uri);

$requestMethod = $_SERVER['REQUEST_METHOD'];
$controller = new Controller($dbConnection, $requestMethod, $firstPar);
$controller->processRequest();