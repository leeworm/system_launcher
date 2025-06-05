using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterScrollItemData : InfiniteScrollData
{
    public int ChapterNo;
}

public class ChapterScrollItem : InfiniteScrollItem
{
    public GameObject CurrChapter;
    public RawImage CurrChapterBg;
    public Image Dim;
    public Image LockIcon;
    public Image Round;
    public ParticleSystem ComingSoonFX;
    public TextMeshProUGUI ComingSoonTxt;

    private ChapterScrollItemData m_ChapterScrollItemData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        m_ChapterScrollItemData = scrollData as ChapterScrollItemData;
        if (m_ChapterScrollItemData == null)
        {
            Logger.LogError("Invalid ChapterScrollItemData.");
            return;
        }

        if (m_ChapterScrollItemData.ChapterNo > GlobalDefine.MAX_CHAPTER)
        {
            CurrChapter.SetActive(false);
            ComingSoonFX.gameObject.SetActive(true);
            ComingSoonTxt.gameObject.SetActive(true);
        }
        else
        {
            CurrChapter.SetActive(true);
            ComingSoonFX.gameObject.SetActive(false);
            ComingSoonTxt.gameObject.SetActive(false);

            var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
            if (userPlayData != null)
            {
                var isLocked = m_ChapterScrollItemData.ChapterNo > userPlayData.MaxClearedChapter + 1;
                Dim.gameObject.SetActive(isLocked);
                LockIcon.gameObject.SetActive(isLocked);
                Round.color = isLocked ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white;
            }

            var bgTexture = Resources.Load($"ChapterBG/Background{m_ChapterScrollItemData.ChapterNo.ToString("D3")}") as Texture2D;
            if (bgTexture != null)
            {
                CurrChapterBg.texture = bgTexture;
            }
        }
    }
}