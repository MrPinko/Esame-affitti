<?php
header("Content-Type:application/json");
require "data.php";
require "Users.php";
require "DatabaseConnector.php";

$uri = parse_url($_SERVER['REQUEST_URI'], PHP_URL_PATH);
$uri = explode('/', $uri);

// all of our endpoints start with /person
// everything else results in a 404 Not Found
if ($uri[2] !== 'index.php') {
    header("HTTP/1.1 404 Not Found");
    exit();
}

$requestMethod = $_SERVER["REQUEST_METHOD"];

if (isset($uri[3])) {
    $params = $uri[3];
} else {
    $params = null;
}

$db = (new DatabaseConnector())->getConnection();

$controller = new Users($db, $requestMethod, $params);
$controller->processRequest();