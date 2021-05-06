using TMPro;
using UnityEngine;

public class QuitText : MonoBehaviour
{
    [Tooltip("How much time the player has to press escape the second time")]
    public float EscapeTime;
    
    //text mesh pro element
    private TextMeshProUGUI _tmp;

    //text smack component
    private TextSmack _textSmack;

    //variable that keeps track of how long the since the first esc press.
    private float _timePassed; 
    void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _textSmack = GetComponent<TextSmack>();
    }
    
    void Update()
    {

        if (Commons.RoomGenerator.CurrentRoomConfig.Class == RoomClass.Starting)
        {
            _timePassed = EscapeTime;
        }

        //If the player just pressed esc
        if (_timePassed >= 0)
        {
            //if this was the first time in a while since the last esc press.
            if (_tmp.text.Equals("[Esc]"))
            {
                //smack
                _textSmack.Smack();
            }
            _tmp.text = "Press Esc to Quit";
            _timePassed -= Time.deltaTime;
            
            //If esc is pressed again shortly after the first
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Quit
                Debug.Log("Shutting down");
                Application.Quit();
            }
        }
        
        //the player has not pressed esc
        else
        {
            _tmp.text = "[Esc]";
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _timePassed = EscapeTime;
        }
    }
}
