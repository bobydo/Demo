# Wordpress plugin demo

## Environment
* PHP 7.2 or higher
* composer 2.0 or higher
* Wordpress 5.6

## Install
1. Download wordpress on your server (Download Link: https://wordpress.org/latest.zip)
2. Unzip everything in <strong>latest.zip</strong> and copy to your web server document folder (If you are using <strong>Xampp</strong>, then copy them into <strong>htdocs</strong> folder)
3. Create a new folder under <strong>YOUR_WORDPRESS_PATH/wp-content/plugins</strong>, then copy everything of this plugin into this folder
4. Open your terminal under the path that you just created. 
   * For example: <strong>cd YOUR_WORDPRESS_PATH/wp-content/plugins/demo</strong>, where "demo" is the folder that contains everything of the plugin
5. Open <strong>bootstrap.php</strong> under your plugin root path by any editor, edit <strong>$connectionParameters</strong> to your database connection information
6. Run command lines: 
   * composer install
   * composer dump-autoload -o
   * vendor\bin\doctrine.bat orm:schema-tool:create or vendor\bin\doctrine orm:schema-tool:create, <strong>depends on your operation system</strong>
7. Go to your wordpress <strong>admin dashboard -> plugins -> activate this plugin which is named by "Demo"</strong>
8. Done

## Play around
* This plugin adds 3 short codes for now which are [salesTable], [studentTable] and [universityTable]
* You can access RESTFul API endpoints by 
  * http://localhost/wordpress/wp-json/demo/v1/sales
  * http://localhost/wordpress/wp-json/demo/v1/university
  * http://localhost/wordpress/wp-json/demo/v1/student