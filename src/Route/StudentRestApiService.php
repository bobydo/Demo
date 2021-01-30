<?php

namespace Mjy\Demo\Route;

use Mjy\Demo\Model\Student;
use Mjy\Demo\Plugin;
use Exception;
use Mjy\Demo\Util\Util;
use WP_Error;
use WP_REST_Server;

class StudentRestApiService {
    public function init() {
        if ( ! function_exists( 'register_rest_route' ) ) {
            // The REST API wasn't integrated into core until 4.4, and we support 4.0+ (for now).
            return false;
        }
        if(!Util::isAvailableOrigin())
            return false;
        add_action( 'rest_api_init', array($this, 'initRoutes'));

        return true;
    }

    public function initRoutes() {
        register_rest_route( 'demo/v1', '/student', array(
            array(
                'methods' => WP_REST_Server::READABLE,
                'callback' => array($this, 'getStudent'),
                'args' => array(
                    'id' => array(
                        'required' => false,
                        'type' => 'int'
                    ),
                ),
            ), array(
                'methods' => WP_REST_Server::CREATABLE,
                'callback' => array($this, 'createStudent'),
            ), array(
                'methods' => WP_REST_Server::EDITABLE,
                'callback' => array($this, 'updateStudent'),
                'args' => array(
                    'id' => array(
                        'required' => true,
                        'type' => 'int'
                    ),
                ),
            ), [
                'methods' => WP_REST_Server::DELETABLE,
                'callback' => array($this, 'deleteStudent'),
            ]
        ) );
    }

    public function getStudent($request) {
        try {
            $id = $request->get_param('id', null);
            if($id != null) {
                $student = Plugin::$entityManager->find('Mjy\\Demo\\Model\\Student', $id);

                return rest_ensure_response($student->toArray());
            }

            $studentRepository = Plugin::$entityManager->getRepository('Mjy\\Demo\\Model\\Student');
            $students = [];
            foreach ($studentRepository->findBy(['is_deleted' => false]) as $singleStudent) {
                $students[] = $singleStudent->toArray();
            }

            return rest_ensure_response($students);
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }

    public function createStudent($request) {
        try {
            $body = json_decode($request->get_body(), true);
            $firstName = $body['first_name'];
            $lastName = $body['last_name'];
            $university_id = $body['university_id'];
            $resume = $body['resume'];
            $sales_id = $body['sales_id'];
            $resumeUrl = Util::base64ToPdf($resume);
            $student = new Student();
            $student->setFirstName($firstName);
            $student->setLastName($lastName);
            $student->setUniversity(Plugin::$entityManager->find('Mjy\\Demo\\Model\\University', $university_id));
            $student->setResume($resumeUrl);
            $student->setSales(Plugin::$entityManager->find('Mjy\\Demo\\Model\\Sales', $sales_id));

            Plugin::$entityManager->persist($student);
            Plugin::$entityManager->flush();
            return rest_ensure_response($student->toArray());
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }

    public function updateStudent($request) {
        try {
            $id = $request->get_param('id');
            $body = json_decode($request->get_body(), true);

            $student = Plugin::$entityManager->find('Mjy\\Demo\\Model\\Student', $id);

            if(array_key_exists('university_id', $body)) {
                $university_id = $body['university_id'];
                unset($body['university_id']);
                $body['university'] = Plugin::$entityManager->find('Mjy\\Demo\\Model\\University', $university_id);
            }

            if(array_key_exists('sales_id', $body)) {
                $sales_id = $body['sales_id'];
                unset($body['sales_id']);
                $body['sales'] = Plugin::$entityManager->find('Mjy\\Demo\\Model\\Sales', $sales_id);
            }

            // Check if body contains resume and is resume a url
            if(array_key_exists('resume', $body) && !filter_var($body['resume'], FILTER_VALIDATE_URL)) {
                Util::deleteUploadFile($student->getResume());
                $body['resume'] = Util::base64ToPdf($body['resume']);
            }

            $student = $student->updateByArray($body);

            Plugin::$entityManager->flush();
            return rest_ensure_response($student->toArray());
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }

    public function deleteStudent($request) {
        try {
            $id = $request->get_param('id');
            $student = Plugin::$entityManager->getReference('Mjy\\Demo\\Model\\Student', ['id' => $id]);
            $student->setIsDeleted(true);
            Plugin::$entityManager->flush();
            return rest_ensure_response(['success' => 'true']);
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }
}
