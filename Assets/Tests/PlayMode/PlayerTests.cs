using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class PlayerTests
{
    private Player player;
    private Bullet bullet;
    

    [SetUp]
    public void SetUp()
    {
        // Adding player component
        var gameObject = new GameObject();
        player = gameObject.AddComponent<Player>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(player.gameObject);
    }

    [Test]
    public void PlayerTurnLeft()
    {
        player.turnDirection = 1.0f;
        player.rigidbody.AddTorque(player.rotationSpeed * player.turnDirection);
        Assert.AreEqual(1.0f, player.turnDirection);
    }

    [Test]
    public void PlayerTurnRight()
    {
        player.turnDirection = -1.0f;
        player.rigidbody.AddTorque(player.rotationSpeed * player.turnDirection);
        Assert.AreEqual(-1.0f, player.turnDirection);
    }

    [Test]
    public void PlayerThrust()
    {
        player.thrusting = true;
        if (player.thrusting)
        {
            player.rigidbody.AddForce(player.transform.up * player.thrustSpeed);
        }
        Assert.AreEqual(new Vector3(0, 1, 0), player.transform.up);
    }


    // Shooting test; does the prefab instantiate when the shoot function is called?
    [UnityTest]
    public IEnumerator PlayerShoot()
    {
        // Load bullet prefab locally
        Bullet bulletPrefab = (Bullet)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullet.prefab", typeof(Bullet));
        
        // replicate Shoot() locally
        bullet = Object.Instantiate(bulletPrefab, player.transform.position, player.transform.rotation);
        bullet.Project(player.transform.up);

        float initYPos = bullet.transform.position.y;

        yield return new WaitForSeconds(0.1f);

        // Has the bullet left initial position?
        Assert.Greater(bullet.transform.position.y, initYPos);

        Object.Destroy(bullet.gameObject);
    }

    /* 
    [UnityTest]
    public IEnumerator LifeDecrementOnAsteroidCollision()
    {
        // Load asteroid prefab locally
        Asteroid asteroidPrefab = (Asteroid)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Asteroid.prefab", typeof(Asteroid));
        float spawnDistance = 12.0f;
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPoint = spawnDirection * spawnDistance;
        
        Asteroid asteroid = Object.Instantiate(asteroidPrefab, spawnPoint, Quaternion.identity);
        
        asteroid.transform.position = player.transform.position;
        


        yield return new WaitForSeconds(2.0f);

        Assert.AreEqual(2, gameManager.lives);

        Object.Destroy(asteroid);
    }*/
}
