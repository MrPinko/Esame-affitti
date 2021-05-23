<?php

include "Database_Query.php";

class Controller
{
    private $db;
    private $requestMethod;
    private $queryMenu; //può essere RegisterUser, loginUser
    private $DBquery;

    private $hashedName;
    private $hashedPw;
    private $email;

    public function __construct($db, $requestMethod, $queryMenu)
    {
        $this->db = $db;
        $this->requestMethod = $requestMethod;
        $this->queryMenu = $queryMenu;

        $this->DBquery = new Database_Query($db);

        if ($queryMenu == 'loginUser') {
            $uri = parse_url($_SERVER['REQUEST_URI'], PHP_URL_PATH);
            $uri = explode('/', $uri);
            if(isset($uri[6])){
                $this->hashedName = $uri[5]; //contiene il nome
                $this->hashedPw = $uri[6]; //contiene la password
            }else{
                $this->email = $uri[5];
            }
        }
    }

    public function processRequest()
    {
        switch ($this->requestMethod) {
            case 'GET' && $this->queryMenu == "loginUser" && $this->hashedPw != null:
                $response = $this->checkLoginUser($this->hashedName, $this->hashedPw);
                /*if ($this->userId) { //se esiste un id
                    $response = $this->getUser($this->userId);
                } else {
                    $response = $this->getAllUsers();
                };*/
                break;
            case 'GET' && $this->queryMenu == 'attrazioniTuristiche':
                $response = $this->getAttrazioniTuristiche();
                break;
            case 'GET' && $this->queryMenu == 'appartamenti':
                $response = $this->getAppartamenti();
                break;
            case 'GET' && $this->queryMenu == 'review':
                $response = $this->getReview();
                break;
            case 'GET' && $this->queryMenu == 'appartamentiImmagini':
                $response = $this->getAppartamentiImmagini();
                break;
            case 'GET' && $this->queryMenu == 'loginUser' && $this->hashedPw == null:
                $response = $this->getNomeEcognome($this->email);
                break;
            case 'GET' && $this->queryMenu == 'dateDisponibili':
                $response = $this->getDateDisponibili();
                break;
            case 'POST' && $this->queryMenu == "registerUser":
                $response = $this->createUserFromRequest();
                break;
            case 'PUT':
                //$response = $this->updateUserFromRequest($this->userId);
                break;
            case 'DELETE':
                //$response = $this->deleteUser($this->userId);
                break;
            default:
                $response = $this->notFoundResponse();
                break;
        }
        header($response['status_code_header']);
        if ($response['body']) {
            echo $response['body'];
        }
    }

    private function getAllUsers()
    {
        $result = $this->DBquery->getAll();
        $response['status_code_header'] = 'HTTP/1.1 200 OK';
        $response['body'] = json_encode($result);
        return $response;
    }

    private function getAttrazioniTuristiche()
    {
        $result = $this->DBquery->getAttrazioniTuristiche();
        $response['status_code_header'] = 'HTTP/1.1 200 OK';
        $response['body'] = json_encode($result);
        return $response;
    }

    private function getAppartamenti()
    {
        $result = $this->DBquery->getAppartamenti();
        $response['status_code_header'] = 'HTTP/1.1 200 OK';
        $response['body'] = json_encode($result);
        return $response;
    }

    private function getReview()
    {
        $result = $this->DBquery->getReview();
        $response['status_code_header'] = 'HTTP/1.1 200 OK';
        $response['body'] = json_encode($result);
        return $response;
    }

    private function getAppartamentiImmagini()
    {
        $result = $this->DBquery->getAppartamentiImmagini();
        $response['status_code_header'] = 'HTTP/1.1 200 OK';
        $response['body'] = json_encode($result);
        return $response;
    }

    private function getNomeEcognome($email)
    {
        $result = $this->DBquery->getNomeEcognome($email);
        $response['status_code_header'] = 'HTTP/1.1 200 OK';
        $response['body'] = json_encode($result);
        return $response;
    }

    private function getDateDisponibili()
    {
        $result = $this->DBquery->getDateDisponibili();
        $response['status_code_header'] = 'HTTP/1.1 200 OK';
        $response['body'] = json_encode($result);
        return $response;
    }
    

    private function checkLoginUser($hashedName, $hashedPw)
    {
        $result = $this->DBquery->find($hashedName, $hashedPw);
        if (!$result) {
            return $this->notFoundResponse();
        }
        echo "<br>";
        $response['status_code_header'] = 'HTTP/1.1 200 OK';
        $response['body'] = json_encode($result);
        return $response;
    }

    private function createUserFromRequest()
    {
        $input = json_decode(file_get_contents('php://input'), true);
        print_r($input);

        /*if (!$this->validatePerson($input)) {
            return $this->unprocessableEntityResponse();
        }*/

        $this->DBquery->insert($input);
        $response['status_code_header'] = 'HTTP/1.1 201 Created';
        $response['body'] = null;
        return $response;
    }

    /*private function updateUserFromRequest($id)
    {
        $result = $this->DBquery->find($id);

        if (!$result) {
            return $this->notFoundResponse();
        }

        $input = (array) json_decode(file_get_contents('php://input'), true);

        /*if (!$this->validatePerson($input)) {
            return $this->unprocessableEntityResponse();
        }

        $this->DBquery->update($id, $input);
        $response['status_code_header'] = 'HTTP/1.1 200 OK';
        $response['body'] = null;
        return $response;
    }

    private function deleteUser($id)
    {
        $result = $this->DBquery->find($id);
        if (!$result) {
            return $this->notFoundResponse();
        }
        $this->DBquery->delete($id);
        $response['status_code_header'] = 'HTTP/1.1 200 OK';
        $response['body'] = null;
        return $response;
    }*/

    private function validatePerson($input)
    {
        if (!isset($input['firstname'])) {
            return false;
        }
        if (!isset($input['lastname'])) {
            return false;
        }
        return true;
    }

    private function unprocessableEntityResponse()
    {
        $response['status_code_header'] = 'HTTP/1.1 422 Unprocessable Entity';
        $response['body'] = json_encode([
            'error' => 'Invalid input',
        ]);
        return $response;
    }

    private function notFoundResponse()
    {
        $response['status_code_header'] = 'HTTP/1.1 404 Not Found';
        $response['body'] = null;
        return $response;
    }
}
