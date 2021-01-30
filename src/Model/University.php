<?php


namespace Mjy\Demo\Model;

/**
 * @Entity
 * @Table(name="university")
 */
class University extends BaseModel implements SerializeInterface
{
    /**
     * @Id
     * @GeneratedValue
     * @Column(type="integer")
     */
    protected $id;

    /**
     * @Column(type="string")
     */
    protected $name;

    public function getId() {
        return $this->id;
    }

    public function getName() {
        return $this->name;
    }

    public function setName($name) {
        $this->name = $name;
    }

    public function toArray($forbidRecursion=false) {
        return [
            'id' => $this->getId(),
            'name' => $this->getName()
        ];
    }
}