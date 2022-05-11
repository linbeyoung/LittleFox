using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDialog : MonoBehaviour
{   

    public GameObject enterDialog;
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            enterDialog.SetActive(true);
            // Debug.Log("Enter");
            // DialogManager.Instance.ShowDialog();
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        enterDialog.SetActive(false);
    }
}
