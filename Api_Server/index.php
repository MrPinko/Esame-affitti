<?php

include "DatabaseConnector.php";
include "Controller.php";

$dbConnection = (new DatabaseConnector())->getConnection();

$uri = parse_url($_SERVER['REQUEST_URI'], PHP_URL_PATH);
$uri = explode('/', $uri);

$userId = null;
if (isset($uri[3])) {
    $userId = (int) $uri[3];
}
$requestMethod = $_SERVER['REQUEST_METHOD'];

$controller = new Controller($dbConnection, $requestMethod, $userId);
$controller->processRequest();
