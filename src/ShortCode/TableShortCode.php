<?php

namespace Mjy\Demo\ShortCode;

use Mjy\Demo\Util\Util;

class TableShortCode {
    public function init() {
        add_shortcode('salesTable', [$this, 'createSalesTable']);
        add_shortcode('studentTable', [$this, 'createStudentTable']);
        add_shortcode('universityTable', [$this, 'createUniversityTable']);
    }

    public function createSalesTable($attrs, $content) {
        return Util::loadFile(MJY__PLUGIN_DIR . 'src/Template/SalesTable.vue', ["rootUrl" => get_site_url()]);
    }

    public function createStudentTable($attrs, $content) {
        return Util::loadFile(MJY__PLUGIN_DIR . 'src/Template/StudentTable.vue', ["rootUrl" => get_site_url()]);
    }

    public function createUniversityTable($attrs, $content) {
        return Util::loadFile(MJY__PLUGIN_DIR . 'src/Template/UniversityTable.vue', ["rootUrl" => get_site_url()]);
    }
}

