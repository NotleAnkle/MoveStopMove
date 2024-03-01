using _UI.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _UI.Scripts
{
    public class MainMenu : UICanvas
    {
        [SerializeField] private Text txtCoin;
        public override void Open()
        {
            base.Open();
            SimplePool.Collect(PoolType.Bot);
            LevelManager.Instance.OnReset();
            CameraFollower.Instance.ChangeState(CameraFollower.State.MainMenu);

            txtCoin.text = UserData.Ins.coin.ToString();
        }
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
            UIManager.Instance.OpenUI<UIWeaponShop>();
        }
    }
}
