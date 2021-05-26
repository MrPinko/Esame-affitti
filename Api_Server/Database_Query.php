<?php

class Database_Query
{

    private $db = null;

    public function __construct($db)
    {
        $this->db = $db;
    }

    public function find($hashedName, $hashedPw)
    {
        $statement = "
            SELECT
                *
            FROM
                utente
            WHERE email = ?
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
                    att_turistiche.descrizione,
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
            proprietario.iban,
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

    public function getUsername($email)
    {
        $statement = "SELECT username FROM utente WHERE email = ?";
        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array($email));
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            return $result;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function getCF($username)
    {
        $statement = "SELECT cf_utente FROM utente WHERE username = ?";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array($username));
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
                    utente_appartamenti.idUtente_Appartamenti,
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

    public function getOrdiniEffettuati($CF)
    {
        $statement = "SELECT
                    utente.username,
                    utente_appartamenti.dataInizio,
                    utente_appartamenti.dataFine,
                    utente_appartamenti.timestamp,
                    appartamenti.nome AS nomeAppartamento
                        FROM utente_appartamenti
                    INNER JOIN appartamenti
                        ON utente_appartamenti.fk_appartamenti = appartamenti.idappartamenti
                    INNER JOIN utente
                        ON utente_appartamenti.fk_utente = utente.cf_utente
                    WHERE utente_appartamenti.fk_utente = ?";
        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array($CF));
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

    public function BindingUserToDate(array $input)
    {
        /*
        {"fk_utente" : "RSOFRC21E23C623Y", "idUtente_Appartamenti" : 0}
        TUTTO SU UNA RIGA MANNAGGIA AI JSON
        */
        $statement = "UPDATE utente_appartamenti SET fk_utente = :fk_utente WHERE idUtente_Appartamenti = :idUtente_Appartamenti ";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                'fk_utente' => $input['fk_utente'],
                'idUtente_Appartamenti' => $input['idUtente_Appartamenti'],
            ));
            return $statement->rowCount();
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function DeleteBindingUserToDate(array $input)
    {
        /*
        {"idUtente_Appartamenti" : 0}
        TUTTO SU UNA RIGA MANNAGGIA AI JSON
        */
        $statement = "UPDATE utente_appartamenti SET fk_utente = null, timestamp='0000-00-00 00:00:00' WHERE idUtente_Appartamenti= :idUtente_Appartamenti";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                'idUtente_Appartamenti' => $input['idUtente_Appartamenti'],
            ));
            return $statement->rowCount();
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }
    }

    public function CreateReview(array $input)
    {
        /*
        {"idrecensioni" : 0, "posizione" : 0, "qualita_prezzo" : 0, "servizio" : 0, "timestamp" : 2021-12-12, "idappartamenti_recensioni" : 0, "fk_recensioni" : 0, "fk_appartamenti" : 0 }
        idrecensinoi e idappartamenti_recensioni possiedono lo stesso numero 
        */

        $statement = "SELECT idappartamenti_recensioni FROM appartamenti_recensioni order by idappartamenti_recensioni desc limit 1";

        try {
            $statement = $this->db->query($statement);
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);
            $lastID = $result[0]['idappartamenti_recensioni'] + 1;
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }

        $statementRecensioni = "INSERT INTO recensioni
            (idrecensioni, posizione, qualita_prezzo, servizio, timestamp)
            VALUES
            (:idrecensioni, :posizione, :qualita_prezzo, :servizio, :timestamp)";

        try {
            $statementRecensioni = $this->db->prepare($statementRecensioni);
            $statementRecensioni->execute(array(
                'idrecensioni' => $lastID,
                'posizione' => $input['posizione'],
                'qualita_prezzo' => $input['qualita_prezzo'],
                'servizio' => $input['servizio'],
                'timestamp' => $input['timestamp'],
            ));
        } catch (\PDOException$e) {
            exit($e->getMessage());
        }

        $statementJoin = "INSERT INTO appartamenti_recensioni
            (idappartamenti_recensioni, fk_recensioni, fk_appartamenti) VALUES
            (:idappartamenti_recensioni, :fk_recensioni, :fk_appartamenti)";
        try {
            $statementJoin = $this->db->prepare($statementJoin);
            $statementJoin->execute(array(
                'idappartamenti_recensioni' => $lastID,
                'fk_recensioni' => $lastID,
                'fk_appartamenti' => $input['fk_appartamenti'],
            ));
            return $statementJoin->rowCount() . "   " . $statementRecensioni->rowCount();
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
