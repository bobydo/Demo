<?php
/**
 * Plugin Name: Demo
 * Description: Mjy demo plugin of Wordpress
 */

use Mjy\Demo\Plugin;

define('MJY__PLUGIN_DIR', plugin_dir_path( __FILE__ ));
define( 'WP_DEBUG', true );

require MJY__PLUGIN_DIR . 'vendor/autoload.php';

$plugin = new Plugin();
$plugin->init();

?>