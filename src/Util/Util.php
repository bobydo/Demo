<?php

namespace Mjy\Demo\Util;

use Exception;
use Mjy\Demo\Common\Setting;

class Util {
    /**
     * @param $path
     * @param array $params
     * @return false|string
     */
    public static function loadFile($path, $params=[]) {
        ob_start();
        require $path;
        $html = ob_get_clean();
        $html = str_replace('%%params%%', json_encode($params), $html);

        return $html;
    }

    public static function isAvailableOrigin() {
        return in_array($_SERVER['REMOTE_ADDR'], Setting::$CORS_WHITELIST);
    }

    public static function base64ToPdf($base64) {
        try {
            $filename = bin2hex(openssl_random_pseudo_bytes(16)) . '.pdf';
            $url = plugin_dir_url( __FILE__ ) . '../../uploads/' . $filename;
            $base64 = explode(';base64,', $base64)[1];
            $pdf_decoded = base64_decode($base64);

            file_put_contents(plugin_dir_path(__FILE__) . '../../uploads/' . $filename, $pdf_decoded);

            return $url;
        } catch (Exception $e) {
            throw $e;
        }
    }

    public static function deleteUploadFile($url) {
        $filename = end(explode('/', $url));
        unlink(plugin_dir_path(__FILE__) . '../../uploads/' . $filename);
    }
}
