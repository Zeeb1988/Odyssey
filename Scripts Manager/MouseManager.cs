using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEngine.Events;


 //[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { };
public class MouseManager : Singleton<MouseManager>
{
    //public  EventVector3 whenMouseClick;
    public event Action<Vector3> whenMouseClick;
    public event Action<GameObject> whenMouseClickEnemy;

    public RaycastHit mouseClickPosition; 
    //public static MouseManager Instance ;
    public Texture2D Attack, Basic, Map, Hand, Move;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        SetCurSorTexture();
        MouseController();   
    }
    private void FixedUpdate()
    {
        
    }
    
    /*void Awake()
    {
        if (Instance!=null) { Destroy(gameObject); }
        Instance = this;
    }*/
    //�����߻�ȡ�����λ������
    void SetCurSorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        
        if (Physics.Raycast(ray,out mouseClickPosition))
        {
            switch (mouseClickPosition.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(Move, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(Attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Attackable":
                    Cursor.SetCursor(Attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(Map, new Vector2(16, 16), CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(Basic, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }
    //�����λ�����괫�ݸ�NavMeshAgent.destination��Ŀ��㣩
    void MouseController()
    {
        if (Input.GetMouseButton(1) && mouseClickPosition.collider!=null)
        {
            if (mouseClickPosition.collider.gameObject.CompareTag("Ground"))
            {
                whenMouseClick?.Invoke(mouseClickPosition.point);
            }
            if (mouseClickPosition.collider.gameObject.CompareTag("Enemy"))
            {
                whenMouseClickEnemy?.Invoke(mouseClickPosition.collider.gameObject);
            }
            if (mouseClickPosition.collider.gameObject.CompareTag("Attackable"))
            {
                whenMouseClickEnemy?.Invoke(mouseClickPosition.collider.gameObject);
            }
            if (mouseClickPosition.collider.gameObject.CompareTag("Portal"))
            {
                whenMouseClick?.Invoke(mouseClickPosition.point);
            }

        }
         

    }
    
}
