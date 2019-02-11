﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Stylet;
using StyletIoC;
using DAQ.Service;

namespace DAQ
{
    public class SettingsViewModel : Screen
    {
        [Inject]
        public IEventAggregator Events { get; set; }
        [Inject]
        public PortAService PortServiceA { get; set; }
        [Inject]
        public PortBService PortServiceB { get; set; }

        public string[] Ports { get { return SerialPort.GetPortNames(); } }

        public string[] PortACMDs { get { return new string[] { "*IDN?", "TRIG?","SCAN:DATA?","READ?","FETCH?" }; } }
        public string[] PortBCMDs { get { return new string[] { "*IDN?", "TRIG?","FETCh?"}; } }

        public SettingsViewModel()
        {
        }
        protected override void OnInitialActivate()
        {

            base.OnInitialActivate();
            Task.Run(() =>
            {
                if (!PortServiceA.IsConnected)
                    PortServiceA.Connect();
                if (!PortServiceB.IsConnected)
                    PortServiceB.Connect();
            });

        }
        public string PortA
        {
            get { return Properties.Settings.Default.PORT_A; }
            set
            {
                Properties.Settings.Default.PORT_A = value;
                PortServiceA.Connect();
            }
        }
        public string PortB
        {
            get { return Properties.Settings.Default.PORT_B; }
            set
            {
                Properties.Settings.Default.PORT_B = value;
                PortServiceB.Connect();
            }
        }
        public string PLC_IP
        {
            get { return Properties.Settings.Default.PLC_IP; }
            set { Properties.Settings.Default.PLC_IP = value; }
        }

        public string PortABuffer { get; set; }
        public string PortBBuffer { get; set; }
        public int PLC_Port
        {
            get { return Properties.Settings.Default.PLC_PORT; }
            set { Properties.Settings.Default.PLC_PORT = value; }
        }

        public void QueryA(string Cmd)
        {
            PortABuffer = $"Send:\t{Cmd}{Environment.NewLine}";
            bool r = PortServiceA.Request(Cmd, out string replay);
            if (r)
            {
                PortABuffer += $"Recieve:\t{replay}{Environment.NewLine}";
            }
            else
            {
                PortABuffer += $"error:\t{replay}{Environment.NewLine}";
            }
        }
        public void QueryB(string Cmd)
        {
            PortBBuffer = $"Send:\t{Cmd}{Environment.NewLine}";
            bool r = PortServiceB.Request(Cmd, out string replay);
            if (r)
            { 
                PortBBuffer += $"Recieve:\t{replay}{Environment.NewLine}";
            }
            else
            {
                PortBBuffer += $"error:\t{replay}{Environment.NewLine}";
            }
        }

        public string CameraIP
        {
            get { return Properties.Settings.Default.CAMERA_IP; }
            set { Properties.Settings.Default.CAMERA_IP = value; }
        }
        protected override void OnDeactivate()
        {
            Properties.Settings.Default.Save();
            base.OnDeactivate();
        }
        public int CameraPort
        {
            get { return Properties.Settings.Default.CAMERA_PORT; }
            set { Properties.Settings.Default.CAMERA_PORT = value; }
        }
       


    }
}
