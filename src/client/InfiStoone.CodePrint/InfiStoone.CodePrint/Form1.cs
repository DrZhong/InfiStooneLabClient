using Fleck;
using InfiStoone.CodePrint.Dto;
using System; 
using System.Drawing; 
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.IO;
using BarcodeLib;
using System.Runtime.InteropServices;

namespace InfiStoone.CodePrint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //是否取消close操作
                e.Cancel = true;
                this.NormalToMinimized();//最小化时窗体隐藏
            }
        }
        private void NormalToMinimized()
        {
            this.notifyIcon1.Visible = true;
            this.Hide(); 
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            var server = new WebSocketServer("ws://127.0.0.1:18181")
            {
                RestartAfterListenError = true
            };
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    listBox1.Items.Add("socket opend!");
                };
                socket.OnClose = () => listBox1.Items.Add("socket closed"); ;
                socket.OnMessage = message =>
                {
                    RequestData obj = JsonConvert.DeserializeObject<RequestData>(message);
                    if (obj == null)
                    {
                        socket.Send(JsonConvert.SerializeObject(new ResultData<string>
                        {
                            success = false,
                            error = "数据解析失败"
                        }));
                        return;
                    }

                    if (string.IsNullOrEmpty(obj.data))
                    {
                        socket.Send(JsonConvert.SerializeObject(new ResultData<string>
                        {
                            success = false,
                            error = "未提供data数据"
                        }));
                        return;
                    }

                    try
                    {
                        Printer(obj.data);
                        socket.Send(JsonConvert.SerializeObject(new ResultData<string>
                        {
                            success = true,
                            data = "success"
                        }));
                    }
                    catch (Exception exception)
                    {
                        socket.Send(JsonConvert.SerializeObject(new ResultData<string>
                        {
                            success = true,
                            error = exception.Message
                        }));
                    } 
                };
            });
        }
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int Index);
        public static double MillimetersToPixelsWidth(double length) //length是毫米，1厘米=10毫米
        {
            System.Windows.Forms.Panel p = new System.Windows.Forms.Panel();
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(p.Handle);
            IntPtr hdc = g.GetHdc();
            int width = GetDeviceCaps(hdc, 4);     // HORZRES 
            int pixels = GetDeviceCaps(hdc, 8);     // BITSPIXEL
            g.ReleaseHdc(hdc);
            return (((double)pixels / (double)width) * (double)length);
        }
        /// <summary>
        /// 转换毫米到百分之一英寸
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        private int MM2Inch(int mm)
        {
            return (int)(mm * 100.0f / 25.4f);
        }
        /// <summary>
        /// 真正的打印方法
        /// </summary>
        /// <param name="msg"></param>
        void Printer(string msg)
        {
            PrintDocument pd = new PrintDocument();
            var str = File.ReadAllText("json.json");
            var json = JsonConvert.DeserializeObject<SettingDto>(str);
            Brush brush = new SolidBrush(Color.Black);//画刷    
            Font fntTxt = new Font("宋体", 12, FontStyle.Regular);//正文文字
            pd.PrintPage += (s, e) =>
            {
                //var px = Convert.ToInt32(MillimetersToPixelsWidth(json.qrCodeWidth));

                var width = json.BarCodeWidth;
                var height = json.BarCodeHight;
                // var px2 = MM2Inch(json.qrCodeWidth);
                var bitmap = CreateQRCode(msg,width,height);

                e.Graphics.DrawImage(bitmap, new Point(0,json.Margin));
                //var no = msg.Substring(msg.LastIndexOf('$') + 2, 1) + "-" + msg.Substring(msg.Length - 6);
                e.Graphics.DrawString(msg, fntTxt, brush, 20, height+5+json.Margin);
            };
            PrinterSettings printSetting = new PrinterSettings
            {
                PrintRange = PrintRange.AllPages
            };


            int width_in =  MM2Inch(json.PageWidth); //画布大小
            int height_in =  MM2Inch(json.PageHeight); //画布大小

            int margin = MM2Inch(0); 
            pd.DefaultPageSettings.PrinterSettings.PrinterName = json.printerName;// //"Deli DL-886AW";//打印机大小
            pd.DefaultPageSettings.PaperSize = new PaperSize("customer", width_in, height_in); //= pageSetting;
            pd.DefaultPageSettings.Margins = new Margins(margin, margin, margin, margin);

            pd.PrintController = new System.Drawing.Printing.StandardPrintController();
            pd.Print();
        }


        Image CreateQRCode(string msg,int width= 290, int height= 120)
        {
            var b = new Barcode(); 
             return b.Encode( TYPE.CODE39, msg, Color.Black, Color.White, width, height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Printer("15601580721");
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void 显示窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("确定退出吗？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {

                Application.ExitThread();
            }
        }
    }
}
