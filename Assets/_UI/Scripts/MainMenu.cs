namespace _UI.Scripts
{
    public class MainMenu : UICanvas
    {
        public void PlayButton()
        {
            LevelManager.Instance.OnInit();
            UIManager.Instance.CloseUI<MainMenu>();
            UIManager.Instance.OpenUI<PlayUI>();
        }

        public void SkinButton()
        {
            UIManager.Instance.CloseUI<MainMenu>();
            UIManager.Instance.OpenUI<SkinShop>();
        }

        public void WeaponButton()
        {
            UIManager.Instance.CloseUI<MainMenu>();
            UIManager.Instance.OpenUI<WeaponShop>();
        }
    }
}
