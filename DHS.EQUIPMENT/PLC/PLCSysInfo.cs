using DHS.EQUIPMENT.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace DHS.EQUIPMENT.PLC
{
    public class PLCSysInfo
    {
        private static PLCSysInfo plcsysinfo = null;
        public static PLCSysInfo GetInstance()
        {
            if (plcsysinfo == null) plcsysinfo = new PLCSysInfo();
            return plcsysinfo;
        }

        #region PLC SYS INFO 변수
        private string _sInterfaceVersionProject;
        private string _sEquipmentName;
        private int _iEquipmentTypeID;
        private int _iLineID;
        private int _iAreaID;
        private int _iVendorID;
        private int _iEquipmentID;
        private int _iState;
        private int _iMode;
        private bool _iBlocked;
        private bool _iStarved;
        private double _iCurrentSpeed;
        private double _iDesignSpeed;
        private int _iTotalCounter;
        private int _iStandstillReason;
        private int _iStacklight0Color;
        private int _iStacklight0Behavior;
        private int _iStacklight1Color;
        private int _iStacklight1Behavior;
        private int _iStacklight2Color;
        private int _iStacklight2Behavior;
        private int _iStacklight3Color;
        private int _iStacklight3Behavior;
        private int _iStacklight4Color;
        private int _iStacklight4Behavior;
        private int _iStacklight5Color;
        private int _iStacklight5Behavior;

        public string INTERFACEVERSIONPROJECT { get => _sInterfaceVersionProject; set => _sInterfaceVersionProject = value; }
        public string EQUIPMENTNAME { get => _sEquipmentName; set => _sEquipmentName = value; }
        public int EQUIPMENTTYPEID { get => _iEquipmentTypeID; set => _iEquipmentTypeID = value; }
        public int LINEID { get => _iLineID; set => _iLineID = value; }
        public int AREAID { get => _iAreaID; set => _iAreaID = value; }
        public int EQUIPMENTID { get => _iEquipmentID; set => _iEquipmentID = value; }
        public int STATE { get => _iState; set => _iState = value; }
        public int MODE { get => _iMode; set => _iMode = value; }
        public bool BLOCKED { get => _iBlocked; set => _iBlocked = value; }
        public bool STARVED { get => _iStarved; set => _iStarved = value; }
        public double CURRENTSPEED { get => _iCurrentSpeed; set => _iCurrentSpeed = value; }
        public double DESIGNSPEED { get => _iDesignSpeed; set => _iDesignSpeed = value; }
        public int TOTALCOUNTER { get => _iTotalCounter; set => _iTotalCounter = value; }
        public int STANDSTILLREASON { get => _iStandstillReason; set => _iStandstillReason = value; }
        public int STACKLIGHT0COLOR { get => _iStacklight0Color; set => _iStacklight0Color = value; }
        public int STACKLIGHT0BEHAVIOR { get => _iStacklight0Behavior; set => _iStacklight0Behavior = value; }
        public int STACKLIGHT1COLOR { get => _iStacklight1Color; set => _iStacklight1Color = value; }
        public int STACKLIGHT1BEHAVIOR { get => _iStacklight1Behavior; set => _iStacklight1Behavior = value; }
        public int STACKLIGHT2COLOR { get => _iStacklight2Color; set => _iStacklight2Color = value; }
        public int STACKLIGHT2BEHAVIOR { get => _iStacklight2Behavior; set => _iStacklight2Behavior = value; }
        public int STACKLIGHT3COLOR { get => _iStacklight3Color; set => _iStacklight3Color = value; }
        public int STACKLIGHT3BEHAVIOR { get => _iStacklight3Behavior; set => _iStacklight3Behavior = value; }
        public int VENDORID { get => _iVendorID; set => _iVendorID = value; }
        public int STACKLIGHT4COLOR { get => _iStacklight4Color; set => _iStacklight4Color = value; }
        public int STACKLIGHT4BEHAVIOR { get => _iStacklight4Behavior; set => _iStacklight4Behavior = value; }
        public int STACKLIGHT5COLOR { get => _iStacklight5Color; set => _iStacklight5Color = value; }
        public int STACKLIGHT5BEHAVIOR { get => _iStacklight5Behavior; set => _iStacklight5Behavior = value; }
        #endregion

        public PLCSysInfo()
        {
            
        }
    }
}
