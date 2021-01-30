<?php

use Doctrine\ORM\EntityManager;
use Doctrine\ORM\Tools\Setup;

if ( !function_exists( 'plugin_dir_path' ) ) {
    require 'vendor/autoload.php';
} else {
    require plugin_dir_path( __FILE__ ). 'vendor/autoload.php';
}

$configuration = Setup::createAnnotationMetadataConfiguration(
    $paths = [(!function_exists( 'plugin_dir_path' )?'':plugin_dir_path( __FILE__ )) . 'src/Model'],
    $isDevMode = true
);

// Setup connection parameters
$connectionParameters = [
    'dbname' => 'demo',
    'user' => 'root',
    'password' => 'mjy159357',
    'host' => 'localhost',
    'driver' => 'pdo_mysql'
];

// Get the entity manager
$entityManager = EntityManager::create($connectionParameters, $configuration);

$dbPlatform = $entityManager->getConnection()->getDatabasePlatform();
$dbPlatform->registerDoctrineTypeMapping('bit', 'boolean');

return $entityManager;
