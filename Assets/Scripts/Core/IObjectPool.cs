using UnityEngine;

namespace Core
{
    public interface IObjectPool<T> where T : Component
    {
        T GetObject();
        void ReturnObject(T obj);
    }
}