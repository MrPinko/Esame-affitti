<?php

class Database_Query
{

    private $db = null;

    public function __construct($db)
    {
        $this->db = $db;
    }

    public function getAll()
    {
        $statement = "
            SELECT
                id, testcol
            FROM
                test;
        ";

        try {
            $statement = $this->db->query($statement);
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            return $result;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function find($id)
    {
        $statement = "
            SELECT
                id, testcol
            FROM
                test
            WHERE id = ?;
        ";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array($id));
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            return $result;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function insert(array $input)
    {
        $statement = "
            INSERT INTO test
                (id, testcol)
            VALUES
                (:id, :testcol);
        ";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                'id' => $input['id'],
                'testcol' => $input['testcol'],
            ));
            return $statement->rowCount();
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function update($id, array $input)
    {
        $statement = "
            UPDATE test
            SET
                testcol  = :testcol
            WHERE id = :id;
        ";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                'id' => (int) $id,
                'testcol' => $input['testcol'],
            ));
            return $statement->rowCount();
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function delete($id)
    {
        $statement = "
            DELETE FROM test
            WHERE id = :id;
        ";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array('id' => $id));
            return $statement->rowCount();
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

}
