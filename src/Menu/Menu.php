<?php

namespace Mjy\Demo\Menu;

class Menu {
    public function init() {
        add_action('admin_menu', array($this, 'mjy_script_menu_option'));
        add_action('wp_head', array($this, 'mjy_display_header_scripts'));
        add_action('wp_footer', array($this, 'mjy_display_footer_scripts'));
    }

    public function mjy_script_menu_option() {
        add_menu_page('Mjy Script', 'Mjy Script', 'manage_options', 'mjy_script_menu',  [$this, 'mjy_script_page'], '', 200);
    }

    public function mjy_script_page() {
        if(array_key_exists('submit_script_update', $_POST)) {
            update_option('mjy_header_scripts', $_POST['header_scripts']);
            update_option('mjy_footer_scripts', $_POST['footer_scripts']);

            ?>
            <div id="setting-error-settings_updated" class="updated settings-error notice is-dismissible">
                <h3><strong>Setting has been saved!</strong></h3>
            </div>
            <?php
        }

        $header_scripts = get_option('mjy_header_scripts', null);
        $footer_scripts = get_option('mjy_footer_scripts', null);
        ?>
        <div>
            <h1>Mjy Script Page</h1>
            <form method="post" action="">
                <label for="header_scripts">Header Scripts</label>
                <textarea id="header_scripts" name="header_scripts" class="large-text"><?= $header_scripts ?></textarea>
                <label for="footer_scripts">Header Scripts</label>
                <textarea id="footer_scripts" name="footer_scripts" class="large-text"><?= $footer_scripts ?></textarea>

                <input type="submit" class="button button-primary" name="submit_script_update" value="UPDATE">
            </form>
        </div>
        <?php
    }

    public function mjy_display_header_scripts() {
        $rootUrl = get_site_url();

        // $rootUrl = plugin_dir_url( __FILE__ );
        echo "<script>init('$rootUrl')</script>";

        $header_scripts = get_option('mjy_header_scripts', null);

        return $header_scripts;
    }

    public function mjy_display_footer_scripts() {
        $footer_scripts = get_option('mjy_footer_scripts', null);

        return $footer_scripts;
    }
}
