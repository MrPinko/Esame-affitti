<?php

class DatabaseConnector
{

    private $dbConnection = null;

    public function __construct()
    {

        echo getenv('TEST');

        $dns = 'mysql:host=localhost;dbname=esame';
        $user = 'root';
        $pass = 'Rosa';

        try {
            $this->dbConnection = new PDO($dns, $user, $pass);
            $this->dbConnection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        } catch (PDOException $e) {
            exit($e->getMessage());
        }
    }

    public function getConnection()
    {
        return $this->dbConnection;
    }

}