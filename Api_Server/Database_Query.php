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

    public function find($hashedName, $hashedPw)
    {
        $statement = "
            SELECT
                *
            FROM
                utente
            WHERE nome = ?
            AND pw = ?;
        ";
        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array($hashedName, $hashedPw));
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            return $result;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }

    }

    public function getAttrazioniTuristiche()
    {
        $statement = "SELECT
                    att_turistiche.nome,
                    att_turistiche.lat,
                    att_turistiche.`long`,
                    immagini_per_attrazionituristiche.url
                    FROM att_turistiche
                    INNER JOIN immagini_per_attrazionituristiche
                        ON att_turistiche.fk_immagini = immagini_per_attrazionituristiche.idimmagini_per_attrazioniTuristiche
                    ";

        try {
            $statement = $this->db->query($statement);
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            return $result;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function getAppartamenti()
    {
        $statement = "SELECT
            appartamenti.nome as nomeAppartamento,
            appartamenti.piano,
            appartamenti.superficie,
            appartamenti.costo,

            immobile_privato.lat,
            immobile_privato.`long`,
            proprietario.nome AS nomeProprietario,
            proprietario.cognome AS cognomeProprietario,
            social.provider,
            social.nome
            FROM appartamenti
            LEFT JOIN appartamenti_recensioni
                ON appartamenti_recensioni.fk_appartamenti = appartamenti.idappartamenti
            INNER JOIN immobile_privato
                ON appartamenti.fk_immobilePrivato = immobile_privato.idimmobile
            INNER JOIN proprietario
                ON proprietario.fk_immobilePrivato = immobile_privato.idimmobile
            INNER JOIN proprietario_social
                ON proprietario_social.fk_proprietario = proprietario.cf_proprietario
            INNER JOIN social
                ON proprietario_social.fk_social = social.idsocial
            LEFT JOIN recensioni
                ON appartamenti_recensioni.fk_recensioni = recensioni.idrecensioni

        ";

        try {
            $statement = $this->db->query($statement);
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            return $result;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function getNomeEcognome($email)
    {
        $statement = "SELECT nome, cognome FROM utente WHERE email = ?";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array($email));
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            return $result;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }

    }

    public function getReview()
    {
        $statement = "SELECT
            appartamenti.nome,
            AVG(recensioni.posizione) AS avg_posizione,
            AVG(recensioni.qualita_prezzo) AS avg_qualita_prezzo,
            AVG(recensioni.servizio) AS avg_servizio
            FROM appartamenti
            INNER JOIN appartamenti_recensioni
            ON appartamenti_recensioni.fk_appartamenti = appartamenti.idappartamenti
            INNER JOIN recensioni
            ON appartamenti_recensioni.fk_recensioni = recensioni.idrecensioni
            GROUP BY appartamenti.nome";

        try {
            $statement = $this->db->query($statement);
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            return $result;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function getAppartamentiImmagini()
    {
        $statement = "SELECT
                    appartamenti.nome,
                    immagini_per_appartamenti.url
                    FROM immagini_per_appartamenti
                    INNER JOIN appartamenti
                    ON immagini_per_appartamenti.fkAppartamento = appartamenti.idappartamenti";

        try {
            $statement = $this->db->query($statement);
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            return $result;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function getDateDisponibili()
    {
        $statement = "SELECT
                    utente_appartamenti.idUtente_Apaprtamenti,
                    appartamenti.nome,
                    utente_appartamenti.dataInizio,
                    utente_appartamenti.dataFine
                    FROM utente_appartamenti
                    INNER JOIN appartamenti
                        ON utente_appartamenti.fk_appartamenti = appartamenti.idappartamenti
                        WHERE fk_utente IS NULL";

        try {
            $statement = $this->db->query($statement);
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            return $result;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function insert(array $input)
    {

        /*
        {"username" : "federico", "pw" : "passwordcriptata", "email": "federico@gmail.com", "cell" : "3347235549", "citta" : "verceia", "via" : "nazionale", "numero": "14", "cap" : "23020", "dataN": "2002-01-18", "sesso": "M", "cf_utente" : "frosa1802cfy", "nome" : "federico", "cognome": "rosa", "m_pagamento": "visa"}
        TUTTO SU UNA RIGA MANNAGGIA AI JSON
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
