using System;
using System.Collections.Generic;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public interface IWorldController
    {
        void WrapFloatingObjectsPositionsIfOutsideBoundaries();
        void RegisterFloatingObject(IFloatingObject floatingObject);
        void RemoveFloatingObject(IFloatingObject floatingObject);
    }
    public class WorldController : MonoBehaviour, IWorldController
    {
        private List<IFloatingObject> _floatingObjects = new List<IFloatingObject>();

        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;
        
        private void Start()
        {
            var mainCam = Camera.main;
            var ratio = (float)Screen.width / Screen.height;
            _minX = -mainCam.orthographicSize * ratio;
            _maxX = mainCam.orthographicSize * ratio;
            _minY = -mainCam.orthographicSize;
            _maxY = mainCam.orthographicSize;
        }

        public void WrapFloatingObjectsPositionsIfOutsideBoundaries()
        {
            foreach (var floatingObject in _floatingObjects)
            {
                if (floatingObject.Position.x < _minX)
                {
                    floatingObject.Position = new Vector2(_maxX, floatingObject.Position.y);
                }
                
                if (floatingObject.Position.x > _maxX)
                {
                    floatingObject.Position = new Vector2(_minX, floatingObject.Position.y);
                }
                
                if (floatingObject.Position.y < _minY)
                {
                    floatingObject.Position = new Vector2(floatingObject.Position.x, _maxY);
                }
                
                if (floatingObject.Position.y > _maxY)
                {
                    floatingObject.Position = new Vector2(floatingObject.Position.x, _minY);
                }
            }
        }

        public void RegisterFloatingObject(IFloatingObject floatingObject)
        {
            _floatingObjects.Add(floatingObject);
        }

        public void RemoveFloatingObject(IFloatingObject floatingObject)
        {
            _floatingObjects.Remove(floatingObject);
        }

        private void Update()
        {
            WrapFloatingObjectsPositionsIfOutsideBoundaries();
        }
    }
}