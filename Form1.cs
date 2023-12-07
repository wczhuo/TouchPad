using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices; //调用WINDOWS API函数时要用到
using Microsoft.Win32; //写入注册表时要用到
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace TouchPad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 设置窗体最大化
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            // 设置背景
            this.BackColor = Color.White;
            this.TransparencyKey = Color.White;
            this.Opacity = 0.9;

            // 图标大小
            int iconSize = 128;

            this.BackgroundImage = Bitmap.FromFile("bg.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;//设置背景图自适应

            //安装键盘钩子
            KeyboardHook k_hook = new KeyboardHook();
            k_hook.KeyDownEvent += new KeyEventHandler(hook_KeyDown);//钩住键按下
            k_hook.Start();//安装键盘钩子

            // 展示快捷方式
            /*List<ListDictionary> shortcutList = new List<ListDictionary>();
            for(int i = 0; i <= -1; i++)
            {
                ListDictionary item = new ListDictionary();
                item.Add("name","快捷方式" + i);
                item.Add("path", "C:/Users/wczhuo/Desktop/HBuilder X.lnk");
                shortcutList.Add(item);

                //Console.WriteLine(de.Key + " " + de.Value);
                //实例化GroupBox控件
                Button btnItem = new Button();
                btnItem.Name = "btnShortcut" + i;
                btnItem.Text = "C:\\Program Files\\Genshin Impact\\launcher.exe";
                btnItem.Text = "C:\\Users\\wczhuo\\Desktop\\原神.lnk";
                //设置上和左位置
                //groubox.Top = 50;
                //groubox.Left = 50;
                //通过坐标设置位置
                btnItem.Location = new Point(120*i, 120);
                btnItem.Click += new EventHandler(click_btnItem);
                //将groubox添加到页面上
                this.Controls.Add(btnItem);
            }*/


            string wordTemplateName = "Config.json";
            StreamReader sr = File.OpenText(wordTemplateName);
            string jsonWordTemplate = sr.ReadToEnd();
            JArray jsonArr = (JArray)JsonConvert.DeserializeObject(jsonWordTemplate);

            int j = 2;
            foreach (var ss in jsonArr)  //查找某个字段与值
            {
                j++;
                int positionX = (iconSize + 40) * j;
                int positionY = 120;
                Button btnItem = new Button();
                btnItem.Name = "btnShortcut" + j;
                //btnItem.Text = ((JObject)ss)["name"].ToString();
                btnItem.Text = ((JObject)ss)["name"].ToString();
                btnItem.Text = "";
                btnItem.Tag = ((JObject)ss)["path"].ToString();
                btnItem.BackgroundImage = Bitmap.FromFile(((JObject)ss)["icon"].ToString());
                btnItem.BackgroundImageLayout = ImageLayout.Stretch;//设置背景图自适应
                btnItem.Width = iconSize;
                btnItem.Height = iconSize;
                //设置上和左位置
                //groubox.Top = 50;
                //groubox.Left = 50;
                //通过坐标设置位置
                btnItem.Location = new Point(positionX, positionY);
                btnItem.Click += new EventHandler(click_btnItem);
                //将groubox添加到页面上
                this.Controls.Add(btnItem);


                Label lblItem = new Label();
                lblItem.Text = ((JObject)ss)["name"].ToString();
                lblItem.ForeColor = Color.Red;
                lblItem.Font = new Font("微软雅黑", 15, FontStyle.Bold);  //字体
                lblItem.BackColor = Color.Transparent;
                //lblItem.TransparencyKey = Color.White;
                //lblItem.Opacity = 0.8;
                lblItem.Location = new Point(positionX, positionY + iconSize + 20);
                lblItem.Width = iconSize;
                lblItem.TextAlign = ContentAlignment.MiddleCenter;
                //btnItem.Click += new EventHandler(click_btnItem);
                //将groubox添加到页面上
                this.Controls.Add(lblItem);
            }

        }

        private void click_btnItem(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(((Button)sender).Tag.ToString());
        }


        //3.判断输入键值（实现KeyDown事件）
        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //判断按下的键（Alt + A）
            if (e.KeyValue == (int)Keys.T && (int)Control.ModifierKeys == (int)Keys.Alt)
            {
                this.WindowState = FormWindowState.Maximized;
                //System.Windows.Forms.MessageBox.Show("按下了指定快捷键组合");
            }

            if (e.KeyValue == (int)Keys.Y && (int)Control.ModifierKeys == (int)Keys.Alt)
            {
                this.WindowState = FormWindowState.Minimized;
                //System.Windows.Forms.MessageBox.Show("按下了指定快捷键组合");
            }
        }
    }
}
