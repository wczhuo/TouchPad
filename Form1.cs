using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices; //����WINDOWS API����ʱҪ�õ�
using Microsoft.Win32; //д��ע���ʱҪ�õ�
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
            // ���ô������
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            // ���ñ���
            this.BackColor = Color.White;
            this.TransparencyKey = Color.White;
            this.Opacity = 0.9;

            // ͼ���С
            int iconSize = 128;

            this.BackgroundImage = Bitmap.FromFile("bg.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;//���ñ���ͼ����Ӧ

            //��װ���̹���
            KeyboardHook k_hook = new KeyboardHook();
            k_hook.KeyDownEvent += new KeyEventHandler(hook_KeyDown);//��ס������
            k_hook.Start();//��װ���̹���

            // չʾ��ݷ�ʽ
            /*List<ListDictionary> shortcutList = new List<ListDictionary>();
            for(int i = 0; i <= -1; i++)
            {
                ListDictionary item = new ListDictionary();
                item.Add("name","��ݷ�ʽ" + i);
                item.Add("path", "C:/Users/wczhuo/Desktop/HBuilder X.lnk");
                shortcutList.Add(item);

                //Console.WriteLine(de.Key + " " + de.Value);
                //ʵ����GroupBox�ؼ�
                Button btnItem = new Button();
                btnItem.Name = "btnShortcut" + i;
                btnItem.Text = "C:\\Program Files\\Genshin Impact\\launcher.exe";
                btnItem.Text = "C:\\Users\\wczhuo\\Desktop\\ԭ��.lnk";
                //�����Ϻ���λ��
                //groubox.Top = 50;
                //groubox.Left = 50;
                //ͨ����������λ��
                btnItem.Location = new Point(120*i, 120);
                btnItem.Click += new EventHandler(click_btnItem);
                //��groubox��ӵ�ҳ����
                this.Controls.Add(btnItem);
            }*/


            string wordTemplateName = "Config.json";
            StreamReader sr = File.OpenText(wordTemplateName);
            string jsonWordTemplate = sr.ReadToEnd();
            JArray jsonArr = (JArray)JsonConvert.DeserializeObject(jsonWordTemplate);

            int j = 2;
            foreach (var ss in jsonArr)  //����ĳ���ֶ���ֵ
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
                btnItem.BackgroundImageLayout = ImageLayout.Stretch;//���ñ���ͼ����Ӧ
                btnItem.Width = iconSize;
                btnItem.Height = iconSize;
                //�����Ϻ���λ��
                //groubox.Top = 50;
                //groubox.Left = 50;
                //ͨ����������λ��
                btnItem.Location = new Point(positionX, positionY);
                btnItem.Click += new EventHandler(click_btnItem);
                //��groubox��ӵ�ҳ����
                this.Controls.Add(btnItem);


                Label lblItem = new Label();
                lblItem.Text = ((JObject)ss)["name"].ToString();
                lblItem.ForeColor = Color.Red;
                lblItem.Font = new Font("΢���ź�", 15, FontStyle.Bold);  //����
                lblItem.BackColor = Color.Transparent;
                //lblItem.TransparencyKey = Color.White;
                //lblItem.Opacity = 0.8;
                lblItem.Location = new Point(positionX, positionY + iconSize + 20);
                lblItem.Width = iconSize;
                lblItem.TextAlign = ContentAlignment.MiddleCenter;
                //btnItem.Click += new EventHandler(click_btnItem);
                //��groubox��ӵ�ҳ����
                this.Controls.Add(lblItem);
            }

        }

        private void click_btnItem(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(((Button)sender).Tag.ToString());
        }


        //3.�ж������ֵ��ʵ��KeyDown�¼���
        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //�жϰ��µļ���Alt + A��
            if (e.KeyValue == (int)Keys.T && (int)Control.ModifierKeys == (int)Keys.Alt)
            {
                this.WindowState = FormWindowState.Maximized;
                //System.Windows.Forms.MessageBox.Show("������ָ����ݼ����");
            }

            if (e.KeyValue == (int)Keys.Y && (int)Control.ModifierKeys == (int)Keys.Alt)
            {
                this.WindowState = FormWindowState.Minimized;
                //System.Windows.Forms.MessageBox.Show("������ָ����ݼ����");
            }
        }
    }
}
