
using System;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    //播放动画需要的参数
    private float xInput;
    private float yInput;
    private bool isCarrying = false;
    private bool isIdle;
    private bool isLiftingToolDown;
    private bool isLiftingToolLeft;
    private bool isLiftingToolRight;
    private bool isLiftingToolUp;
    private bool isRunning;
    private bool isUsingToolDown;
    private bool isUsingToolLeft;
    private bool isUsingToolRight;
    private bool isUsingToolUp;
    private bool isSwingingToolDown;
    private bool isSwingingToolLeft;
    private bool isSwingingToolRight;
    private bool isSwingingToolUp;
    private bool isWalking;
    private bool isPickingUp;
    private bool isPickingDown;
    private bool isPickingLeft;
    private bool isPickingRight;
    private ToolEffect toolEffect = ToolEffect.none;

    private Rigidbody2D rigidbody2D;
    private float moveSpeed;
    #pragma warning disable 414
    private Direction direction;
    #pragma warning restore 414
    private Camera _camera;
    private bool isPlayerEnableWalk;
    [SerializeField]private SpriteRenderer equipItemSprite;
    public AnimationOverrides _animationOverrides;
    public bool IsPlayerEnableWalk
    {
        get => isPlayerEnableWalk;
        set => isPlayerEnableWalk = value;
    }

    protected override void Awake()
    {
        base.Awake();
        rigidbody2D = GetComponent<Rigidbody2D>();
        _camera=Camera.main;
        isPlayerEnableWalk = true;
        _animationOverrides = GetComponentInChildren<AnimationOverrides>();
    }

    private void Update()
    {
        if (isPlayerEnableWalk == true)
        {
            ResetAnimationTriggers();
            PlayerMovementInput();
            PlayerWalkInput();
            //调用事件发送
            EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                false, false, false, false);
        }
    }

    //取消后，清除自己所选的道具
    public void ClearCarriedItem()
    {
        if(equipItemSprite==null) return;
        equipItemSprite.sprite = null;
        equipItemSprite.color = new Color(1, 1, 1, 0);
    }
    
    //选中后，展示自己举着的道具
    public void ShowCarriedItem(int itemCode)
    {
        ItemDetails details = InventoryManager.Instance.GetItemDetails(itemCode);
        if(details==null||equipItemSprite==null) return;
        if (details.canBeCarried)
        {
            equipItemSprite.sprite = details.itemSprite;
            equipItemSprite.color = new Color(1, 1, 1, 1);
        }
        else
        {
            ClearCarriedItem();
        }
        
    }

    public void DisablePlayerInputAndResetMovement()
    {
        DisablePlayerInput();
        RestMoveMent();
        EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
            isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
            isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
            isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
            isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
            false, false, false, false);
    }

    private void RestMoveMent()
    {
        xInput = 0;
        yInput = 0;
        isWalking = false;
        isRunning = false;
        isIdle = true;
    }

    public void EnablePlayerInput()
    {
        isPlayerEnableWalk = true;
    }

    public void DisablePlayerInput()
    {
        isPlayerEnableWalk = false;
    }

    private void FixedUpdate()
    {
        Vector2 positon = new Vector2(xInput * Time.deltaTime*moveSpeed, yInput * Time.deltaTime*moveSpeed);
        rigidbody2D.MovePosition(rigidbody2D.position+positon);
    }
    
    //处理玩家奔跑的方法
    private void PlayerMovementInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        //考虑对角的走法
        if (xInput != 0 && yInput != 0)
        {
            xInput = xInput * 0.71f;
            yInput = yInput * 0.71f;
        }
        //按下不同时的方向和状态
        if (xInput != 0 || yInput != 0)
        {
            //设置动画参数
            isRunning = true;
            isWalking = false;
            isIdle = false;
            //判断人物方向，为了后面保存用
            if (xInput > 0)
            {
                direction = Direction.right;
            }else if (xInput < 0)
            {
                direction = Direction.left;
            }else if (yInput > 0)
            {
                direction = Direction.up;
            }else
            {
                direction = Direction.down;
            }
        }else if(xInput==0&&yInput==0)
        {
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }
    }
    
    //处理玩家行走的方法，这里walking只是改的速度
    private void PlayerWalkInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isRunning = false;
            isWalking = true;
            isIdle = false;
            moveSpeed = Settings.WalkingSpeed;
        }
        else
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            moveSpeed = Settings.RunningSpeed;
        }
    }
    
    //动画参数复原
    private void ResetAnimationTriggers()
    {
        isPickingRight = false;
        isPickingLeft = false;
        isPickingUp = false;
        isPickingDown = false;
        isUsingToolRight = false;
        isUsingToolLeft = false;
        isUsingToolUp = false;
        isUsingToolDown = false;
        isLiftingToolRight = false;
        isLiftingToolLeft = false;
        isLiftingToolUp = false;
        isLiftingToolDown = false;
        isSwingingToolRight = false;
        isSwingingToolLeft = false;
        isSwingingToolUp = false;
        isSwingingToolDown = false;
        toolEffect = ToolEffect.none;
    }

    public Vector3 GetPlayerViewportPosition()
    {
        return _camera.WorldToViewportPoint(transform.position);
    }
}
