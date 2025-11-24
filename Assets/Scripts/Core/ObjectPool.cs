using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Core
{
    public class ObjectPool<T> : IObjectPool<T> where T : Component
    { 
        private readonly T prefab;
        private readonly Transform parent;
        private readonly Queue<T> pool = new Queue<T>();

        public ObjectPool(T prefab, Transform parent = null, int initialSize = 0)
        {
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                T block = Object.Instantiate(prefab, parent);
                block.gameObject.SetActive(false);
                pool.Enqueue(block);
            }
        }


        public T GetObject()
        {
            if(pool.Count > 0)
            {
                T block = pool.Dequeue();
                block.gameObject.SetActive(true);
                return block;
            }
            else
            {
                T block = Object.Instantiate(prefab, parent);
                return block;
            }
        }

        public void ReturnObject(T block)
        {
            block.gameObject.SetActive(false);
            pool.Enqueue(block);
        }
    }
}