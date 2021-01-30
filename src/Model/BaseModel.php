<?php


namespace Mjy\Demo\Model;


use Exception;

class BaseModel
{
    /**
     * @Column(type="boolean")
     */
    protected $is_deleted = false;

    public function isDeleted() {
        return $this->is_deleted;
    }

    public function setIsDeleted($isDeleted) {
        $this->is_deleted = $isDeleted;
    }

    public function updateByArray($array) {
        foreach ($array as $key => $value) {
            if($key == 'id')
                continue;

            try {
                $this->$key = $value;
            } catch (Exception $_) {}
        }
        return $this;
    }
}