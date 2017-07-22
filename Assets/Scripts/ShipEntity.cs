using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ocean
{
    public class CShipEntity : AOceanEntity
    {
        int mu_deviceId;

        public CShipEntity(int _x, int _y, EOrientation _orientation, int _deviceId) : base(_x, _y, _orientation)
        {
            mu_deviceId = _deviceId;
        }

        public override EOceanEntityType pu_EntityType
        {
            get
            {
                return EOceanEntityType.Ship;
            }
        }

        /// <summary>
        /// ship is moved because of streams and swirls
        /// </summary>
        public void fu_ProcessStreamsAndSwirls()
        {
            List<AOceanEntity> entities = CGameController.Get.mu_ocean.fu_GetListAt(pu_x, pu_y);

            int xOffset;
            int yOffset;
            foreach (var e in entities)
            {
                if (e.pu_EntityType == EOceanEntityType.SwirlClock)
                {
                    pu_orientation = (EOrientation)(((int)pu_orientation + 1) % (int)EOrientation.MAX_ORIENTATION);
                }

                fi_GetOffsetForOrientation(e.pu_orientation, out xOffset, out yOffset);
                pu_x += xOffset; // TODO: bounds check for play field
                pu_y += yOffset;
            }
        }

        /// <summary>
        /// ship moves 1 step according to its programming
        /// </summary>
        public void fu_ProcessMove(EGameStage _stage)
        {
            int xOffset;
            int yOffset;

            CPlayer player = CGameController.Get.mu_PlayerDict[mu_deviceId];
            EMoveCommand moveCommand = EMoveCommand.STAY;
            switch(_stage)
            {
                case EGameStage.MOVE_1:
                moveCommand = (EMoveCommand)player.pu_movement1Mode;
                break;
                case EGameStage.MOVE_2:
                moveCommand = (EMoveCommand)player.pu_movement2Mode;
                break;
                case EGameStage.MOVE_3:
                moveCommand = (EMoveCommand)player.pu_movement3Mode;
                break;
                case EGameStage.MOVE_4:
                moveCommand = (EMoveCommand)player.pu_movement4Mode;
                break;
            }

            switch (moveCommand)
            {
                case EMoveCommand.LEFT:
                    pu_orientation += (int)EOrientation.MAX_ORIENTATION - 1;
                break;
                case EMoveCommand.RIGHT:
                    pu_orientation += 1;
                break;
            }

            pu_orientation = (EOrientation)((int)pu_orientation % (int)EOrientation.MAX_ORIENTATION);

            fi_GetOffsetForOrientation(pu_orientation, out xOffset, out yOffset);

            List<AOceanEntity> entities = CGameController.Get.mu_ocean.fu_GetListAt(pu_x + xOffset, pu_y + yOffset);
            foreach(var e in entities)
            {
                if (e.pu_EntityType == EOceanEntityType.Ship)
                {
                    fi_BumpIntoShip((CShipEntity)e);
                    xOffset = 0;
                    yOffset = 0;
                }
                else if (e.pu_EntityType == EOceanEntityType.Rock)
                {
                    fi_BumpIntoRock();
                    xOffset = 0;
                    yOffset = 0;
                }
            }

            pu_x += xOffset; // TODO: bounds check for play field
            pu_y += yOffset;
        }

        /// <summary>
        /// try tossing a hook and perhaps pull some other ship closer
        /// </summary>
        public void fu_ProcessHooks(EGameStage _moveStage)
        {
            CPlayer player = CGameController.Get.mu_PlayerDict[mu_deviceId];
            bool portHook = false;
            bool starboardHook = false;
            switch (_moveStage)
            {
                case EGameStage.MOVE_1:
                {
                    portHook = player.pu_movement1LeftMode == 2;
                    starboardHook = player.pu_movement1RightMode == 2;
                }
                break;
                case EGameStage.MOVE_2:
                {
                    portHook = player.pu_movement2LeftMode == 2;
                    starboardHook = player.pu_movement2RightMode == 2;
                }
                break;
                case EGameStage.MOVE_3:
                {
                    portHook = player.pu_movement3LeftMode == 2;
                    starboardHook = player.pu_movement3RightMode == 2;
                }
                break;
                case EGameStage.MOVE_4:
                {
                    portHook = player.pu_movement4LeftMode == 2;
                    starboardHook = player.pu_movement4RightMode == 2;
                }
                break;
            }

            int xStep = 0;
            int yStep = 0;
            if (starboardHook)
            {
                EOrientation or = (EOrientation)(((int)pu_orientation + 1) % (int)EOrientation.MAX_ORIENTATION);
                fi_GetOffsetForOrientation(or, out xStep, out yStep);
                fi_TryHookEnemyShip(xStep, yStep, 2);
            }
            if (portHook)
            {
                EOrientation or = (EOrientation)(((int)pu_orientation + (int)EOrientation.MAX_ORIENTATION - 1) % (int)EOrientation.MAX_ORIENTATION);
                fi_GetOffsetForOrientation(or, out xStep, out yStep);
                fi_TryHookEnemyShip(xStep, yStep, 2);
            }
        }

        private void fi_TryHookEnemyShip(int _xStep, int _yStep, int _maxSteps)
        {
            for (int mul = 0; mul < _maxSteps; mul++)
            {
                List<AOceanEntity> list = CGameController.Get.mu_ocean.fu_GetListAt(pu_x + _xStep * mul, pu_y + _yStep * mul);
                if (list.Count > 0)
                {
                    if (list[0].pu_EntityType == EOceanEntityType.Ship)
                    {
                        list[0].pu_x -= _xStep;
                        list[0].pu_y -= _yStep;
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// do some damage to other ships?
        /// </summary>
        public void fu_ProcessCannons(EGameStage _moveStep)
        {
            CPlayer player = CGameController.Get.mu_PlayerDict[mu_deviceId];
            bool portCannon = false;
            bool starboardCannon = false;
            switch (_moveStep)
            {
                case EGameStage.MOVE_1:
                {
                    portCannon = player.pu_movement1LeftMode == 1;
                    starboardCannon = player.pu_movement1RightMode == 1;
                }
                break;
                case EGameStage.MOVE_2:
                {
                    portCannon = player.pu_movement2LeftMode == 1;
                    starboardCannon = player.pu_movement2RightMode == 1;
                }
                break;
                case EGameStage.MOVE_3:
                {
                    portCannon = player.pu_movement3LeftMode == 1;
                    starboardCannon = player.pu_movement3RightMode == 1;
                }
                break;
                case EGameStage.MOVE_4:
                {
                    portCannon = player.pu_movement4LeftMode == 1;
                    starboardCannon = player.pu_movement4RightMode == 1;
                }
                break;
            }

            int xStep = 0;
            int yStep = 0;
            if (starboardCannon)
            {
                EOrientation or = (EOrientation)(((int)pu_orientation + 1) % (int)EOrientation.MAX_ORIENTATION);
                fi_GetOffsetForOrientation(or, out xStep, out yStep);
                fi_TryShootEnemyShip(xStep, yStep, 3);
            }
            if (portCannon)
            {
                EOrientation or = (EOrientation)(((int)pu_orientation + (int)EOrientation.MAX_ORIENTATION - 1) % (int)EOrientation.MAX_ORIENTATION);
                fi_GetOffsetForOrientation(or, out xStep, out yStep);
                fi_TryShootEnemyShip(xStep, yStep, 3);
            }
        }

        public void fu_Kill()
        {

            throw new NotImplementedException();
        }

        public bool fu_KillCheck()
        {
            CPlayer p = CGameController.Get.mu_PlayerDict[mu_deviceId];
            return p.pu_healthAmount <= 0;
        }

        private void fi_TryShootEnemyShip(int _xStep, int _yStep, int _maxSteps)
        {
            for (int mul = 0; mul < _maxSteps; mul++)
            {
                List<AOceanEntity> list = CGameController.Get.mu_ocean.fu_GetListAt(pu_x + _xStep * mul, pu_y + _yStep * mul);
                if (list.Count > 0)
                {
                    if (list[0].pu_EntityType == EOceanEntityType.Ship)
                    {
                        CShipEntity otherShip = (CShipEntity)list[0];
                        CPlayer otherPlayer = CGameController.Get.mu_PlayerDict[otherShip.mu_deviceId];
                        otherPlayer.pu_healthAmount -= 1;
                    }
                    break;
                }
            }
        }

        private void fi_BumpIntoShip(CShipEntity _otherShip)
        {
            CPlayer player = CGameController.Get.mu_PlayerDict[mu_deviceId];
            CPlayer otherPlayer = CGameController.Get.mu_PlayerDict[_otherShip.mu_deviceId];
            player.pu_healthAmount -= 1;
            otherPlayer.pu_healthAmount -= 1;
        }

        private void fi_BumpIntoRock()
        {
            CPlayer player = CGameController.Get.mu_PlayerDict[mu_deviceId];
            player.pu_healthAmount -= 1;
        }

        private void fi_GetOffsetForOrientation(EOrientation _orientation, out int _xOffset, out int _yOffset)
        {
            _xOffset = 0;
            _yOffset = 0;

            switch (_orientation)
            {
                case EOrientation.North:
                {
                    _xOffset += 0;
                    _yOffset += 1;
                }
                break;
                case EOrientation.East:
                {
                    _xOffset += 1;
                    _yOffset += 0;
                }
                break;
                case EOrientation.South:
                {
                    _xOffset += 0;
                    _yOffset += -1;
                }
                break;
                case EOrientation.West:
                {
                    _xOffset += -1;
                    _yOffset += 0;
                }
                break;

                case EOrientation.MAX_ORIENTATION: // for center of swirl
                break;
            }
        }
    }
}