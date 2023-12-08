using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices; //调用WINDOWS API函数时要用到
using Microsoft.VisualBasic;
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

            // 设置窗体最大化
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            // 设置背景
            this.BackColor = Color.White;
            this.TransparencyKey = Color.Black;
            this.Opacity = 1;

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

            // 图标大小
            int iconSize = 128;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int marginRight = 40;
            int marginBottom = 80;
            int startY = 200;
            int num = (screenWidth - 360) / 168;
            int startX = (screenWidth - 128 * num - (num - 1) * 40)/2;
            Color fontColor = Color.White;

            int j = 0;
            foreach (var ss in jsonArr)  //查找某个字段与值
            {
                // 计算放置的位置
                int positionX = startX + (iconSize + marginRight) * (j % num);
                int positionY = startY + ((int)(j / num)) * (iconSize+ marginBottom);

                Button btnItem = new Button();
                btnItem.Name = "btnShortcut" + j;
                //btnItem.Text = ((JObject)ss)["name"].ToString();
                btnItem.Text = ((JObject)ss)["name"].ToString();
                btnItem.Text = "";
                btnItem.Tag = ((JObject)ss)["path"].ToString();
                
                //btnItem.BackgroundImage = Bitmap.FromFile(((JObject)ss)["icon"].ToString());
                btnItem.BackgroundImage = Image.FromFile(((JObject)ss)["icon"].ToString());

                btnItem.BackgroundImageLayout = ImageLayout.Stretch;//设置背景图自适应
                btnItem.BackColor = Color.Transparent;
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
                lblItem.ForeColor = fontColor;
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
                j++;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void click_btnItem(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            // 目录：最后一个字符为斜杠
            // 文本文件：没有后缀名或者为txt等
            // exe：后缀名为exe
            string fileName = ((Button)sender).Tag.ToString();
            string strExt = System.IO.Path.GetExtension(fileName);
            if(strExt == ".exe")
            {
                System.Diagnostics.Process.Start(fileName);
            } else if(fileName.Substring(fileName.Length-1,1) == "\\")
            {
                System.Diagnostics.Process.Start("C:\\Windows\\explorer.exe", fileName);
            } else
            {
                System.Diagnostics.Process.Start("D:\\Program Files\\Microsoft VS Code\\Code.exe", fileName);
            }

        }

        //3.判断输入键值（实现KeyDown事件）
        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //判断按下的键（Alt + A）
            if (e.KeyValue == (int)Keys.T && (int)Control.ModifierKeys == (int)Keys.Alt)
            {
                this.Visible = false;
                this.WindowState = FormWindowState.Maximized;
                this.Visible = true;
                //System.Windows.Forms.MessageBox.Show("按下了指定快捷键组合");
            }

            if (e.KeyValue == (int)Keys.Y && (int)Control.ModifierKeys == (int)Keys.Alt)
            {
                this.WindowState = FormWindowState.Minimized;
                //System.Windows.Forms.MessageBox.Show("按下了指定快捷键组合");
            }
        }

        protected override CreateParams CreateParams
        {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}
