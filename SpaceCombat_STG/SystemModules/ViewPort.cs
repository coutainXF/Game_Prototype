using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ViewPort : Singleton<ViewPort>
{
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private float middleX;
    public float MaxX => maxX;
    
    private void Start()
    {
        Camera mainCamera = Camera.main;

        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
        middleX = (minX + maxX) / 2;
    }

    //限定玩家移动范围
    public Vector3 ClampPlayerMoveableArea(Vector3 playerPosition,float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Math.Clamp(playerPosition.x, minX+paddingX, maxX-paddingX);
        position.y = Math.Clamp(playerPosition.y, minY+paddingY, maxY-paddingY);
        return position;
    }

    
    //生成敌人的随机位置
    public Vector3 RandomEnemySpawnPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = maxX + paddingX;
        position.y = Random.Range(minY+paddingY,maxY-paddingY);
        return position;
    }

    //限定敌人在右侧的随机位置
    public Vector3 RandomRightHalfPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(middleX,maxX - paddingX);
        position.y = Random.Range(minY+paddingY,maxY-paddingY);
        return position;
    }
    
    //限定boss的范围在较为右侧的位置
    public Vector3 RandomBossPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(middleX+5,maxX - paddingX);
        position.y = Random.Range(minY+paddingY,maxY-paddingY);
        return position;
    }

    public Vector3 RandomExpression()
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(middleX+5,maxX-4);
        position.y = Random.Range(minY+5,maxY-4);
        return position;
    }
}
