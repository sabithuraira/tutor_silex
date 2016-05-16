<?php 
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;

$app->get('/hello', function () {
    return 'Hello!';
});

$app->get('/daftar', function (Silex\Application $app,Request $request)
{
    $result=array();
    $sql = "SELECT * FROM food_list";
    $stmt=$app['db']->query($sql);
    while($row=$stmt->fetch()){
        $result[]=array(
            'id'                =>$row['id'],
            'name'              =>$row['name'],
            'price'             =>$row['price'],
        );
    }
    
    return  $app->json($result);
});


$app->get('/daftar/{id}', function (Silex\Application $app,Request $request,$id)
{
    $result=array();
    $sql = "SELECT * FROM food_list WHERE id={$id}";

    $stmt=$app['db']->query($sql);
    while($row=$stmt->fetch()){
        $result[]=array(
            'id'                =>$row['id'],
            'name'              =>$row['name'],
            'price'             =>$row['price'],
        );
    }
    
    return  $app->json($result);
});

$app->post('/insert', function (Silex\Application $app, Request $request) {
    $data=array(
        'name' => $request->get('name'),
        'price' => $request->get('price'),
    );

    $app['db']->insert('food_list', $data);

    return  $app->json("Success save data");
});


$app->put('/update/{id}', function (Silex\Application $app, Request $request,$id) {
    $data=array(
        'name' => $request->get('name'),
        'price' => $request->get('price'),
    );

    //$conn->update('user', array('username' => 'jwage'), array('id' => 1));
    $app['db']->update('food_list', $data,array('id'=>$id));
    return  $app->json("Success update data");
});

$app->delete('/delete/{id}', function (Silex\Application $app, Request $request,$id) {
    $app['db']->delete('food_list', array('id'=>$id));
    return  $app->json("Success delete data");
});