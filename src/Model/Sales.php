<?php


namespace Mjy\Demo\Model;
use Doctrine\Common\Collections\ArrayCollection;

/**
 * @Entity
 * @Table(name="Sales")
 */
class Sales extends BaseModel implements SerializeInterface
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
     * @Column(type="string")
     */
    protected $email;

    /**
     * @OneToMany(targetEntity="Student", mappedBy="sales")
     */
    protected $students;

    public function __construct(){
        $this->students = new ArrayCollection();
    }

    public function getId() {
        return $this->id;
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

    public function getEmail() {
        return $this->email;
    }

    public function setEmail($email) {
        $this->email = $email;
    }

    public function getStudents() {
        return $this->students;
    }

    public function addStudent(Student $student) {
        $this->students[] = $student;
    }

    public function toArray($forbidRecursion=false) {
        $result = [
            'id' => $this->getId(),
            'first_name' => $this->getFirstName(),
            'last_name' => $this->getLastName(),
            'email' => $this->getEmail(),
            'students' => []
        ];

        foreach ($this->getStudents() as $student) {
            $result['students'][] = $student->toArray(true);
        }
        return $result;
    }
}