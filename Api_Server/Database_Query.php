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

        /*
        {
            "username" : "federico",
            "pw" : "passwordcriptata",
            "email": "federico@gmail.com",
            "cell" : "3347235549",
            "citta" : "verceia",
            "via" : "nazionale",
            "numero": 14,
            "cap", 23020,
            "dataN": 2002-01-18,
            "sesso": "M",
            "cf_utente" : "frosa1802cfy",
            "nome" : "federico",
            "cognome": "rosa",
            "m_pagamento": "visa",
        }
        */

        $statement = "
            INSERT INTO utente
                (username, pw, email, cell, citta, via, numero, cap, dataN, sesso, cf_utente, nome, cognome, m_pagamento)
            VALUES
                (:username, :pw, :email, :cell, :citta, :via, :numero, :cap, :dataN, :sesso, :cf_utente, :nome, :cognome, :m_pagamento)
        ";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                'username' => $input['username'],
                'pw' => $input['pw'],
                'email' => $input['email'],
                'cell' => $input['cell'],
                'citta' => $input['citta'],
                'via' => $input['via'],
                'numero' => $input['numero'],
                'cap' => $input['cap'],
                'dataN' => $input['dataN'],
                'sesso' => $input['sesso'],
                'cf_utente' => $input['cf_utente'],
                'nome' => $input['nome'],
                'cognome' => $input['cognome'],
                'm_pagamento' => $input['m_pagamento'],



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
