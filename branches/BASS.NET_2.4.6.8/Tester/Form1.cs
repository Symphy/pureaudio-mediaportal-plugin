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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Player.PureAudio;
using Un4seen.Bass;
using Un4seen.BassAsio;
using Un4seen.Bass.AddOn.Cd;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn;
using Un4seen.Bass.AddOn.Vst;
//using Un4seen.Bass.AddOn.Wa;
using Un4seen.Bass.Misc;

namespace Tester
{
	public partial class Form1 : Form
	{
		private BASSPlayer _BackGroundPlayer = new BASSPlayer();

		public Form1()
		{
			InitializeComponent();
			timer1.Start();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			bool result = true;
			if (result)
			{
				_BackGroundPlayer.DebugMode = true;
				_BackGroundPlayer.Profile.UseASIO = false;
				_BackGroundPlayer.Profile.ASIODevice = "ASIO4ALL v2";
				_BackGroundPlayer.Profile.ASIOFirstChan = 0;
				_BackGroundPlayer.Profile.ASIOLastChan = 2;

				_BackGroundPlayer.Profile.DoSoftStop = true;
				_BackGroundPlayer.Profile.SoftStopDuration = 500;
				_BackGroundPlayer.Profile.StereoUpMix = StereoUpMix.None;
				_BackGroundPlayer.Profile.PlayBackBufferSize = 2000;
				_BackGroundPlayer.Profile.BASSPlayBackBufferSize = 1000;
				_BackGroundPlayer.Profile.GapLength = 1000;
				_BackGroundPlayer.Profile.VizLatencyCorrection = 500;

				//_BackGroundPlayer.Ended += new BackGroundBASSPlayer.EndedDelegate(_BackGroundPlayer_Ended);
				result = _BackGroundPlayer.InitBASS();
			}

			if (result)
			{
        _BackGroundPlayer.Play(@"D:\My Documents\My Music\Surround\DTS\DEMO2_DTS.wav");
				//_BackGroundPlayer.Play(@"TestFiles\dance.mp3");
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
			_BackGroundPlayer.Stop();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			_BackGroundPlayer.Pause();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			label1.Text = _BackGroundPlayer.CurrentPosition.ToString("#####") + " / " + _BackGroundPlayer.Duration.ToString("#####");
		}

		private void button4_Click(object sender, EventArgs e)
		{
			_BackGroundPlayer.Rewind();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			_BackGroundPlayer.Forward();
		}

		private void button6_Click(object sender, EventArgs e)
		{
			_BackGroundPlayer.TogglePlayBackMode();
			label2.Text = _BackGroundPlayer.PlayBackMode.ToString();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			label2.Text = _BackGroundPlayer.PlayBackMode.ToString();
		}

		private void button7_Click(object sender, EventArgs e)
		{
			ConfigurationForm confForm = new ConfigurationForm();
			confForm.ShowDialog();

		}
	}
}