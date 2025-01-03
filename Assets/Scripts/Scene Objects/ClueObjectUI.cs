using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueObjectUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    //private Outline _outline;

    // Start is called before the first frame update
    void Start()
    {
        //_outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select(bool select) {
        //_outline.enabled = select;
    }

    public void ChangeColor()
    {
        _image.color = Color.blue;
    }
}
