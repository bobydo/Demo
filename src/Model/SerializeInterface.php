<?php


namespace Mjy\Demo\Model;


interface SerializeInterface
{
    public function toArray($forbidRecursion=false);
}