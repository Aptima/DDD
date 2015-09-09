using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;

using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;

using System.Runtime.InteropServices; // DllImport

namespace Aptima.Asim.DDD.CommonComponents.VoiceRecorder
{
    //The VoiceRecorder class is used to log talking events and to record the audio on each channel.  
    //There should be one instance of this class running on a separate thread for each channel
    // that has been created.
    public class VoiceRecorder
    {
        // Here's the VoIP Evo voice client
        //If you get a compiler error here it's because the COM control is not
        //registered. Run "regsvr32.exe EvoVoIP.dll" in the COM-folder.
        private ConaitoLib.EvoVoIP voipclient;

        private int channelID;
        private bool recording = true;
        private StringBuilder fileDirectory = new StringBuilder("C:\\Software\\DDDVoice\\VoiceRecorder\\bin\\Debug");
        private StringBuilder fileName = new StringBuilder();

        [DllImport("VoiceLog.dll")]
        public static extern int VL_CreateVoiceLog([MarshalAs(UnmanagedType.LPStr)] StringBuilder szTmpDir, [MarshalAs(UnmanagedType.LPStr)] StringBuilder szMp3File, int nFlushInterval);
        [DllImport("VoiceLog.dll")]
        public static extern int VL_CloseVoiceLog(int bAbort);
        [DllImport("VoiceLog.dll")]
        public static extern int VL_AddStreamData(int nStreamID, int nSampleRate, System.IntPtr lpStreamData, int nSampleCount);
        [DllImport("VoiceLog.dll")]
        public static extern int VL_EndStream(int nStreamID);

        public VoiceRecorder(int channelNo)
        {
            //simModelName = simModelPath;
            //simModelInfo = smr.readModel(simModelName);
            //isRunning = false;
            //isLoggedIn = false;
            //roomMembership = new Dictionary<string, List<string>>();
            //channelIDMap = new Dictionary<string, int>();
            //userChannelMap = new Dictionary<int, List<int>>();
            //server = new SimulationEventDistributorClient();
            //distributor.RegisterClient(ref server);

            channelID = channelNo;

            fileName.Append("DDDVoiceRecord_" + channelID.ToString());

            voipclient = new ConaitoLib.EvoVoIPClass();

            if (!voipclient.InitVoIP(true))
            {
                ErrorLog.Write("Failed to initialize VoIP Evo voice client");
            }

            //Register the user is talking event.  Use this to log user talking events to the DDD.
            voipclient.OnUserTalking += new ConaitoLib.IEvoVoIPEvents_OnUserTalkingEventHandler(this.voipclient_OnUserTalking);

            //notification containing the raw audio (PCM data) which was played when a user was talking.
            voipclient.OnUserAudioData += new ConaitoLib.IEvoVoIPEvents_OnUserAudioDataEventHandler(this.voipclient_OnUserAudioData);

            if (!System.IO.File.Exists("lame_enc.dll"))
            {
                ErrorLog.Write("Unable to find lame_enc.dll. Please download it at http://lame.sourceforge.net");
                return;
            }

                //set up the file directory and file name to record to.
                //StringBuilder tempdir = new StringBuilder(tempdirTextBox.Text);
                //StringBuilder mp3file = new StringBuilder(mp3fileTextBox.Text);
                if (VL_CreateVoiceLog(fileDirectory, fileName, 5) == 0)
                    ErrorLog.Write("Failed to start voice-log");
        }


        public void StartVoiceRecord()
        {

            ErrorLog.Write("Recording started on channel " + channelID);
        }

        private void voipclient_OnUserTalking(int userid)
        {
            //need to change this to create the DDD event to be logged.
            ErrorLog.Write("User id: " + userid + " is talking.");
        }

        private void voipclient_OnUserAudioData(int userid, int samplerate, ref System.Array rawAudio, int samples)
        {
            if (samples > 0)
            {
                System.IntPtr ptr = Marshal.AllocHGlobal(samples * 2);
                short[] shortArr = (short[])rawAudio;
                Marshal.Copy(shortArr, 0, ptr, samples);
                VL_AddStreamData(userid, samplerate, ptr, samples);
                Marshal.FreeHGlobal(ptr);
            }
            else
                VL_EndStream(userid);
        }
        public void Abort()
        {
            VL_CloseVoiceLog(0);
            //recordButton.Text = "Record";
            recording = false;

        }
    }

}
