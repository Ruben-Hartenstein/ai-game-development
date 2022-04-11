using RelegatiaCCG.rccg.frontend.animations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace rccg.frontend
{
    public class GUIResourceLoader
    {
        public const string PATH_SEPARATOR = "/";

 
        protected static GUIResourceLoader resourceLoaderInstance;

        private Dictionary<string, object> resourceCache;

        protected GUIResourceLoader()
        {
            resourceCache = new Dictionary<string, object>();
        }

        public static GUIResourceLoader getResourceLoaderInstance()
        {
            if (resourceLoaderInstance == null)
            {
                resourceLoaderInstance = new GUIResourceLoader();
            }

            return resourceLoaderInstance;
        }

        public void Awake()
        {
            resourceLoaderInstance = this;
        }

        public void Start()
        {
            resourceLoaderInstance = this;
        }


        public Sprite[] loadSprites(string path)
        {
            return loadAll<Sprite>(path);
        }

        public Sprite loadSprite(string path)
        {
            return loadResource<Sprite>(path);
        }
        


  

  
        public UnityEngine.Object loadPrefab(string path)
        {
            return loadResource<UnityEngine.Object>(path);
        }

        
        private T[] loadAll<T>(string path) where T : UnityEngine.Object
        {
            T[] loadedResources = null;
            if (resourceCache.ContainsKey(path))
            {
                loadedResources = (T[])resourceCache[path];
            }
            else
            {
                loadedResources = Resources.LoadAll<T>(path);
                resourceCache.Add(path, loadedResources);
            }
            return loadedResources;
        }

        private T loadResource<T>(string path) where T : UnityEngine.Object
        {
            T loadedResource = null;
            if (resourceCache.ContainsKey(path))
            {
                loadedResource = (T)resourceCache[path];
            }
            else
            {
                loadedResource = Resources.Load<T>(path);
                resourceCache.Add(path, loadedResource);
            }
            return loadedResource;
        }

        
        public const string MINIGAME_PATH = "minigame/";
        public const string MINIGAME_IMAGE_PATH = "minigame/images/";
        public const string MINIGAME_TILESET_PATH = "minigame/tilesets/";
        public const string MINIGAME_PREFAB_PATH = "minigame/prefabs/";
        public const string MINIGAME_LEVEL_PATH = "minigame/levels/";


        public Sprite[] loadMinigameTileset(string name)
        {
            return loadSprites(MINIGAME_TILESET_PATH + name);
        }

        public Sprite[] loadMinigameSprites(string name)
        {
            return loadSprites(MINIGAME_IMAGE_PATH + name);
        }

        public UnityEngine.Object loadMinigamePrefab(string name)
        {
            return loadResource<UnityEngine.Object>(MINIGAME_PREFAB_PATH + name);
        }

        public Texture2D loadMinigameLevel(string game, string level)
        {
            Debug.LogError("Loading " + MINIGAME_LEVEL_PATH + game + PATH_SEPARATOR + level);
            return loadResource<Texture2D>(MINIGAME_LEVEL_PATH + game + PATH_SEPARATOR + level);
        }
    }
}
