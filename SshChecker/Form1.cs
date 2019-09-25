using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci.SshNet;

namespace SshChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// SSHコマンド処理
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <returns>標準出力</returns>
        private string[] SshCmd(string command, int sshTimeoutSec = -1)
        {
            SshClient ssh = new SshClient("192.168.0.100", "user", "pass");
            ssh.Connect();
            string[] str_result;
            string str_results;
            char ptn = ',';

            int timeout = (sshTimeoutSec <= -1) ? 60 : sshTimeoutSec;
            // 60秒タイムアウト付きでコマンド実行
            var result = ssh.RunCommand("timeout -sKILL " + timeout + " " + command);
            str_results = result.Result;
            str_results = str_results.Trim('\n');
            str_result = str_results.Split(ptn);
            ssh.Disconnect();

            if ( str_result.Length > 0 && str_result[0].Equals("") )
            {
                throw new TimeoutException("SSHコマンドがタイムアウトしました");
            }

            return str_result;
        }

        /// <summary>
        /// 実行ボタン クリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRun_Click(object sender, EventArgs e)
        {
            string cmd = textBoxCommand.Text;
            string[] str_result = SshCmd(cmd);
            textBoxResult.Text = "";
            foreach (var item in str_result)
            {
                textBoxResult.Text += "- " + item + "\r\n";
            }
        }
    }
}
