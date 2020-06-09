using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.NiceVibrations;
using TMPro;
using ToolScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameUI
{
    public class GoldParticleSystem : MonoBehaviour
    {
        private PoolManager _goldParticlePool;

        public RectTransform destinationPosition;

        public GameObject goldParticle;

        private bool _isActive = false;
        
        private readonly Dictionary<RectTransform, Vector2> _activeGoldParticles = new Dictionary<RectTransform, Vector2>();

        private float _distanceToGo;

        private Action _onCompletedCallback;
        private Action<int> _onParticleCompletedCallback;
        public RectTransform startPosition;
        private int gold;
        public TextMeshProUGUI goldText;
        private void Awake()
        {
            _goldParticlePool = new PoolManager();
            _goldParticlePool.SetUnits(new List<GameObject> {goldParticle});
        }

        private void Start()
        {
            ResetGold();
        }

        public void OnEnable()
        {
            GameManager.LevelRestarted += ResetGold;
            GameManager.NextLevelStarted += ResetGold;
        }
        public void OnDisable()
        {
            GameManager.LevelRestarted -= ResetGold;
            GameManager.NextLevelStarted -= ResetGold;
        }
        private void ResetGold()
        {
            gold = 132+Random.Range(-10,10);
            goldText.text = gold.ToString();
        }    
      

        public void PlayGoldEffect()
        {
            StartGoldParticle(startPosition.position, 8,_onParticleCompletedCallback,_onCompletedCallback);
        }

        public void StartGoldParticle(Vector2 startPosition, int particleCount, Action<int/*remaining ParticleCount*/> onParticleCompleted, Action onFinishCallback)
        {
            // Canvas input almayı engelliyor
         //   var canvasGroup = GetComponent<CanvasGroup>();
    //            canvasGroup.interactable = true;
     //       canvasGroup.blocksRaycasts = true;

            BasicCoroutineHandler.Instance.UpdateForSeconds(.3f, value => {  },
                () => { BeginAnimation(startPosition, particleCount, onParticleCompleted, onFinishCallback); });
        }

        private void Stop()
        {
            BasicCoroutineHandler.Instance.WaitForSeconds(.2f, Temp);
        }

        private void Temp()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
               
            _onCompletedCallback?.Invoke();
            _isActive = false;
        }
        
        private void BeginAnimation(Vector2 startPosition, int particleCount, Action<int/*remaining ParticleCount*/> onParticleCompleted,  Action onFinishedCallback)
        {
            _onCompletedCallback = onFinishedCallback;
            _onParticleCompletedCallback = onParticleCompleted;
            
            _activeGoldParticles.Clear();

            var destVector = ((Vector2)destinationPosition.transform.position - startPosition);
            _distanceToGo = destVector.magnitude;
            
            for (int i = 0; i < particleCount; i++)
            {
                var particle = _goldParticlePool.GetObjectFromPool(0, transform).GetComponent<RectTransform>();

                var goldParticleInitialVelocity = _distanceToGo * .4f * (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) - destVector.normalized);

                particle.transform.position = startPosition;
                
                _activeGoldParticles.Add(particle, goldParticleInitialVelocity);
            }

            _isActive = true;
        } 

        private void Update()
        {
            if (!_isActive) return;
            
            var keys = _activeGoldParticles.Keys.ToArray();

            foreach (var particle in keys)
            {
                var previousPosition = (Vector2) particle.transform.position;
                
                var velocity = _activeGoldParticles[particle];
                particle.transform.position += (Vector3) velocity * Time.deltaTime;

                var distance = (Vector2)(destinationPosition.transform.position - particle.transform.position);
                var previousDistance = ((Vector2) destinationPosition.transform.position - previousPosition);
                
                var previousVelocity = velocity;
                velocity += _distanceToGo *
                            Time.deltaTime * 1.1f * distance.normalized;

                var projectedVector = ProjectedVector(velocity,distance);
                var vectorComponent = velocity - projectedVector;

                velocity -= 2 * Time.deltaTime * vectorComponent;
                
                particle.transform.localScale = Mathf.Pow(distance.magnitude / _distanceToGo, .2f)  * Vector3.one;

                _activeGoldParticles[particle] = velocity;
                
                if (Vector2.Dot(velocity, distance) <= 0 && Vector2.Dot(previousVelocity, previousDistance) >= 0)
                {
                    gold += 10+Random.Range(-2,5);
                    goldText.text = gold.ToString();
                    _onParticleCompletedCallback?.Invoke(_activeGoldParticles.Count);
                    
                    _activeGoldParticles.Remove(particle);
                    _goldParticlePool.AddObjectToPool(particle.gameObject);

                    MMVibrationManager.Haptic(HapticTypes.Selection);
                    
                    if (_activeGoldParticles.Count == 0)
                    {
                        Stop();
                    }
                }
            }
        }
        public static Vector2 ProjectedVector(Vector2 A, Vector2 B)
        {
            return Vector2.Dot(B, A) / B.sqrMagnitude * B;
        }
    }
   
}
