using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Script.Manager.Input;

namespace Assets.TestInput
{
    public class TestButton : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, 
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IKeyDownHandler, IKeyUpHandler,
        IKeyHoldHandler, IKeyBeginHoldHandler, IKeyEndHoldHandler
    {
        [SerializeField]
        GameObject _labelPress;
        
        [SerializeField]
        GameObject _labelHold;
        
        [SerializeField]
        GameObject _labelRelease;

        Vector3 _position;
        
        /// <summary>
        /// インスタンス生成直後に実行される処理
        /// </summary>
        void Awake ()
        {
            _labelPress.SetActive(false);
            _labelHold.SetActive(false);
            _labelRelease.SetActive(true);
        }
        
        /// <summary>
        /// 毎フレーム実行される更新処理
        /// </summary>
        void Update ()
        {
            
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Pointer down.");
            Debug.Log("clickCount:"+ eventData.clickCount);
                
            Debug.Log(eventData.ToString());

            _labelPress.SetActive(true);
            _labelRelease.SetActive(false);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Pointer up.");
            Debug.Log(eventData.ToString());

            _labelPress.SetActive(false);
            _labelRelease.SetActive(true);
            _labelHold.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _labelHold.SetActive(true);
            transform.position = eventData.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Drag begin.");
            Debug.Log(eventData.ToString());

            _labelPress.SetActive(false);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("Drag end.");
            Debug.Log(eventData.ToString());
        }

        public void OnKeyDown(KeyEventData eventData)
        {
            Debug.Log("Key down.");
            Debug.Log(eventData.ToString());
        }

        public void OnKeyUp(KeyEventData eventData)
        {
            Debug.Log("Key up.");
            Debug.Log(eventData.ToString());
        }

        public void OnKeyHold(KeyEventData eventData)
        {
            Debug.Log("Key hold.");
            Debug.Log(eventData.ToString());
        }

        public void OnKeyBeginHold(KeyEventData eventData)
        {
            Debug.Log("Key begin hold.");
            Debug.Log(eventData.ToString());
        }

        public void OnKeyEndHold(KeyEventData eventData)
        {
            Debug.Log("Key end hold.");
            Debug.Log(eventData.ToString());
        }
    }
}
