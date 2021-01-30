<?php

namespace Mjy\Demo\Route;

use Mjy\Demo\Model\University;
use Mjy\Demo\Plugin;
use Exception;
use Mjy\Demo\Util\Util;
use WP_Error;
use WP_REST_Server;

class UniversityRestApiService {
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
        register_rest_route( 'demo/v1', '/university', array(
            array(
                'methods' => WP_REST_Server::READABLE,
                'callback' => array($this, 'getUniversity'),
                'args' => array(
                    'id' => array(
                        'required' => false,
                        'type' => 'int'
                    ),
                ),
            ), array(
                'methods' => WP_REST_Server::CREATABLE,
                'callback' => array($this, 'createUniversity'),
            ), array(
                'methods' => WP_REST_Server::EDITABLE,
                'callback' => array($this, 'updateUniversity'),
                'args' => array(
                    'id' => array(
                        'required' => true,
                        'type' => 'int'
                    ),
                ),
            ), [
                'methods' => WP_REST_Server::DELETABLE,
                'callback' => array($this, 'deleteUniversity'),
            ]
        ) );
    }

    public function getUniversity($request) {
        try {
            $id = $request->get_param('id', null);
            if($id != null) {
                $university = Plugin::$entityManager->find('Mjy\\Demo\\Model\\University', $id);
                if($university == null)
                    throw new Exception('Id is invalid');
                return rest_ensure_response($university->toArray());
            }

            $universityRepository = Plugin::$entityManager->getRepository('Mjy\\Demo\\Model\\University');
            $universities = [];
            foreach ($universityRepository->findBy(['is_deleted' => false]) as $university) {
                $universities[] = $university->toArray();
            }

            return rest_ensure_response($universities);
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }

    public function createUniversity($request) {
        try {
            $body = json_decode($request->get_body(), true);
            $name = $body['name'];

            $university = new University();
            $university->setName($name);

            Plugin::$entityManager->persist($university);
            Plugin::$entityManager->flush();
            return rest_ensure_response($university->toArray());
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }

    public function updateUniversity($request) {
        try {
            $id = $request->get_param('id');
            $body = json_decode($request->get_body(), true);

            $university = Plugin::$entityManager->find('Mjy\\Demo\\Model\\University', $id);

            $university = $university->updateByArray($body);

            Plugin::$entityManager->flush();
            return rest_ensure_response($university->toArray());
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }

    public function deleteUniversity($request) {
        try {
            $id = $request->get_param('id');
            $university = Plugin::$entityManager->getReference('Mjy\\Demo\\Model\\University', ['id' => $id]);
            $university->setIsDeleted(true);
            Plugin::$entityManager->flush();
            return rest_ensure_response(['success' => 'true']);
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }
}
