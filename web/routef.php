<?php
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;

$app->get('/daftar', function (Silex\Application $app,Request $request)
{
    $result=array();
    $sql = "SELECT * FROM menu_makanan";
    $stmt=$app['db']->query($sql);
    while($row=$stmt->fetch()){
        $result[]=array(
            'id'                =>$row['id'],
            'nama'              =>$row['nama'],
            'harga'             =>$row['harga'],
        );
    }
    
    return  $app->json($result);
});


$app->get('/q/{t}/{j}', function (Silex\Application $app,Request $request,$id)
{
    $result=array();
    $sql = "SELECT * FROM menu_makanan WHERE id={$id}";

    $stmt=$app['db']->query($sql);
    while($row=$stmt->fetch()){
        $result[]=array(
            'id'                =>$row['id'],
            'nama'              =>$row['nama'],
            'harga'             =>$row['harga'],
        );
    }
    
    return  $app->json($result);
});