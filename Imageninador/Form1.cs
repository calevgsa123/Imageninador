using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Imageninador
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Abre la ventana de dialogo para poder buscar la carpete de trabajo
            if (carpeta.ShowDialog() == DialogResult.OK)
            {
                globales.DirectorioPrincipal = carpeta.SelectedPath;
                cargarDirectoriosLocales(globales.DirectorioPrincipal);
                cargarImagenesLocales(globales.DirectorioPrincipal);
                PictureBox picture = (PictureBox)navImagenesPanel.Controls[navImagenesPanel.Controls.Count-1].Controls[0];
                imagenClick(picture, e);
            }
        }
        private void cargarDirectoriosLocales(string directorioLocal)
        {
            //Agrega en el panel dLocales los directorios de la carpeta local
            string[] directoriosLocales = System.IO.Directory.GetDirectories(directorioLocal);
            dLocales.Controls.Clear();
            if (directoriosLocales.Length > 0)
            {
                
                foreach (string directorio in directoriosLocales)
                {
                    PictureBox dLocalesP = new PictureBox();
                    Panel dLocalesPanel = new Panel();
                    Label dLocalesLabel = new Label();
                    dLocalesPanel.Parent = dLocales;
                    dLocalesPanel.Dock = DockStyle.Top;
                    dLocalesPanel.Height = 113;
                    dLocalesLabel.Parent = dLocalesPanel;
                    dLocalesP.Parent= dLocalesPanel;
                    dLocalesP.Dock = DockStyle.Top;
                    dLocalesP.Height = 100;
                    dLocalesP.Image = getImagenDirectorios(directorio);
                    dLocalesP.Tag = directorio;
                    dLocalesP.SizeMode = PictureBoxSizeMode.Zoom;
                    dLocalesLabel.Text = getDirectorynombre(directorio);
                    dLocalesLabel.ForeColor = Color.YellowGreen;
                    dLocalesLabel.TextAlign = ContentAlignment.TopCenter;
                    dLocalesLabel.Dock = DockStyle.Top;
                    dLocalesLabel.BackColor = Color.White;
                    toolTip1.SetToolTip(dLocalesP, directorio);
                    dLocalesP.MouseEnter += new System.EventHandler(imageHover);
                    dLocalesP.MouseLeave += new System.EventHandler(imageLeave);
                    dLocalesP.Click += new System.EventHandler(directorioClick);
                }
            }
            else { dLocales.Controls.Add(clonarLabelVacio()); }
        }
        private Image getImagenDirectorios(string directorio)
        {
            string[] imagenes = System.IO.Directory.GetFiles(directorio);
            bool esImagenV=false;
            string archivo=Application.StartupPath + "\\archivos\\folder.png";
            foreach (string fImagenes in imagenes)
            {
                esImagenV=esImagen(fImagenes);
                if (esImagenV==true)
                {
                    archivo = fImagenes;
                    break;
                }
            }
            Image imagenLocal = Image.FromFile(archivo);
            return imagenLocal;
        }
        private bool esImagen (string archivo){
            string extencion = System.IO.Path.GetExtension(archivo);
            bool esImagenFT = false;
            switch (extencion.ToLower())
            {
                case ".bmp":
                    esImagenFT = true;
                    break;
                case ".jpg":
                    esImagenFT = true;
                    break;
                case ".jpeg":
                    esImagenFT = true;
                    break;
                case ".gif":
                    esImagenFT = true;
                    break;
                case ".tiff":
                    esImagenFT = true;
                    break;
                case ".tif":
                    esImagenFT = true;
                    break;
                case ".png":
                    esImagenFT = true;
                    break;
            }
            return esImagenFT;
        }

        private string getDirectorynombre(string directorio)
        {
            string nombre="error";
            for(int i = (directorio.Length-2); i >= 0; i--)
            {
                nombre = directorio.Substring(i,1);
                if (nombre == "\\")
                {
                    int i1 = (directorio.Length - i)-1;
                    nombre = directorio.Substring((i+1),i1);
                    break;
                }

            }
            return nombre;
        }

        private BorderStyle estiloAnt;
        private void imageHover(object sender, System.EventArgs e)
        {
            PictureBox imagen = (PictureBox)sender;
            Panel padreImage=(Panel)imagen.Parent;
            estiloAnt = padreImage.BorderStyle;
            padreImage.BorderStyle = BorderStyle.FixedSingle;
        }
        private void imageLeave(object sender, System.EventArgs e)
        {
            PictureBox imagen = (PictureBox)sender;
            Panel padreImage = (Panel)imagen.Parent;
            padreImage.BorderStyle = estiloAnt;
        }

        private void directorioClick(object sender, System.EventArgs e)
        {
            PictureBox picture = (PictureBox)sender;
            string nuevaRuta = picture.Tag.ToString() + "\\" + nombreTextBox.Text + globales.extencionImagenSelect;
            if (System.IO.File.Exists(nuevaRuta))
            {
                MessageBox.Show("Ya existe una imagen con el mismo nombre en este directorio");
            }
            else
            {
                System.IO.File.Move(globales.direccionImagenSelect, nuevaRuta);
                navImagenesPanel.Controls.Remove(globales.panelImageSelect);
                if (navImagenesPanel.Controls.Count > 0)
                {
                    if (globales.indiceImageSelect < navImagenesPanel.Controls.Count - 1)
                    {
                        
                        PictureBox picture1 = (PictureBox)navImagenesPanel.Controls[globales.indiceImageSelect].Controls[0];
                        imagenClick(picture1, e);
                        estiloAnt = BorderStyle.None;
                    }
                    else
                    {
                        
                        PictureBox picture1 = (PictureBox)navImagenesPanel.Controls[globales.indiceImageSelect - 1].Controls[0];
                        imagenClick(picture1, e);
                        estiloAnt = BorderStyle.None;
                    }
                }
                else
                {
                    //Borar Datos de imagenes
                    navImagenesPanel.Controls.Add(clonarLabelVacio());
                }
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            //Cambia de navegador de directorios. locales visibles
            dLocales.Visible = true;
            dFavoritos.Visible = false;
            button8.ForeColor = Color.LimeGreen;
            button7.ForeColor = Color.Black;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Cambia de navegador de directorios. favoritos visibles
            dLocales.Visible = false;
            dFavoritos.Visible = true;
            button7.ForeColor = Color.LimeGreen;
            button8.ForeColor = Color.Black;
        }
        private void cargarImagenesLocales(string directorio)
        {
            //Carga las imagenes y las pone en el panel3
            string[] archivos = System.IO.Directory.GetFiles(directorio);
            navImagenesPanel.Controls.Clear();
            foreach (string archivo in archivos)
            {
                if(esImagen(archivo)==true)
                {
                    
                    Panel pImagen = new Panel();
                    PictureBox imagenI = new PictureBox();
                    pImagen.Parent = navImagenesPanel;
                    imagenI.Parent = pImagen;
                    pImagen.Dock = DockStyle.Left;
                    imagenI.Dock = DockStyle.Fill;
                    pImagen.Width = 100;
                    imagenI.ImageLocation = archivo;
                    //imagenI.Image = Image.FromFile(archivo);
                    imagenI.SizeMode = PictureBoxSizeMode.Zoom;
                    toolTip1.SetToolTip(imagenI, archivo);
                    imagenI.MouseEnter += new System.EventHandler(imageHover);
                    imagenI.MouseLeave += new System.EventHandler(imageLeave);
                    imagenI.Click += new System.EventHandler(imagenClick);
                }
            }
            if(navImagenesPanel.Controls.Count == 0)
            {
                navImagenesPanel.Controls.Add(clonarLabelVacio());
            }
        }

        private void imagenClick(object sender, EventArgs e)
        {
            //Marca el borde de la imagen al hacer click 
            PictureBox imgSelect = (PictureBox)sender;
            globales.panelImageSelect.BorderStyle = BorderStyle.None;
            globales.panelImageSelect = (Panel)imgSelect.Parent;
            globales.panelImageSelect.BorderStyle = BorderStyle.Fixed3D;
            estiloAnt = BorderStyle.Fixed3D;
            //Selecciona la imagen
            globales.pictureBoxSelect = imgSelect;
            visorPictureBox.ImageLocation = imgSelect.ImageLocation;
            globales.direccionImagenSelect = imgSelect.ImageLocation;
            visorPictureBox.Refresh();
            nombreTextBox.Text = System.IO.Path.GetFileNameWithoutExtension(globales.direccionImagenSelect);
            globales.extencionImagenSelect= System.IO.Path.GetExtension(globales.direccionImagenSelect);
            globales.indiceImageSelect = navImagenesPanel.Controls.IndexOf(globales.panelImageSelect);
            nombreTextBox.Focus();
            nombreTextBox.SelectAll();
        }

        private void nombreTextBox_Enter(object sender, EventArgs e)
        {
            nombreTextBox.SelectAll();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string nuevo = System.IO.Path.GetDirectoryName(globales.direccionImagenSelect) + "\\" + nombreTextBox.Text  + globales.extencionImagenSelect; 
            if (globales.direccionImagenSelect != nuevo)
            {
                if (System.IO.File.Exists(nuevo))
                {
                    MessageBox.Show("Una imagen con este nombre ya existe");
                    nombreTextBox.Focus();
                    nombreTextBox.SelectAll();
                }
                else
                {
                    System.IO.File.Move(globales.direccionImagenSelect, nuevo);
                    System.IO.File.Delete(globales.direccionImagenSelect);
                    globales.pictureBoxSelect.ImageLocation = nuevo;
                    visorPictureBox.ImageLocation = nuevo;
                    globales.direccionImagenSelect = nuevo;
                    visorPictureBox.Refresh();
                    nombreTextBox.Focus();
                    nombreTextBox.SelectAll();
                }
                
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Seguro que deseas eliminar esta imagen?", "Eliminar?", MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                System.IO.File.Delete(globales.direccionImagenSelect);
                navImagenesPanel.Controls.Remove(globales.panelImageSelect);
                if (navImagenesPanel.Controls.Count > 0)
                {
                    if (globales.indiceImageSelect < navImagenesPanel.Controls.Count - 1)
                    {
                        PictureBox picture = (PictureBox)navImagenesPanel.Controls[globales.indiceImageSelect].Controls[0];
                        imagenClick(picture, e);
                    }
                    else
                    {
                        PictureBox picture = (PictureBox)navImagenesPanel.Controls[globales.indiceImageSelect - 1].Controls[0];
                        imagenClick(picture, e);
                    }
                }
                else
                {
                    //Borar Datos de imagenes
                    navImagenesPanel.Controls.Add(clonarLabelVacio());
                }
            }
        }
        private void seleccionarDespues()
        {
            EventArgs e = new EventArgs();
            if (globales.indiceImageSelect >0)
            {
                PictureBox picture = (PictureBox)navImagenesPanel.Controls[globales.indiceImageSelect - 1].Controls[0];
                imagenClick(picture, e);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            navImagenesPanel.Controls.Add(clonarLabelVacio());
            dLocales.Controls.Add(clonarLabelVacio());
            dFavoritos.Controls.Add(clonarLabelVacio());
        }
        private Label clonarLabelVacio ()
        {
            Label labelVacio = new Label();
            labelVacio.Text = "(Vacio)";
            labelVacio.Dock = DockStyle.Fill;
            labelVacio.AutoSize = false;
            labelVacio.TextAlign = ContentAlignment.MiddleCenter;
            return labelVacio;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(globales.indiceImageSelect<navImagenesPanel.Controls.Count-1)
            {
                PictureBox picture = (PictureBox)navImagenesPanel.Controls[globales.indiceImageSelect + 1].Controls[0];
                imagenClick(picture, e);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            seleccionarDespues();
        }

        private void nombreTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            nombreTextBox.SelectAll();
        }

        private void nombreTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs e1 = new EventArgs();
            if (e.KeyCode == Keys.Enter)
            {
            button5_Click(sender = new object(), e1);
            }
            else if(e.KeyCode == Keys.F8)
            {
                seleccionarDespues();
            }else if (e.KeyCode == Keys.F7)
            {
                button3_Click(sender = new object(), e1);
            }
        }
    }
}
