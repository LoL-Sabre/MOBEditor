using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace MOBEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MOBFile MOBFile;
        private string fileloc;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void buttonOpenMob_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                fileloc = ofd.FileName;
                MOBFile = new MOBFile(ofd.FileName);
                foreach(var o in MOBFile.MobObjects)
                {
                    listMOBObjects.Items.Add(o.MOBObjectName);
                }
                buttonEdit.IsEnabled = true;
            }
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            string selected = listMOBObjects.Items[listMOBObjects.SelectedIndex].ToString();
            var s = MOBFile.MobObjects.Find(x => x.MOBObjectName == selected);
            textObjectName.Text = s.MOBObjectName;
            textObjectFlag.Text = s.ObjectFlag.ToString();
            textPosX.Text = s.Position[0].ToString();
            textPosY.Text = s.Position[1].ToString();
            textPosZ.Text = s.Position[2].ToString();
            textRotX.Text = s.Rotation[0].ToString();
            textRotY.Text = s.Rotation[1].ToString();
            textRotZ.Text = s.Rotation[2].ToString();
            textScaX.Text = s.Scale[0].ToString();
            textScaY.Text = s.Scale[1].ToString();
            textScaZ.Text = s.Scale[2].ToString();
            textHealthPosX.Text = s.HealthBarPosition1[0].ToString();
            textHealthPosY.Text = s.HealthBarPosition1[1].ToString();
            textHealthPosZ.Text = s.HealthBarPosition1[2].ToString();
            textHealthScaX.Text = s.HealthBarPosition2[0].ToString();
            textHealthScaY.Text = s.HealthBarPosition2[1].ToString();
            textHealthScaZ.Text = s.HealthBarPosition2[2].ToString();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            var s = MOBFile.MobObjects.Find(x => x.MOBObjectName == textObjectName.Text);
            s.Position[0] = Convert.ToSingle(textPosX.Text);
            s.Position[1] = Convert.ToSingle(textPosY.Text);
            s.Position[2] = Convert.ToSingle(textPosZ.Text);

            s.Rotation[0] = Convert.ToSingle(textRotX.Text);
            s.Rotation[1] = Convert.ToSingle(textRotY.Text);
            s.Rotation[2] = Convert.ToSingle(textRotZ.Text);

            s.Scale[0] = Convert.ToSingle(textScaX.Text);
            s.Scale[1] = Convert.ToSingle(textScaY.Text);
            s.Scale[2] = Convert.ToSingle(textScaZ.Text);

            s.HealthBarPosition1[0] = Convert.ToSingle(textHealthPosX.Text);
            s.HealthBarPosition1[1] = Convert.ToSingle(textHealthPosY.Text);
            s.HealthBarPosition1[2] = Convert.ToSingle(textHealthPosZ.Text);

            s.HealthBarPosition2[0] = Convert.ToSingle(textHealthScaX.Text);
            s.HealthBarPosition2[1] = Convert.ToSingle(textHealthScaY.Text);
            s.HealthBarPosition2[2] = Convert.ToSingle(textHealthScaZ.Text);
        }

        private void buttonSaveMOB_Click(object sender, RoutedEventArgs e)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileloc));

            bw.Write(MOBFile.Magic.ToCharArray());
            bw.Write(MOBFile.Version);
            bw.Write(MOBFile.NumberOfObjects);
            bw.Write(MOBFile.Zero);
            foreach(var o in MOBFile.MobObjects)
            {
                bw.Write(o.MOBObjectNameChar);
                bw.Write(o.ObjectZero1);
                bw.Write((byte)o.ObjectFlag);
                bw.Write(o.ObjectZero2);
                for(int i = 0; i < 3; i++)
                {
                    bw.Write(o.Position[i]);
                }
                for (int i = 0; i < 3; i++)
                {
                    bw.Write(o.Rotation[i]);
                }
                for (int i = 0; i < 3; i++)
                {
                    bw.Write(o.Scale[i]);
                }
                for (int i = 0; i < 3; i++)
                {
                    bw.Write(o.HealthBarPosition1[i]);
                }
                for (int i = 0; i < 3; i++)
                {
                    bw.Write(o.HealthBarPosition2[i]);
                }
                bw.Write(o.ObjectZero3);
            }
            bw.Dispose();
            bw.Close();
        }
    }
}
