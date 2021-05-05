<?php

include "DatabaseConnector.php";

$dbConnection = (new DatabaseConnector())->getConnection();

$data = json_decode(file_get_contents("php://input"), true);

print_r($data);

insert($data, $dbConnection);

function insert(array $input, $dbConnection)
{
    $statement = "
            INSERT INTO test
                (id, testcol)
            VALUES
                (:id, :testcol);
        ";

    try {
        $statement = $dbConnection->prepare($statement);
        $statement->execute(array(
            'id' => $input['id'],
            'testcol' => $input['testcol'],
        ));
        return $statement->rowCount();
    } catch (\PDOException$e) {
        exit($e->getMessage());
    }
}
