using TMPro;
using UnityEngine;

public class Q_UIText : MonoBehaviour
{
    
    [SerializeField] private RectTransform arrow;

    [SerializeField] private TextMeshProUGUI f, g, h, p;

    public RectTransform m_Arrow { get => arrow; set => arrow = value; }

    public TextMeshProUGUI F { get => f; set => f = value; }
    public TextMeshProUGUI G { get => g; set => g = value; }
    public TextMeshProUGUI H { get => h; set => h = value; }
    public TextMeshProUGUI P { get => p; set => p = value; }

}
