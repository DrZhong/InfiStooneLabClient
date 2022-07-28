using InfiStoone.LabClient.Runtime;
using InfiStoone.LabClient.Runtime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using zkfvcsharp;

namespace InfiStoone.LabClient.Services
{
    public class FingerService
    {
        public static event Action<bool> OnDeviceConnected;

        public static event Action<bool> OnDeviceOpened;

        public static event Action<FingerService> OnCaptured;


        public bool IsDevConnected { get; set; }

        public bool IsDevOpend { get; set; }

        public void Init()
        {
            mRegFVTempLen = new int[mEnrollCnt];
            mPreRegFPTempLen = new int[mEnrollCnt];
            mPreRegFVTempLen = new int[mEnrollCnt];
            mfpTemp = new byte[mMaxFpTempLen];
            mfvTemp = new byte[mMaxTempLen];
            mRegFPRegTemp = new byte[mMaxFpTempLen];
            mRegFPRegTempLen = 0;
            for (int i = 0; i < mEnrollCnt; i++)
            {
                mRegFVRegTemps[i] = new byte[mMaxTempLen];
                mPreRegFPRegTemps[i] = new byte[mMaxFpTempLen];
                mPreRegFVRegTemps[i] = new byte[mMaxTempLen];
                mRegFVTempLen[i] = 0;
                mPreRegFPTempLen[i] = 0;
                mPreRegFVTempLen[i] = 0;
            }

            Task.Factory.StartNew(() => {
                while (true)
                {
                    int ret = zkfverrdef.ZKFV_ERR_OK;
                    if (zkfverrdef.ZKFV_ERR_OK != (ret = zkfingervein.Init()))
                    {
                        DeviceConnectedInvoke(false);
                        Thread.Sleep(1000);
                        continue;
                    }
                    int nCount = zkfingervein.GetDeviceCount();
                    if (nCount <= 0)
                    {
                        DeviceConnectedInvoke(false);
                        Thread.Sleep(1000);
                        zkfv.Terminate();
                        continue;
                    }
                    this.IsDevConnected = true;
                    DeviceConnectedInvoke(true);
                    Open();
                    break;
                }
            }); 
        }

        public void Reset()
        {
            zkfingervein.DBFree(this.mDBHandle);
        }

        public void Close()
        {
            if (this.IsDevOpend)
            {
                zkfingervein.CloseDevice(mDevHandle);
                zkfingervein.Terminate();
            }
        }


        //Device Handle
        private IntPtr mDevHandle = IntPtr.Zero;
        //DB Handle
        private IntPtr mDBHandle = IntPtr.Zero;
        //Last reg-fingerprint template
        private byte[] mRegFPRegTemp = null;
        //Last reg-fingerprint template length
        private int mRegFPRegTempLen = 0;
        //Last reg-fingervein templates(3 fingervein temlates)
        private byte[][] mRegFVRegTemps = new byte[3][];
        //the length of reg-fingervein template
        private int[] mRegFVTempLen = null;
        //stop thread
        private bool mbStop = true;
        //Enroll
        private bool mbRegister = false;
        //Identify(!Verify)
        private bool mbIdentify = false;
        //Enroll count
        private static int mEnrollCnt = 3;
        //Enroll index
        public int mEnrollIdx = 0;
        //register finger id(must > 0)
        private int mFingerID = 1;
        //Max template length
        private static int mMaxTempLen = 1024;

        private static int mMaxFpTempLen = 2048;

        //From handle
        IntPtr FormHandle = IntPtr.Zero;

        //Acquire info form sdk begin
        public byte[] mfpImg = null;
        public byte[] mfvImg = null;
        public int mcbFpImg = 0;
        public int mcbFvImg = 0;
        public int mfpWidth = 0;
        public int mfpHeight = 0;
        public int mfvWidth = 0;
        public int mfvHeight = 0;
        public byte[] mfpTemp = null;
        public byte[] mfvTemp = null;
        public int mfpTempLen = 0;
        public int mfvTempLen = 0;
        //prereg-fingerprint templates
        private byte[][] mPreRegFPRegTemps = new byte[3][];
        //the length of reg-fingervein template
        private int[] mPreRegFPTempLen = null;
        //Last reg-fingervein templates(3 fingervein temlates)
        private byte[][] mPreRegFVRegTemps = new byte[3][];
        //the length of reg-fingervein template
        private int[] mPreRegFVTempLen = null;
        //Acquire info form sdk end


        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;
        public void Open()
        {

            if (IntPtr.Zero != mDevHandle)
            {
                throw new UserFriendException("请先关闭设备后再打开设备"); 
            }

            mDevHandle = zkfingervein.OpenDevice(0);
            if (IntPtr.Zero == mDevHandle)
            {
                throw new UserFriendException("打开设备失败"); 
            }
            mDBHandle = zkfingervein.DBInit(mDevHandle);
            if (IntPtr.Zero == mDevHandle)
            { 
                zkfv.CloseDevice(mDevHandle);
                mDevHandle = IntPtr.Zero;
                throw new UserFriendException("打开设备失败"); ;
            }
            //bnOpen.Enabled = false;
            //bnClose.Enabled = true;
            //bnEnroll.Enabled = true;
            //bnCount.Enabled = true;
            //bnVerify.Enabled = true;
            //bnIdentify.Enabled = true;

            byte[] retParam = new byte[4];
            int retSize = 4;
            int ret = 0;


            byte[] nFullWidth = new byte[4];
            byte[] nFullHeight = new byte[4];
            zkfingervein.GetParameters(mDevHandle, 1, nFullWidth, ref retSize);
            retSize = 4;
            zkfingervein.GetParameters(mDevHandle, 2, nFullHeight, ref retSize);

            int nFullWidth1 = System.BitConverter.ToInt32(nFullWidth, 0);
            int nFullHeight1 = System.BitConverter.ToInt32(nFullHeight, 0);

            mfpWidth = nFullWidth1 & 0xFFFF;
            mfpHeight = nFullHeight1 & 0xFFFF;
            mfvWidth = (nFullWidth1 >> 16) & 0xFFFF;
            mfvHeight = (nFullHeight1 >> 16) & 0xFFFF;

             

            mfvImg = new byte[mfvWidth * mfvHeight];
            mfpImg = new byte[mfpWidth * mfpHeight];
            mcbFpImg = mfpWidth * mfpHeight;
            mcbFvImg = mfvWidth * mfvHeight;

            
            Thread captureThread = new Thread(new ThreadStart(DoCapture));
            captureThread.IsBackground = true;
            captureThread.Start();
            this.IsDevOpend = true;
            DeviceOpendInvoke(true);
            //txtResult.Text = "Open succ!";
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <returns></returns>
        public int  DBHybridIdentify()
        {
            int nFingerId = 0;
            int nScore = 0;
            //zkfv.DBSecurityHybridIdentify
            //zkfv.DBFakeHybridIdentify
            int ret = zkfv.DBIdentifyFV(mDBHandle, mfvTemp, ref nFingerId, ref nScore);
            if (zkfverrdef.ZKFV_ERR_OK == ret && nFingerId>0)
            {
                return nFingerId;
            }
            ret = zkfv.DBIdentifyFP(mDBHandle, mfpTemp, ref nFingerId, ref nScore);
            if (zkfverrdef.ZKFV_ERR_OK == ret && nFingerId > 0)
            {
                return nFingerId;
            }

            byte[] uID = new byte[4];
            ret = zkfingervein.DBHybridIdentify(mDBHandle, 0, mfpTemp, mfpTempLen, mfvTemp, mfvTempLen, uID, ref nScore);

            //ret = zkfv.DBNormalHybridIdentify(mDBHandle, mfpTemp, mfvTemp, ref nFingerId, ref nScore);
            if (zkfverrdef.ZKFV_ERR_OK != ret)
            {
                return -1;
            }
            else
            {
                return System.BitConverter.ToInt32(uID, 0);
                //System.Text.Encoding.Default.GetString(byteArray);
                //txtResult.Text = "Identify succ, fingerid = " + System.BitConverter.ToInt32(uID, 0) + ", score = " + nScore;
            }
        }

        public bool Verify()
        {
            if (mEnrollIdx > 0)
            {
                int nR1 = zkfingervein.Verify(mDevHandle, 0, mfpTemp, mfpTempLen, mPreRegFPRegTemps[mEnrollIdx - 1], mPreRegFPTempLen[mEnrollIdx - 1]);
                int nR2 = zkfingervein.Verify(mDevHandle, 1, mfvTemp, mfvTempLen, mPreRegFVRegTemps[mEnrollIdx - 1], mPreRegFVTempLen[mEnrollIdx - 1]);

                if (nR1 <= 0 || nR2 <= 0)
                {
                    //txtResult.Text = "Please press the same finger while registering!";
                    return false;
                }
            }
         
            if (mfpTempLen > 0)
            {
                Array.Copy(mfpTemp, mPreRegFPRegTemps[mEnrollIdx], mfpTempLen);
            }
            if (mfpTempLen > 0)
            {
                Array.Copy(mfvTemp, mPreRegFVRegTemps[mEnrollIdx], mfvTempLen);
            }
            mPreRegFPTempLen[mEnrollIdx] = mfpTempLen;
            mPreRegFVTempLen[mEnrollIdx] = mfvTempLen;
            return true;
        }

        public byte[] GetByteArrByString(string str) {

            return Convert.FromBase64String(str); 
        }

        public bool Register(UserFingerDto model)
        {
            var b1 = GetByteArrByString(model.Data1);
            var b2 = GetByteArrByString(model.Data2);
            var b3 = GetByteArrByString(model.Data3);

            int nFpRet = zkfingervein.DBAddEx(mDBHandle, 0,
             BitConverter.GetBytes(model.Id),  b1, b2, b3, 3);

            var b4 = GetByteArrByString(model.Data4);
            var b5 = GetByteArrByString(model.Data5);
            var b6 = GetByteArrByString(model.Data6);

            int nFvRet = zkfingervein.DBAddEx(mDBHandle, 1,
                 BitConverter.GetBytes(model.Id),
             b4,
              b5,
              b6,
                3); 

            if (zkfverrdef.ZKFV_ERR_OK != nFpRet && zkfverrdef.ZKFV_ERR_OK != nFvRet)
            {
                //txtResult.Text = "Enroll failed, DBAddEx failed!";
                return false;
            }
            return true;
        }

        public bool Register(int fingerId, List<string> target)
        {
            mEnrollIdx = 0;
            mbRegister = false;
            byte[] temp = new byte[mMaxFpTempLen];
            int nTempLen = mMaxFpTempLen;
            int nnRet = zkfingervein.MergeFPEx(mDevHandle, mPreRegFPRegTemps[0], mPreRegFPRegTemps[1], mPreRegFPRegTemps[2], 3, temp, ref nTempLen);
            if (zkfverrdef.ZKFV_ERR_OK != nnRet)
            {
               // txtResult.Text = "Enroll failed, because merge fingerprint failed!";
                return false;
            }

            byte[] a = { 1 };
            byte[] b = { 2 };
            int nFpRet = zkfingervein.DBAddEx(mDBHandle, 0,
                System.BitConverter.GetBytes(fingerId), mPreRegFPRegTemps[0], mPreRegFPRegTemps[1], mPreRegFPRegTemps[2], 3);


            int nFvRet = zkfingervein.DBAddEx(mDBHandle, 1,
                System.BitConverter.GetBytes(fingerId), mPreRegFVRegTemps[0], mPreRegFVRegTemps[1], mPreRegFVRegTemps[2], 3);

            target[0] = Convert.ToBase64String(mPreRegFPRegTemps[0]); //Encoding.ASCII.GetString(mPreRegFPRegTemps[0]); 
            target[1] = Convert.ToBase64String(mPreRegFPRegTemps[1]); //Encoding.ASCII.GetString(mPreRegFPRegTemps[1]);  
            target[2] = Convert.ToBase64String(mPreRegFPRegTemps[2]); //Encoding.ASCII.GetString(mPreRegFPRegTemps[2]);  

            target[3] = Convert.ToBase64String(mPreRegFVRegTemps[0]); //Encoding.ASCII.GetString(mPreRegFVRegTemps[0]); 
            target[4] = Convert.ToBase64String(mPreRegFVRegTemps[1]); //Encoding.ASCII.GetString(mPreRegFVRegTemps[1]);
            target[5] = Convert.ToBase64String(mPreRegFVRegTemps[2]); //Encoding.ASCII.GetString(mPreRegFVRegTemps[2]);


            if (zkfverrdef.ZKFV_ERR_OK != nFpRet && zkfverrdef.ZKFV_ERR_OK != nFvRet)
            {
                //txtResult.Text = "Enroll failed, DBAddEx failed!";
                return false;
            }

            Array.Copy(temp, mRegFPRegTemp, nTempLen);
            mRegFPRegTempLen = nTempLen;
            for (int i = 0; i < mEnrollCnt; i++)
            {
                Array.Copy(mPreRegFVRegTemps[i], mRegFVRegTemps[i], mPreRegFVTempLen[i]);
                mRegFVTempLen[i] = mPreRegFVTempLen[i];
            }
            return true;
            //mFingerID++;
            //txtResult.Text = "Enroll succ!";
        }


        private static void DeviceConnectedInvoke(bool bol) {
            OnDeviceConnected?.Invoke(bol);
        }

        private static void DeviceOpendInvoke(bool bol)
        {
            OnDeviceOpened?.Invoke(bol);
        }


        private void DoCapture()
        {
            int fpImageSize = 0;
            int fvImageSize = 0;
            while (this.IsDevOpend)
            {
                mfpTempLen = mfpTemp.Length;
                mfvTempLen = mfvTemp.Length;
                fpImageSize = mcbFpImg;
                fvImageSize = mcbFvImg;
                int ret = zkfingervein.CaptureFingerVeinImageAndTemplate(mDevHandle, mfpImg, ref fpImageSize, mfvImg, ref fvImageSize, mfpTemp, ref mfpTempLen, mfvTemp, ref mfvTempLen);
                if (ret == zkfverrdef.ZKFV_ERR_OK)
                {
                    //Console.WriteLine($"{DateTime.Now.ToLongTimeString()},mfpTempLen:{mfpTempLen},mfvTempLen:{mfvTempLen}");
                    if (mfpTempLen > 0 && mfvTempLen > 0)
                    {
                        OnCaptured?.Invoke(this);
                       
                    }
                }
                Thread.Sleep(200);
            }
        }
    }
}
