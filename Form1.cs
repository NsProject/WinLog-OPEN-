using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace WinLog
{
    public partial class Form1 : Form
    {
        public bool osstartupflag = true;
        public bool timesignalflag = true;
        public string osstartupcash;
        public string[] pronamawhisper;
        public int state;

        private WinLog Winlog = new WinLog();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //画像の表示
            int count=this.Winlog.FolderToGraphCountGet("graph");
            Random cRandom = new System.Random();
            int r = cRandom.Next(count);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = System.Drawing.Image.FromFile(@"graph/"+r.ToString()+".png");

            //稼働時間の設定
            timer1.Interval = 1000;
            timer1.Enabled = true;

            //非表示の設定
            textBox1.Visible = false;

            //コメント
            timer2.Interval = 1000;
            timer2.Enabled = true;

            //フォームの設定
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.pronamawhisper = Winlog.FileRead(@"script.txt");
        }

        private void button1_Click(object sender, EventArgs e)
        {//パソコン起動時間＆終了時間の表示

            if (!textBox1.Visible||state==1)
            {
                state = 2;
                //非表示の設定
                label1.Visible = false;
                label2.Visible = false;

                //表示の設定
                textBox1.Visible = true;
                textBox1.WordWrap = false;
                textBox1.ScrollBars = ScrollBars.Both;
                textBox1.Text = "";

                Task t1 = new Task(this.OsStartUpIni);
                t1.Start();

                if (this.osstartupflag)
                {
                    MessageBox.Show("読み込みにしばらく時間が掛かります\r\n今しばらくお待ちください");
                }
            }
            else
            {
                label1.Visible = true;
                label2.Visible = true;
                textBox1.Visible = false;
                state=0;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {//起動中のアプリケーションの表示

            if (!textBox1.Visible||state==2)
            {
                state = 1;
                //非表示の設定
                label1.Visible = false;
                label2.Visible = false;

                //表示の設定
                textBox1.Visible = true;
                textBox1.WordWrap = false;
                textBox1.ScrollBars = ScrollBars.Both;
                textBox1.Text = "";

                System.Diagnostics.Process[] hProcesses = System.Diagnostics.Process.GetProcesses();
                string stPrompt = string.Empty;

                // 取得できたプロセスからプロセス名を取得する
                foreach (System.Diagnostics.Process hProcess in hProcesses)
                {
                    try
                    {
                        textBox1.Text += "・" + hProcess.ProcessName + "\r\n";
                        if (hProcess.MainWindowTitle != "")
                        {
                            textBox1.Text += hProcess.MainWindowTitle + "\r\n";
                        }

                        try
                        {
                            textBox1.Text += "起動時間：" + hProcess.StartTime + "\r\n";
                            TimeSpan time = System.DateTime.Now - hProcess.StartTime;
                            textBox1.Text += "経過時間：" + time.Hours + ":" + time.Minutes + ":" + time.Seconds + "\r\n\r\n";
                        }
                        catch
                        {
                            textBox1.Text += "\r\n";
                        }
                        
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                label1.Visible = true;
                label2.Visible = true;
                textBox1.Visible = false;
                state = 0;
            }
        }    
        
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ソフト情報\nバージョン：β版\n製作　　　：NsProject");
        }

        private void OsStartUpIni()
        {//パソコン起動＆終了時間の表示処理
            int i = 0, j = 1;

            if (this.osstartupflag)
            {//キャッシュされていなかったら
                System.Diagnostics.EventLog[] logs = System.Diagnostics.EventLog.GetEventLogs();
                foreach (System.Diagnostics.EventLog log in logs)
                {
                    try
                    {
                        if (log.LogDisplayName == "システム")
                        {
                            foreach (System.Diagnostics.EventLogEntry entry in log.Entries)
                            {
                                switch (entry.EventID)
                                {
                                    case 6005:
                                        if (i > 0)
                                        {
                                          this.osstartupcash += "強制終了したため終了時間が分かりません\r\n\r\n";
                                        }
                                        this.osstartupcash += "起動時間：" + entry.TimeWritten + "\r\n";
                                        i++;
                                        j++;
                                        break;
                                    case 6006:
                                        this.osstartupcash += "起動終了：" + entry.TimeWritten + "\r\n\r\n";
                                        i = 0;
                                        j++;
                                        break;
                                }
                                textBox1.Text = j + "件読み込み中。。。";
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                this.osstartupflag = false;               
            }
            textBox1.Text = this.osstartupcash;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!label2.Visible || !label2.Visible)
            {
                label1.Visible = true;
                label2.Visible = true;
                textBox1.Visible = false;
            }
            else
            {
                DialogResult dr = MessageBox.Show("私に何か用かしら？", "", MessageBoxButtons.YesNo);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    pictureBox1.Image = System.Drawing.Image.FromFile(@"graph/2.png");
                }
                else if (dr == System.Windows.Forms.DialogResult.No)
                {
                    pictureBox1.Image = System.Drawing.Image.FromFile(@"graph/10.png");
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.timesignalflag)
            {
                this.timesignalflag = false;
            }
            else
            {
                this.timesignalflag = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {//起動時間の表示
            int s = this.Winlog.SystemStartUp();
            TimeSpan ts = new TimeSpan(0, 0, s);
            string hms = ts.ToString();
            label1.Text = "パソコンを起動してから\n" + hms + "\nぐらい経過しましたよ";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            int now_time = int.Parse(now.ToString("HH"));

            string target="EOF";
            string[] delimiter = {"@"};
            label2.Text = "";

            if (now_time >= 7 && now_time <= 11)
            {
                target = this.pronamawhisper[0];
            }

            if (now_time >= 13 && now_time <= 17)
            {
                target = this.pronamawhisper[1];
            }

            if (now_time >= 19 && now_time <= 23)
            {
                target = this.pronamawhisper[2];
            } 
            
            if (now_time >= 1 && now_time <= 6)
            {
                target = this.pronamawhisper[3];
            }

            if (target != "EOF")
            {
                string[] copy = target.Split(delimiter, StringSplitOptions.None);

                foreach (string s in copy)
                {
                    label2.Text += s + "\n";
                }
            }

            if (now_time % 3 == 0)
            {
                if (this.timesignalflag)
                {
                    if (now_time == 12)
                    {
                        label2.Text = now_time + "時か～\nもうお腹がぺこぺこー\n何か食べて来ようっと♪♪";
                    }
                    else if (now_time == 0)
                    {
                        label2.Text = now_time + "時\nもうこんな時間かー\nまだ作業が残っているけど\n眠たいな～寝ようかな～";
                    }
                    else
                    {
                        label2.Text = now_time + "時かー\nうーん、よしっ！\nちょっと気分転換に体を動かして来ますね♪";
                    }
                }
                else
                {
                    label2.Text = "もうこんな時間かー\nちょっと疲れたから休んでくるね";
                }
            }
        }
    }
}
