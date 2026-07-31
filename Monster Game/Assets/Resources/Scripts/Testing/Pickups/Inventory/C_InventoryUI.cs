using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class C_InventoryUI : MonoBehaviour
{
    C_InventoryManager m_InvManager;
    [SerializeField] Image[] m_ImageSlots;
    [SerializeField] TextMeshProUGUI[] m_ItemNameSlots;
    [SerializeField] Color m_DefaultItemColor = Color.white;
    [SerializeField] Color m_NoItemColor = new Color(255,255,255,0);
    private void Awake()
    {
        m_InvManager = FindAnyObjectByType<C_InventoryManager>();
    }
    private void Start()
    {
        UpdateUI();
        m_InvManager.OnInventoryChange += UpdateUI;
    }
    private void UpdateUI()
    {
        if (m_InvManager == null || m_InvManager.InvSlots == null)
            return;
        
        int invSlotsCount = m_InvManager.InvSlots.Count;
        int uiImageCount;
        if (m_ImageSlots != null)
        {
            uiImageCount = m_ImageSlots.Length;
        }
        else
        {
            uiImageCount = 0;
        }

        int uiNameCount;
        if (m_ItemNameSlots != null)
        {
            uiNameCount = m_ItemNameSlots.Length;
        }
        else
        {
            uiNameCount = 0;
        }
        int slotCount = Mathf.Min(m_InvManager.MaxSlots, invSlotsCount, uiImageCount, uiNameCount);

        // iterate only up to the smallest available count to avoid out-of-range access
        for (int i = 0; i < slotCount; i++)
        {
            Debug.Log("Current I: " + i + " max slots: " + m_InvManager.MaxSlots);
            if (m_InvManager.InvSlots[i] != null)
            {
                m_ImageSlots[i].sprite = m_InvManager.InvSlots[i].m_CurItem.m_Sprite;
                m_ImageSlots[i].color = m_DefaultItemColor;
                m_ItemNameSlots[i].text = m_InvManager.InvSlots[i].m_CurItem.m_ItemName;
            } else
            {
                m_ImageSlots[i].sprite = null;
                m_ImageSlots[i].color = m_NoItemColor;
                m_ItemNameSlots[i].text = null;
            }
        }
        // Clear any remaining UI elements if UI arrays are larger than the inventory sample we iterated
        for (int i = slotCount; i < uiImageCount; i++)
        {
            m_ImageSlots[i].sprite = null;
            m_ImageSlots[i].color = m_NoItemColor;
        }

        for (int i = slotCount; i < uiNameCount; i++)
        {
            m_ItemNameSlots[i].text = string.Empty;
        }
    }
    private void OnDestroy()
    {
        m_InvManager.OnInventoryChange -= UpdateUI;
    }
}
