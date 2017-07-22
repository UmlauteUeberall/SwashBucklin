using NDream.AirConsole;
using Newtonsoft.Json.Linq;

namespace Ocean
{
    public class CPlayer
    {
        public int mu_deviceId;
        public int mu_playerId;
        public bool mu_isActivePlayer;

        public int pu_isReady;
        public int pu_healthAmount;
        public int pu_tokenGunAmount;
        public int pu_tokenHookAmount;
        public int pu_tokenStraightAmount;
        public int pu_tokenLeftAmount;
        public int pu_tokenRightAmount;
        public int pu_movement1Mode;
        public int pu_movement1LeftMode;
        public int pu_movement1RightMode;
        public int pu_movement2Mode;
        public int pu_movement2LeftMode;
        public int pu_movement2RightMode;
        public int pu_movement3Mode;
        public int pu_movement3LeftMode;
        public int pu_movement3RightMode;
        public int pu_movement4Mode;
        public int pu_movement4LeftMode;
        public int pu_movement4RightMode;
        public int pu_gameState;

        public const int pu_healthMax = 4;
        public const int pu_tokenGunMax = 8;
        public const int pu_tokenHookMax = 3;
        public const int pu_tokenStraightMax = 6;
        public const int pu_tokenLeftMax = 5;
        public const int pu_tokenRightMax = 5;

        public CPlayer(int _deviceId)
        {
            mu_deviceId = _deviceId;

            pu_isReady = 0;
            pu_healthAmount = 4;
            pu_tokenGunAmount = 5;
            pu_tokenHookAmount = 2;
            pu_tokenStraightAmount = 4;
            pu_tokenLeftAmount = 3;
            pu_tokenRightAmount = 3;
            pu_movement1Mode = 0;
            pu_movement1LeftMode = 0;
            pu_movement1RightMode = 0;
            pu_movement2Mode = 0;
            pu_movement2LeftMode = 0;
            pu_movement2RightMode = 0;
            pu_movement3Mode = 0;
            pu_movement3LeftMode = 0;
            pu_movement3RightMode = 0;
            pu_movement4Mode = 0;
            pu_movement4LeftMode = 0;
            pu_movement4RightMode = 0;
            pu_gameState = 0;
        }

        public void fu_UpdateClient()
        {
            string information;

            // 1. Packe Alle Infos mit Pipes zusammen
            information = pu_isReady.ToString() + "|";
            information += pu_healthAmount.ToString() + "|";
            information += pu_tokenGunAmount.ToString() + "|";
            information += pu_tokenHookAmount.ToString() + "|";
            information += pu_tokenStraightAmount.ToString() + "|";
            information += pu_tokenLeftAmount.ToString() + "|";
            information += pu_tokenRightAmount.ToString() + "|";
            information += pu_movement1Mode.ToString() + "|";
            information += pu_movement1LeftMode.ToString() + "|";
            information += pu_movement1RightMode.ToString() + "|";
            information += pu_movement2Mode.ToString() + "|";
            information += pu_movement2LeftMode.ToString() + "|";
            information += pu_movement2RightMode.ToString() + "|";
            information += pu_movement3Mode.ToString() + "|";
            information += pu_movement3LeftMode.ToString() + "|";
            information += pu_movement3RightMode.ToString() + "|";
            information += pu_movement4Mode.ToString() + "|";
            information += pu_movement4LeftMode.ToString() + "|";
            information += pu_movement4RightMode.ToString() + "|";
            information += pu_gameState.ToString();

            // 2. Sende der device_id die Informationen zu
            AirConsole.instance.Message(mu_deviceId, information);
        }

        public void fu_UpdateStatus(string _statusMessage)
        {
            string[] msgs = _statusMessage.Split('|');

            int.TryParse(msgs[0], out pu_isReady);
            int.TryParse(msgs[1], out pu_healthAmount);
            int.TryParse(msgs[2], out pu_tokenGunAmount);
            int.TryParse(msgs[3], out pu_tokenHookAmount);
            int.TryParse(msgs[4], out pu_tokenStraightAmount);
            int.TryParse(msgs[5], out pu_tokenLeftAmount);
            int.TryParse(msgs[6], out pu_tokenRightAmount);
            int.TryParse(msgs[7], out pu_movement1Mode);
            int.TryParse(msgs[8], out pu_movement1LeftMode);
            int.TryParse(msgs[9], out pu_movement1RightMode);
            int.TryParse(msgs[10], out pu_movement2Mode);
            int.TryParse(msgs[11], out pu_movement2LeftMode);
            int.TryParse(msgs[12], out pu_movement2RightMode);
            int.TryParse(msgs[13], out pu_movement3Mode);
            int.TryParse(msgs[14], out pu_movement3LeftMode);
            int.TryParse(msgs[15], out pu_movement3RightMode);
            int.TryParse(msgs[16], out pu_movement4Mode);
            int.TryParse(msgs[17], out pu_movement4LeftMode);
            int.TryParse(msgs[18], out pu_movement4RightMode);
            int.TryParse(msgs[19], out pu_gameState);
        }
    }
}