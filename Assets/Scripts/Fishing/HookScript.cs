using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using vector3;
public class HookScript : MonoBehaviour
{
    private GameObject player;
    private GameObject enemy;

    LineRenderer LineRenderer;

    bool isHooking;
    bool wasEnemyHooked;

    float hookDistance;
    Vector3 originalPosition;

    Rigidbody rb;
    private void Awake(){
            player = GameObject.FindGameObjectWithTag("Player");
            
            LineRenderer = GetComponent<LineRenderer>();
            isHooking = false;
            wasEnemyHooked = false;
            hookDistance = 0;
            rb = GetComponent<Rigidbody>();
            originalPosition = new Vector3(player.transform.position.x + Constants.X_OFFSET, player.transform.position.y + Constants.Y_OFFSET, player.transform.position.z + Constants.Z_OFFSET);
    }

    private void Update(){
        originalPosition = new Vector3(player.transform.position.x + Constants.X_OFFSET, player.transform.position.y + Constants.Y_OFFSET, player.transform.position.z + Constants.Z_OFFSET);
        LineRenderer.SetPosition(0, originalPosition);
        LineRenderer.SetPosition(1, transform.position);

        if(!isHooking && !wasEnemyHooked){
            MovePlayer();
        }
        if(Input.GetMouseButtonDown(0) && !isHooking && !wasEnemyHooked){
            StartHooking();
        }
        ReturnHook();
        BringEnemyTowardsPlayer();
        
    }

    private void StartHooking(){

        //Next implement how to add animation when start throwing the hook
        isHooking = true;
        rb.isKinematic = false;
        rb.AddForce(transform.forward*Constants.HOOK_SPEED);
        // rb.AddForce(transform.animation)
    }

    private void ReturnHook() {
        if(isHooking){

            hookDistance = Vector3.Distance(transform.position, originalPosition);
            if(hookDistance > Constants.MAX_HOOK_DISTANCE){
                rb.isKinematic = true;
                transform.position = originalPosition;
                isHooking = false;
            }
        }
    }

    private void BringEnemyTowardsPlayer(){

        //Once hooked pull item toward player
        if(wasEnemyHooked){
            Vector3 finalPosition;
            finalPosition = new Vector3(originalPosition.x, enemy.transform.position.y, originalPosition.z + Constants.ENEMY_Z_OFFSET);
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, finalPosition, Constants.MAX_HOOK_DISTANCE);
            wasEnemyHooked = false;
        }
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag.Equals("Object")){
            wasEnemyHooked = true;
            enemy = collider.gameObject;
        }
    }
    //Adding VR controller input here later
    private void MovePlayer(){
        if(Input.GetKey(KeyCode.W)){
            player.transform.Translate(player.transform.forward*Constants.PLAYER_SPEED);
        }
        if(Input.GetKey(KeyCode.D)){
            player.transform.Translate(player.transform.right*Constants.PLAYER_SPEED);
        }
        if(Input.GetKey(KeyCode.S)){
            player.transform.Translate(-player.transform.forward*Constants.PLAYER_SPEED);
        }
        if(Input.GetKey(KeyCode.A)){
            player.transform.Translate(-player.transform.right*Constants.PLAYER_SPEED);
        }

    }
}


