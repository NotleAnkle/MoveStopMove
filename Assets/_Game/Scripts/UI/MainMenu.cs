using _UI.Scripts.UI;

namespace _UI.Scripts
{
    public class MainMenu : UICanvas
    {
        public void PlayButton()
        {
            UIManager.Instance.CloseUI<MainMenu>();
            GameManager.Instance.OnRestart();
        }

        public void SkinButton()
        {
            UIManager.Instance.CloseUI<MainMenu>();
            UIManager.Instance.OpenUI<UIShop>();
        }

        public void WeaponButton()
        {
            UIManager.Instance.CloseUI<MainMenu>();
            UIManager.Instance.OpenUI<WeaponShop>();
        }
    }
}
