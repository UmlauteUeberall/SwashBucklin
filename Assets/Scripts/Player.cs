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

            fu_UpdateClient();
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
            if (AirConsole.instance)
            {
                AirConsole.instance.Message(mu_deviceId, information);
            }
        }

        private void HandleClientMessage(int _pressedButton, int _buttonParameter)
        {
            switch (_pressedButton)
            {
                case 0:
                    // Ready Button
                    if ( pu_isReady == 0 ) { pu_isReady = 1; } else { pu_isReady = 0; }
                    break;
                case 1:
                    // Movement First
                    // 1. Check if same Mode as current, so we can reset
                    if(_buttonParameter == pu_movement1Mode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch(pu_movement1Mode)
                    {
                        case 1:
                            pu_tokenStraightAmount += 1;
                            break;
                        case 2:
                            pu_tokenLeftAmount += 1;
                            break;
                        case 3:
                            pu_tokenRightAmount += 1;
                            break;
                    }

                    pu_movement1Mode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch(_buttonParameter)
                    {
                        case 1:
                            if(pu_tokenStraightAmount > 0)
                            {
                                pu_tokenStraightAmount -= 1;
                                pu_movement1Mode = 1;
                            }
                            break;
                        case 2:
                            if (pu_tokenLeftAmount > 0)
                            {
                                pu_tokenLeftAmount -= 1;
                                pu_movement1Mode = 2;
                            }
                            break;
                        case 3:
                            if (pu_tokenRightAmount > 0)
                            {
                                pu_tokenRightAmount -= 1;
                                pu_movement1Mode = 3;
                            }
                            break;
                    }
                    break;
                case 2:
                    // Movement First Gun Left
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement1LeftMode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement1LeftMode)
                    {
                        case 1:
                            pu_tokenHookAmount += 1;
                            break;
                        case 2:
                            pu_tokenGunAmount += 1;
                            break;
                    }

                    pu_movement1LeftMode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenHookAmount > 0)
                            {
                                pu_tokenHookAmount -= 1;
                                pu_movement1LeftMode = 1;
                            }
                            else
                            {
                                if(pu_tokenGunAmount > 0)
                                {
                                    pu_tokenGunAmount -= 1;
                                    pu_movement1LeftMode = 2;
                                }
                            }
                            break;
                        case 2:
                            if (pu_tokenGunAmount > 0)
                            {
                                pu_tokenGunAmount -= 1;
                                pu_movement1LeftMode = 2;
                            }
                            else
                            {
                                if (pu_tokenHookAmount > 0)
                                {
                                    pu_tokenHookAmount -= 1;
                                    pu_movement1LeftMode = 1;
                                }
                                else
                                {
                                    pu_movement1LeftMode = 0;
                                }
                            }
                            break;
                    }

                    break;
                case 3:
                    // Movement First Gun Right
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement1RightMode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement1RightMode)
                    {
                        case 1:
                            pu_tokenHookAmount += 1;
                            break;
                        case 2:
                            pu_tokenGunAmount += 1;
                            break;
                    }

                    pu_movement1RightMode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenHookAmount > 0)
                            {
                                pu_tokenHookAmount -= 1;
                                pu_movement1RightMode = 1;
                            }
                            else
                            {
                                if (pu_tokenGunAmount > 0)
                                {
                                    pu_tokenGunAmount -= 1;
                                    pu_movement1RightMode = 2;
                                }
                            }
                            break;
                        case 2:
                            if (pu_tokenGunAmount > 0)
                            {
                                pu_tokenGunAmount -= 1;
                                pu_movement1RightMode = 2;
                            }
                            else
                            {
                                if (pu_tokenHookAmount > 0)
                                {
                                    pu_tokenHookAmount -= 1;
                                    pu_movement1RightMode = 1;
                                }
                                else
                                {
                                    pu_movement1RightMode = 0;
                                }
                            }
                            break;
                    }
                    break;
                case 4:
                    // Movement Second
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement2Mode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement2Mode)
                    {
                        case 1:
                            pu_tokenStraightAmount += 1;
                            break;
                        case 2:
                            pu_tokenLeftAmount += 1;
                            break;
                        case 3:
                            pu_tokenRightAmount += 1;
                            break;
                    }

                    pu_movement2Mode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenStraightAmount > 0)
                            {
                                pu_tokenStraightAmount -= 1;
                                pu_movement2Mode = 1;
                            }
                            break;
                        case 2:
                            if (pu_tokenLeftAmount > 0)
                            {
                                pu_tokenLeftAmount -= 1;
                                pu_movement2Mode = 2;
                            }
                            break;
                        case 3:
                            if (pu_tokenRightAmount > 0)
                            {
                                pu_tokenRightAmount -= 1;
                                pu_movement2Mode = 3;
                            }
                            break;
                    }
                    break;
                case 5:
                    // Movement Second Gun Left
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement2LeftMode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement2LeftMode)
                    {
                        case 1:
                            pu_tokenHookAmount += 1;
                            break;
                        case 2:
                            pu_tokenGunAmount += 1;
                            break;
                    }

                    pu_movement2LeftMode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenHookAmount > 0)
                            {
                                pu_tokenHookAmount -= 1;
                                pu_movement2LeftMode = 1;
                            }
                            else
                            {
                                if (pu_tokenGunAmount > 0)
                                {
                                    pu_tokenGunAmount -= 1;
                                    pu_movement2LeftMode = 2;
                                }
                            }
                            break;
                        case 2:
                            if (pu_tokenGunAmount > 0)
                            {
                                pu_tokenGunAmount -= 1;
                                pu_movement2LeftMode = 2;
                            }
                            else
                            {
                                if (pu_tokenHookAmount > 0)
                                {
                                    pu_tokenHookAmount -= 1;
                                    pu_movement2LeftMode = 1;
                                }
                                else
                                {
                                    pu_movement2LeftMode = 0;
                                }
                            }
                            break;
                    }
                    break;
                case 6:
                    // Movement Second Gun Right
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement2RightMode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement2RightMode)
                    {
                        case 1:
                            pu_tokenHookAmount += 1;
                            break;
                        case 2:
                            pu_tokenGunAmount += 1;
                            break;
                    }

                    pu_movement2RightMode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenHookAmount > 0)
                            {
                                pu_tokenHookAmount -= 1;
                                pu_movement2RightMode = 1;
                            }
                            else
                            {
                                if (pu_tokenGunAmount > 0)
                                {
                                    pu_tokenGunAmount -= 1;
                                    pu_movement2RightMode = 2;
                                }
                            }
                            break;
                        case 2:
                            if (pu_tokenGunAmount > 0)
                            {
                                pu_tokenGunAmount -= 1;
                                pu_movement2RightMode = 2;
                            }
                            else
                            {
                                if (pu_tokenHookAmount > 0)
                                {
                                    pu_tokenHookAmount -= 1;
                                    pu_movement2RightMode = 1;
                                }
                                else
                                {
                                    pu_movement2RightMode = 0;
                                }
                            }
                            break;
                    }
                    break;
                case 7:
                    // Movement Third
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement3Mode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement3Mode)
                    {
                        case 1:
                            pu_tokenStraightAmount += 1;
                            break;
                        case 2:
                            pu_tokenLeftAmount += 1;
                            break;
                        case 3:
                            pu_tokenRightAmount += 1;
                            break;
                    }

                    pu_movement3Mode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenStraightAmount > 0)
                            {
                                pu_tokenStraightAmount -= 1;
                                pu_movement3Mode = 1;
                            }
                            break;
                        case 2:
                            if (pu_tokenLeftAmount > 0)
                            {
                                pu_tokenLeftAmount -= 1;
                                pu_movement3Mode = 2;
                            }
                            break;
                        case 3:
                            if (pu_tokenRightAmount > 0)
                            {
                                pu_tokenRightAmount -= 1;
                                pu_movement3Mode = 3;
                            }
                            break;
                    }
                    break;
                case 8:
                    // Movement Third Gun Left
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement3LeftMode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement3LeftMode)
                    {
                        case 1:
                            pu_tokenHookAmount += 1;
                            break;
                        case 2:
                            pu_tokenGunAmount += 1;
                            break;
                    }

                    pu_movement3LeftMode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenHookAmount > 0)
                            {
                                pu_tokenHookAmount -= 1;
                                pu_movement3LeftMode = 1;
                            }
                            else
                            {
                                if (pu_tokenGunAmount > 0)
                                {
                                    pu_tokenGunAmount -= 1;
                                    pu_movement3LeftMode = 2;
                                }
                            }
                            break;
                        case 2:
                            if (pu_tokenGunAmount > 0)
                            {
                                pu_tokenGunAmount -= 1;
                                pu_movement3LeftMode = 2;
                            }
                            else
                            {
                                if (pu_tokenHookAmount > 0)
                                {
                                    pu_tokenHookAmount -= 1;
                                    pu_movement3LeftMode = 1;
                                }
                                else
                                {
                                    pu_movement3LeftMode = 0;
                                }
                            }
                            break;
                    }
                    break;
                case 9:
                    // Movement Third Gun Right
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement3RightMode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement3RightMode)
                    {
                        case 1:
                            pu_tokenHookAmount += 1;
                            break;
                        case 2:
                            pu_tokenGunAmount += 1;
                            break;
                    }

                    pu_movement3RightMode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenHookAmount > 0)
                            {
                                pu_tokenHookAmount -= 1;
                                pu_movement3RightMode = 1;
                            }
                            else
                            {
                                if (pu_tokenGunAmount > 0)
                                {
                                    pu_tokenGunAmount -= 1;
                                    pu_movement3RightMode = 2;
                                }
                            }
                            break;
                        case 2:
                            if (pu_tokenGunAmount > 0)
                            {
                                pu_tokenGunAmount -= 1;
                                pu_movement3RightMode = 2;
                            }
                            else
                            {
                                if (pu_tokenHookAmount > 0)
                                {
                                    pu_tokenHookAmount -= 1;
                                    pu_movement3RightMode = 1;
                                }
                                else
                                {
                                    pu_movement3RightMode = 0;
                                }
                            }
                            break;
                    }
                    break;
                case 10:
                    // Movement Fourth
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement4Mode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement4Mode)
                    {
                        case 1:
                            pu_tokenStraightAmount += 1;
                            break;
                        case 2:
                            pu_tokenLeftAmount += 1;
                            break;
                        case 3:
                            pu_tokenRightAmount += 1;
                            break;
                    }

                    pu_movement4Mode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenStraightAmount > 0)
                            {
                                pu_tokenStraightAmount -= 1;
                                pu_movement4Mode = 1;
                            }
                            break;
                        case 2:
                            if (pu_tokenLeftAmount > 0)
                            {
                                pu_tokenLeftAmount -= 1;
                                pu_movement4Mode = 2;
                            }
                            break;
                        case 3:
                            if (pu_tokenRightAmount > 0)
                            {
                                pu_tokenRightAmount -= 1;
                                pu_movement4Mode = 3;
                            }
                            break;
                    }
                    break;
                case 11:
                    // Movement Fourth Gun Left
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement4LeftMode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement4LeftMode)
                    {
                        case 1:
                            pu_tokenHookAmount += 1;
                            break;
                        case 2:
                            pu_tokenGunAmount += 1;
                            break;
                    }

                    pu_movement4LeftMode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenHookAmount > 0)
                            {
                                pu_tokenHookAmount -= 1;
                                pu_movement4LeftMode = 1;
                            }
                            else
                            {
                                if (pu_tokenGunAmount > 0)
                                {
                                    pu_tokenGunAmount -= 1;
                                    pu_movement4LeftMode = 2;
                                }
                            }
                            break;
                        case 2:
                            if (pu_tokenGunAmount > 0)
                            {
                                pu_tokenGunAmount -= 1;
                                pu_movement4LeftMode = 2;
                            }
                            else
                            {
                                if (pu_tokenHookAmount > 0)
                                {
                                    pu_tokenHookAmount -= 1;
                                    pu_movement4LeftMode = 1;
                                }
                                else
                                {
                                    pu_movement4LeftMode = 0;
                                }
                            }
                            break;
                    }
                    break;
                case 12:
                    // Movement Fourth Gun Right
                    // 1. Check if same Mode as current, so we can reset
                    if (_buttonParameter == pu_movement4RightMode)
                    {
                        _buttonParameter = 0;
                    }

                    // 2. Reset Value to Empty, so we can get our Tokens back easily!
                    switch (pu_movement4RightMode)
                    {
                        case 1:
                            pu_tokenHookAmount += 1;
                            break;
                        case 2:
                            pu_tokenGunAmount += 1;
                            break;
                    }

                    pu_movement4RightMode = 0;

                    // 3. Set the Movement Mode to the correct one!
                    switch (_buttonParameter)
                    {
                        case 1:
                            if (pu_tokenHookAmount > 0)
                            {
                                pu_tokenHookAmount -= 1;
                                pu_movement4RightMode = 1;
                            }
                            else
                            {
                                if (pu_tokenGunAmount > 0)
                                {
                                    pu_tokenGunAmount -= 1;
                                    pu_movement4RightMode = 2;
                                }
                            }
                            break;
                        case 2:
                            if (pu_tokenGunAmount > 0)
                            {
                                pu_tokenGunAmount -= 1;
                                pu_movement4RightMode = 2;
                            }
                            else
                            {
                                if (pu_tokenHookAmount > 0)
                                {
                                    pu_tokenHookAmount -= 1;
                                    pu_movement4RightMode = 1;
                                }
                                else
                                {
                                    pu_movement4RightMode = 0;
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        public void fu_UpdateStatus(string _statusMessage)
        {
            int PressedButton = 0;
            int ButtonParameter = 0;

            string[] msgs = _statusMessage.Split('|');

            PressedButton = int.Parse(msgs[0]);
            ButtonParameter = int.Parse(msgs[1]);

            plib.Util.L.Log(PressedButton + " " + ButtonParameter);

            // Handle what comes from the Client!
            HandleClientMessage(PressedButton, ButtonParameter);

            // Update the real values to the clients GUI!
            fu_UpdateClient();
        }
    }
}