using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class Character : MonoBehaviour,
    IPointerClickHandler
{
    [SerializeField]
    private string _yarnNode;
    public void OnPointerClick(PointerEventData eventData)
    {
        DialogueHelper.Instance.DialogueRunner.StartDialogue(_yarnNode);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("set_node")]
    public void SetNode(string node)
    {
        _yarnNode = node;
    }
}
