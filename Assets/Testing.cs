﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;


public class Testing : MonoBehaviour
{
    [SerializeField] private bool useJobs;
    [SerializeField] private Transform pfZombie;
    private List<Zombie> zombieList;


    public class Zombie 
    {
        public Transform transform;
        public float moveY;
    }

    private void Start()
    {
        zombieList = new List<Zombie>();
        for (int i = 0; i < 1000; i++) 
        {
            Transform zombieTransform = Instantiate(pfZombie, new Vector3(UnityEngine.Random.Range(-8f, 8), UnityEngine.Random.Range(-5f, 5f)), Quaternion.identity);
            zombieList.Add(new Zombie
            {
                transform = zombieTransform,
                moveY = UnityEngine.Random.Range(1f,2f)
            }) ;
        }
    }

    private void Update()
    {
        float startTime = Time.realtimeSinceStartup;
        foreach (Zombie zombie in zombieList) 
        {
            zombie.transform.position += new Vector3(0, zombie.moveY * Time.deltaTime);
            if (zombie.transform.y > 5f) 
            {
                zombie.moveY = -math.abs(zombie.moveY);
            }
            if (zombie.transform.y < -5f)
            {
                zombie.moveY = +math.abs(zombie.moveY);
            }
        }


        /*
        if (useJobs)
        {
            NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
            for (int i = 0; i < 10; i++)
            {
                JobHandle jobHandle = ReallyToughTaskJob();
                jobHandleList.Add(jobHandle);
            }
            JobHandle.CompleteAll(jobHandleList);
            jobHandleList.Dispose();
        }
        else 
        {
            for (int i = 0; i < 10; i++)
            {
                ReallyToughTask();
            }
        }
       
        Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
        */
        
    }
    private void ReallyToughTask() 
    {
        float value = 0f;
        for (int i = 0; i < 50000; i++) 
        {
            value = math.exp10(math.sqrt(value));
        }
    
    }
    private JobHandle ReallyToughTaskJob() 
    {
        ReallyThoughJob job = new ReallyThoughJob();
        return job.Schedule();    
    }
}

[BurstCompile]
public struct ReallyThoughJob : IJob
{
    public void Execute()
    {
        float value = 0f;
        for (int i = 0; i < 50000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}

public struct ReallyToughParrallelJob : IJobParallelFor
{
    public NativeArray<float> moveYArray;
   public NativeArray<float3> positionArray;
    public void Execute(int index)
    {
        

        positionArray += new Vector3(0, moveYArray * Time.deltaTime);
        if (positionArray.y > 5f)
        {
            moveYArray = -math.abs(moveYArray);
        }
        if (positionArray.y < -5f)
        {
            moveYArray = +math.abs(moveYArray);
        }
    }
}