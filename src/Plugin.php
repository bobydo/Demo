<?php

namespace Mjy\Demo;

use Mjy\Demo\Route\StudentRestApiService;
use Mjy\Demo\Route\UniversityRestApiService;
use Mjy\Demo\ShortCode\TableShortCode;
use Mjy\Demo\Menu\Menu;
use Mjy\Demo\Route\SalesRestApiService;

class Plugin {
    public static $entityManager;

    public function init() {
        wp_register_style('bootstrap.min.css', 'https://unpkg.com/bootstrap/dist/css/bootstrap.min.css');
        wp_enqueue_style('bootstrap.min.css');
        wp_register_style('bootstrap-vue.css', 'https://unpkg.com/bootstrap-vue@latest/dist/bootstrap-vue.css');
        wp_enqueue_style('bootstrap-vue.css');

        wp_register_script( 'bootstrap.bundle.min.js', "https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/js/bootstrap.bundle.min.js", array('jquery'));
        wp_enqueue_script( 'bootstrap.bundle.min.js' );

        wp_register_script( 'axios.min.js', "https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js", array('jquery'));
        wp_enqueue_script( 'axios.min.js' );

        wp_register_script( 'vue.js', "https://cdn.jsdelivr.net/npm/vue@2/dist/vue.js");
        wp_enqueue_script( 'vue.js' );

        wp_register_script( 'sweetalert2', "https://cdn.jsdelivr.net/npm/sweetalert2@10");
        wp_enqueue_script( 'sweetalert2' );

        wp_register_script( 'bootstrap-vue.js', "https://unpkg.com/bootstrap-vue@latest/dist/bootstrap-vue.js");
        wp_enqueue_script( 'bootstrap-vue.js' );

        wp_register_script( 'core.js', plugin_dir_url( __FILE__ ) . '../assets/js/core.js', array('jquery'));
        wp_enqueue_script( 'core.js' );

        wp_register_script( 'util.js', plugin_dir_url( __FILE__ ) . '../assets/js/util/util.js', array('jquery'));
        wp_enqueue_script( 'util.js' );

        wp_register_style( 'core.css', plugin_dir_url( __FILE__ ) . '../assets/css/core.css');
        wp_enqueue_style( 'core.css' );

        $tableShortCode = new TableShortCode();
        $tableShortCode->init();

        $menu = new Menu();
        $menu->init();

        $salesRestApi = new SalesRestApiService();
        $salesRestApi->init();

        $studentRestApi = new StudentRestApiService();
        $studentRestApi->init();

        $universityRestApi = new UniversityRestApiService();
        $universityRestApi->init();

        $this->initDatabase();
    }

    private function initDatabase() {
        Plugin::$entityManager = require MJY__PLUGIN_DIR . 'bootstrap.php';
    }
}
