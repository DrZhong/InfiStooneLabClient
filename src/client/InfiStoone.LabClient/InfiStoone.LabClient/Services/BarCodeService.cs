using InfiStoone.LabClient.Tools;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiStoone.LabClient.Services
{
    public class BarCodeService
    {
        public static event Action<string> OnDataReceived;

        SerialPort sp;
        public void Init()
        {
            string com = Config.ConfigModel.BarCodePort;
            if (!string.IsNullOrEmpty(com))
            {
                sp = new SerialPort(com, 115200, Parity.None, 8, StopBits.One);
                sp.DataReceived += Sp_DataReceived;
                try
                {
                    sp.Open();
                }
                catch (Exception)
                {

                }
            }
        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            OnDataReceived?.Invoke(sp.ReadExisting());
        }
    }
}
