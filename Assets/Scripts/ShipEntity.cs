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
        public void fu_ProcessMove(EMoveCommand _moveCommand)
        {

            int xOffset;
            int yOffset;

            switch(_moveCommand)
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

        public void fu_ProcessHook(bool _starBoard)
        {

        }

        public void fu_ProcessFireCannon(bool _starBoard)
        {

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

        /// <summary>
        /// try tossing a hook and perhaps pull some other ship closer
        /// </summary>
        public void fu_ProcessHooks()
        {

        }

        /// <summary>
        /// do some damage to other ships?
        /// </summary>
        public void fu_ProcessCannons()
        {

        }
    }
}