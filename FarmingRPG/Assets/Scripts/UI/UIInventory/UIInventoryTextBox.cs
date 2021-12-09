
using TMPro;
using UnityEngine;

public class UIInventoryTextBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;
    [SerializeField] private TextMeshProUGUI text3;
    [SerializeField] private TextMeshProUGUI text4;
    [SerializeField] private TextMeshProUGUI text5;
    [SerializeField] private TextMeshProUGUI text6;

    public void SetTextBoxInfomation(string t1, string t2, string t3, string t4, string t5, string t6)
    {
        text1.text = t1;
        text2.text = t2;
        text3.text = t3;
        text4.text = t4;
        text5.text = t5;
        text6.text = t6;
        
    }
    
}
