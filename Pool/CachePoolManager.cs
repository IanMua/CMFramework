using System;
using System.Collections.Generic;
using ProjectBase.Base;
using UnityEngine;

namespace ProjectBase.Pool
{
    public class SubCachePool
    {
        private readonly GameObject _fatherGameObject;
        public readonly List<GameObject> GameObjectList;

        public SubCachePool(GameObject gameObject, GameObject cachePoolGameObject)
        {
            _fatherGameObject = new GameObject(gameObject.name);
            _fatherGameObject.transform.parent = cachePoolGameObject.transform;

            GameObjectList = new List<GameObject>();
            Push(gameObject);
        }

        /// <summary>
        /// Push GameObject in sub cache pool
        /// </summary>
        /// <param name="gameObject"></param>
        public void Push(GameObject gameObject)
        {
            GameObjectList.Add(gameObject);
            gameObject.transform.parent = _fatherGameObject.transform;
            // Disable GameObject
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Get GameObject in cache pool
        /// </summary>
        /// <returns></returns>
        public GameObject Get()
        {
            GameObject gameObject = null;
            gameObject = GameObjectList[0];
            GameObjectList.RemoveAt(0);

            // Enable GameObject
            gameObject.SetActive(true);

            //Retrieve the GameObject from the cache pool GameObject
            gameObject.transform.parent = null;

            return gameObject;
        }
    }

    /// <summary>
    /// Cache pool module
    /// </summary>
    public class CachePoolManager : Singleton<CachePoolManager>
    {
        /// <summary>
        /// Cache Pool Container
        /// Set the path of the GameObject to the cache pool name
        /// </summary>
        private readonly Dictionary<string, SubCachePool> _poolContainer =
            new Dictionary<string, SubCachePool>();

        /// <summary>
        /// Cache pool GameObject in the scene
        /// Disabled cache pool GameObject will all be placed in this GameObject
        /// </summary>
        private GameObject _cachePoolGameObject;

        /// <summary>
        /// Get GameObject in cache pool
        /// </summary>
        /// <param name="golName">The name of GameObject list</param>
        /// <returns></returns>
        public GameObject Get(string golName)
        {
            GameObject gameObject = null;
            // To determine whether a cache pool exists based on its name
            if (_poolContainer.ContainsKey(golName) && _poolContainer[golName].GameObjectList.Count > 0)
            {
                gameObject = _poolContainer[golName].Get();
            }
            else
            {
                // Load GameObject from the Resources
                gameObject = GameObject.Instantiate(Resources.Load<GameObject>(golName));
                gameObject.name = golName;
            }

            return gameObject;
        }

        /// <summary>
        /// Push GameObject in cache pool
        /// </summary>
        /// <param name="golName"></param>
        /// <param name="gameObject"></param>
        public void Push(string golName, GameObject gameObject)
        {
            // If cache pool GameObject is null, init GameObject, placed the GameObject inside
            if (_cachePoolGameObject is null)
            {
                _cachePoolGameObject = new GameObject("CachePool");
            }

            // Determine whether a cache pool exists based on its name and determine whether there are any GameObject in the sub cache pool
            if (_poolContainer.TryGetValue(golName, out SubCachePool subCachePoll))
            {
                subCachePoll.Push(gameObject);
            }
            else
            {
                // Add new cache pool to the cache pool container
                _poolContainer.Add(golName, new SubCachePool(gameObject, _cachePoolGameObject));
            }
        }

        /// <summary>
        /// Clear the cache pool container and the cache pool GameObject
        /// <remarks>
        /// 1. This will not delete the GameObjects in the scene, only disconnects them
        /// 2. Call this method when switching scene
        /// </remarks>
        /// </summary>
        public void Clear()
        {
            _poolContainer.Clear();
            _cachePoolGameObject = null;
        }
    }
}