using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Calculate
{


    public partial class Form1 : Form
    {




        private string parametrs = "";
        private string boxShow = "";
        private string callback = "";
        private List<string> list = new List<string>();
        private List<string> specificity = new List<string>();
        private bool change = false;

        public Form1()
        {
            InitializeComponent();
            initActions();
            list.Add("+");
            list.Add("/");
            list.Add("*");
            list.Add("^");
            specificity.Add("comm");
            specificity.Add("clear");
            specificity.Add("=");
        }


        public enum ErrorCode
        {
            SUCCESS = 0,
            BUFFER_OVERFLOW = 1,
            DIVIDE_BY_ZERO = 2
        }


        [DllImport(".\\CalcBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ErrorCode calculate(string input, out double result);

        [DllImport(".\\CalcBackend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool isShift();


        private void ButtonClick(object sender, EventArgs e)
        {

            Button button = (Button)sender;
            string buttonId = button.Tag.ToString();


            char check = parametrs.Length > 0 ? parametrs.ElementAt<char>(parametrs.Length - 1) : '\0';
            if ((parametrs == "" || (list.Contains(Convert.ToString(check))) || check == ',') && list.Contains(buttonId) && buttonId != "-")
            {
                return;
            }

            bool spec = handleSpsButtion(buttonId);
            if (!spec) {
                parametrs += buttonId;
                boxShow += buttonId;
                textBox1.Text = boxShow;
                change = true;
            }

            
        }

        private bool handleSpsButtion(string buttonId)
        {
            if (!specificity.Contains(buttonId))
            {
                return false;
            }

            switch (buttonId)
            {
                case "clear":
                    if (boxShow == "") return true;
                    boxShow = isShift() ? "" : boxShow.Substring(0, boxShow.Length - 1);
                    textBox1.Text = boxShow;
                    parametrs = boxShow;
                    break;

                case "comm":

                    if (boxShow == "")
                        return true;

                    if (boxShow.Contains(","))
                    {
                        return true;
                    }

                    boxShow += ",";
                    parametrs += ".";
                    textBox1.Text = boxShow;
                    break;

                case "=":
                    callback = parametrs;
                    textBox2.Text = callback;


                    if (!change)
                        return true;

                    if(parametrs == "")
                    {
                        callback = "";
                        textBox2.Text = callback;
                        return true;
                    }
                    double out_res = 0;
                    ErrorCode d = calculate(parametrs, out out_res);
                
                   if(d == ErrorCode.DIVIDE_BY_ZERO)
                    {
                        MessageBox.Show("Нельзя делить на 0!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }else if(d == ErrorCode.BUFFER_OVERFLOW)
                    {
                        clear();
                        MessageBox.Show("Буффер переполнен!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }

                    string result = Convert.ToString(out_res);
                    textBox1.Text = result;
                    parametrs = result;
                    boxShow = result;
                    change = false;
                    break;
            }
            return true;
        }

        void clear()
        {
            parametrs = "";
            boxShow = "";
            textBox1.Text = "";
            callback = "";
            textBox2.Text = "";
        }

        void initActions()
        {
            button1.Click += ButtonClick;
            button2.Click += ButtonClick;
            button3.Click += ButtonClick;
            button4.Click += ButtonClick;
            button5.Click += ButtonClick;
            button6.Click += ButtonClick;
            button7.Click += ButtonClick;
            button8.Click += ButtonClick;
            button9.Click += ButtonClick;
            button11.Click += ButtonClick;
            button12.Click += ButtonClick;
            button13.Click += ButtonClick;
            button14.Click += ButtonClick;
            button16.Click += ButtonClick;
            button18.Click += ButtonClick;
            button19.Click += ButtonClick;
            button17.Click += ButtonClick;
            button10.Click += ButtonClick;
        }

    }

   

}
