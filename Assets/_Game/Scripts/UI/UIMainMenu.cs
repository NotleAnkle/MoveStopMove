using _UI.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _UI.Scripts
{
    public class UIMainMenu : UICanvas
    {
        [SerializeField] private Text txtCoin;
        public override void Open()
        {
            base.Open();
            GameManager.ChangeState(GameState.MainMenu);
            CameraFollower.Instance.ChangeState(CameraFollower.State.MainMenu);

            txtCoin.text = UserData.Ins.coin.ToString();
        }
        public void PlayButton()
        {
            SoundManager.Instance.Play(AudioType.SFX_ButtonClick);

            UIManager.Instance.CloseUI<UIMainMenu>();
            GameManager.Instance.OnGameStart();
        }

        public void SkinButton()
        {
            SoundManager.Instance.Play(AudioType.SFX_ButtonClick);

            UIManager.Instance.CloseUI<UIMainMenu>();
            UIManager.Instance.OpenUI<UIShop>();
        }

        public void WeaponButton()
        {
            SoundManager.Instance.Play(AudioType.SFX_ButtonClick);

            UIManager.Instance.CloseUI<UIMainMenu>();
            UIManager.Instance.OpenUI<UIWeaponShop>();
        }
    }
}
