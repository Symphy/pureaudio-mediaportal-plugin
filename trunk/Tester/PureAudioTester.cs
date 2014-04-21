#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Windows.Forms;
using MediaPortal.Player.PureAudio;
using System.Text;

namespace PureAudioTester
{
  public partial class PureAudioTester : Form
  {
    private BASSPlayer _BASSPlayer = new BASSPlayer();
    
    private StringBuilder _SbLog = new StringBuilder();
    private bool _SbLogUpdated = false;

    private StringBuilder _SbEvents = new StringBuilder();
    private bool _SbEventsUpdated = false;

    public PureAudioTester()
    {
      _BASSPlayer.SessionStarted += new BASSPlayer.SessionStartedDelegate(_BASSPlayer_SessionStarted);
      _BASSPlayer.SessionStopped += new BASSPlayer.SessionStoppedDelegate(_BASSPlayer_SessionStopped);
      _BASSPlayer.Stopped += new BASSPlayer.StoppedDelegate(_BASSPlayer_Stopped);
      _BASSPlayer.Ended += new BASSPlayer.EndedDelegate(_BASSPlayer_Ended);
      _BASSPlayer.MonitorProcess += new BASSPlayer.MonitorProcessDelegate(_BASSPlayer_MonitorProcess);
      _BASSPlayer.MetaStreamTagsChanged += new BASSPlayer.MetaStreamTagsChangedDelegate(_BASSPlayer_MetaStreamTagsChanged);
      _BASSPlayer.StreamTagsChanged += new BASSPlayer.StreamTagsChangedDelegate(_BASSPlayer_StreamTagsChanged);
      _BASSPlayer.WaitCursorRequested += new BASSPlayer.WaitCursorRequestedDelegate(_BASSPlayer_WaitCursorRequested);
      
      Log.Write += new Log.LogDelegate(Log_Write);

      InitializeComponent();
    }

    private void PureAudioTest_Load(object sender, EventArgs e)
    {
      _BASSPlayer.Initialize();
      _BASSPlayer.DebugMode = true;

      tmrStatus.Start();

      foreach (string fileName in Properties.Settings.Default.FileNames)
        cboFileName.Items.Add(fileName);
      
      if (cboFileName.Items.Count > 0)
        cboFileName.SelectedIndex = 0;
    }

    private void tmrStatus_Tick(object sender, EventArgs e)
    {
      lblCurrentFilePath.Text = _BASSPlayer.CurrentFilePath;
      lblCurrentFileType.Text = _BASSPlayer.CurrentFileType.ToString();
      lblPlaybackMode.Text = _BASSPlayer.PlayBackMode.ToString();
      lblCurrentPosition.Text = _BASSPlayer.CurrentPosition.ToString("#####") + " / " + _BASSPlayer.Duration.ToString("#####");
      lblLastError.Text = _BASSPlayer.LastError.ToString();
      
      if (_SbLogUpdated)
      {
        tbLog.Text = _SbLog.ToString();
        tbLog.SelectionStart = tbLog.Text.Length;
        tbLog.ScrollToCaret();

        _SbLogUpdated = false;
      }
      
      if (_SbEventsUpdated)
      {
        tbEvents.Text = _SbEvents.ToString();
        tbEvents.SelectionStart = tbEvents.Text.Length;
        tbEvents.ScrollToCaret();

        _SbEventsUpdated = false;
      }
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      string fileName = "";

      bool result = cboFileName.SelectedItem != null;

      if (result)
      {
        fileName = cboFileName.SelectedItem.ToString();
        result = !String.IsNullOrEmpty(fileName);
      }

      if (result)
      {
        _BASSPlayer.Play(fileName);
      }
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
      _BASSPlayer.Stop();
    }

    private void btnPause_Click(object sender, EventArgs e)
    {
      _BASSPlayer.Pause();
    }

    private void btnBack_Click(object sender, EventArgs e)
    {
      _BASSPlayer.Rewind();
    }

    private void btnForward_Click(object sender, EventArgs e)
    {
      _BASSPlayer.Forward();
    }

    private void btnToggleMode_Click(object sender, EventArgs e)
    {
      _BASSPlayer.TogglePlayBackMode();
    }

    private void btnConfig_Click(object sender, EventArgs e)
    {
      ConfigurationForm confForm = new ConfigurationForm();
      confForm.ShowDialog();
    }

    private void btnGetFile_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
      {
        string fileName = openFileDialog.FileName;
        if (!cboFileName.Items.Contains(fileName))
          cboFileName.Items.Add(fileName);
        cboFileName.SelectedItem = fileName;
      }
    }

    private void cboFileName_Validated(object sender, EventArgs e)
    {
      if (!cboFileName.Items.Contains(cboFileName.Text))
        cboFileName.Items.Add(cboFileName.Text);
      cboFileName.SelectedItem = cboFileName.Text;
    }

    void _BASSPlayer_SessionStopped()
    {
      LogEvent("SessionStopped");
    }

    void _BASSPlayer_SessionStarted()
    {
      LogEvent("SessionStarted");
    }

    void _BASSPlayer_Stopped()
    {
      LogEvent("Stopped");
    }

    void _BASSPlayer_Ended()
    {
      LogEvent("Ended");
    }

    void _BASSPlayer_MonitorProcess()
    {
      //LogEvent("MonitorProcess");
    }

    void _BASSPlayer_MetaStreamTagsChanged(object sender, string tags)
    {
      LogEvent("MetaStreamTagsChanged");
    }

    void _BASSPlayer_StreamTagsChanged(object sender, string[] tags)
    {
      LogEvent("StreamTagsChanged");
    }

    void _BASSPlayer_WaitCursorRequested(object sender, BASSPlayer.WaitCursorRequest request)
    {
      LogEvent(String.Format("WaitCursorRequested({0})", request));
    }

    void Log_Write(Log.LogType logType, string format, params object[] arg)
    {
      _SbLog.AppendLine(String.Format(
        "[{0}] [{1}] - {2}",
        System.Threading.Thread.CurrentThread.Name,
        logType, String.Format(format, arg)));

      _SbLogUpdated = true;
    }

    private delegate void LogEventDelegate(string eventName);

    private void LogEvent(string eventName)
    {
      _SbEvents.AppendLine(eventName);
      _SbEventsUpdated = true;
    }

    private void PureAudioTest_FormClosed(object sender, FormClosedEventArgs e)
    {
      Properties.Settings.Default.FileNames.Clear();

      foreach (string fileName in cboFileName.Items)
        Properties.Settings.Default.FileNames.Add(fileName);

      Properties.Settings.Default.Save();
    }
  }
}