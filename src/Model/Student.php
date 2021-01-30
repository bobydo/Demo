<?php


namespace Mjy\Demo\Model;

use phpDocumentor\Reflection\Types\This;

/**
 * @Entity
 * @Table(name="Student")
 */
class Student extends BaseModel implements SerializeInterface
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
    protected $first_name;

    /**
     * @Column(type="string")
     */
    protected $last_name;

    /**
     * @ManyToOne(targetEntity="University", inversedBy="student")
     */
    protected $university;

    /**
     * @Column(type="string")
     */
    protected $resume;

    /**
     * @ManyToOne(targetEntity="Sales", inversedBy="student")
     */
    protected $sales;

    public function getId() {
        return $this->id;
    }

    public function getSales() {
        return $this->sales;
    }

    public function setSales($sales) {
        $this->sales = $sales;
    }

    public function getFirstName() {
        return $this->first_name;
    }

    public function setFirstName($firstName) {
        $this->first_name = $firstName;
    }

    public function getLastName() {
        return $this->last_name;
    }

    public function setLastName($lastName) {
        $this->last_name = $lastName;
    }

    public function getUniversity() {
        return $this->university;
    }

    public function setUniversity($university) {
        $this->university = $university;
    }

    public function getResume() {
        return $this->resume;
    }

    public function setResume($resume) {
        $this->resume = $resume;
    }

    public function toArray($forbidRecursion=false) {
        return [
            'id' => $this->getId(),
            'first_name' => $this->getFirstName(),
            'last_name' => $this->getLastName(),
            'university' => $this->getUniversity()->toArray(),
            'resume' => $this->getResume(),
            'sales' => $forbidRecursion?$this->getSales()->getId():$this->getSales()->toArray()
        ];
    }
}