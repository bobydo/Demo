<?php

namespace Mjy\Demo\Route;

use Mjy\Demo\Model\Sales;
use Mjy\Demo\Plugin;
use Exception;
use Mjy\Demo\Util\Util;
use WP_Error;
use WP_REST_Server;

class SalesRestApiService {
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
        register_rest_route( 'demo/v1', '/sales', array(
            array(
                'methods' => WP_REST_Server::READABLE,
                'callback' => array($this, 'getSales'),
                'args' => array(
                    'id' => array(
                        'required' => false,
                        'type' => 'int'
                    ),
                ),
            ), array(
                'methods' => WP_REST_Server::CREATABLE,
                'callback' => array($this, 'createSales'),
            ), array(
                'methods' => WP_REST_Server::EDITABLE,
                'callback' => array($this, 'updateSales'),
                'args' => array(
                    'id' => array(
                        'required' => true,
                        'type' => 'int'
                    ),
                ),
            ), [
                'methods' => WP_REST_Server::DELETABLE,
                'callback' => array($this, 'deleteSales'),
            ]
        ) );
    }

    public function getSale($id) {
        try {
            $sales = Plugin::$entityManager->find('Mjy\\Demo\\Model\\Sales', $id);

            return rest_ensure_response($sales->toArray());
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }

    public function getSales($request) {
        try {
            $id = $request->get_param('id', null);
            if($id != null) {
                return $this->getSale($id);
            }

            $salesRepository = Plugin::$entityManager->getRepository('Mjy\\Demo\\Model\\Sales');
            $sales = [];

            foreach ($salesRepository->findBy(['is_deleted' => false]) as $singleSales) {
                $sales[] = $singleSales->toArray();
            }

            return rest_ensure_response($sales);
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }

    public function createSales($request) {
        try {
            $body = json_decode($request->get_body(), true);
            $firstName = $body['first_name'];
            $lastName = $body['last_name'];
            $email = $body['email'];
            $sales = new Sales();
            $sales->setFirstName($firstName);
            $sales->setLastName($lastName);
            $sales->setEmail($email);
            Plugin::$entityManager->persist($sales);
            Plugin::$entityManager->flush();
            return rest_ensure_response($sales->toArray());
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
            //return rest_ensure_response(['error' => $e->getMessage()]);
        }
    }

    public function updateSales($request) {
        try {
            $id = $request->get_param('id');
            $body = json_decode($request->get_body(), true);

            $sales = Plugin::$entityManager->find('Mjy\\Demo\\Model\\Sales', $id);

            $sales = $sales->updateByArray($body);

            Plugin::$entityManager->flush();
            return rest_ensure_response($sales->toArray());
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }

    public function deleteSales($request) {
        try {
            $id = $request->get_param('id');
            $sales = Plugin::$entityManager->getReference('Mjy\\Demo\\Model\\Sales', ['id' => $id]);
            $sales->setIsDeleted(true);
            Plugin::$entityManager->flush();
            return rest_ensure_response(['success' => 'true']);
        } catch (Exception $e) {
            return new WP_Error( 'error', $e->getMessage(), array( 'status' => 400 ) );
        }
    }
}
