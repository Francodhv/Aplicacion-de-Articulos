using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;
using dominio;

namespace WindowsFormsApp_Final
{
    public partial class frmAltaArticulo : Form
    {

        private Articulo articulo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo articulo)
        {

            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio mn = new MarcaNegocio();
            CategoriaNegocio cn = new CategoriaNegocio();

            try
            {
                cbxMarca.DataSource = mn.listar();
                cbxMarca.ValueMember = "Id";
                cbxMarca.DisplayMember = "Descripcion";
                cbxCategoria.DataSource = cn.listar();
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    cbxMarca.SelectedValue = articulo.DesMarca.Id;
                    cbxCategoria.SelectedValue = articulo.DesCategoria.Id;
                    txtUrlImagen.Text = articulo.UrlImagen;
                    cargarImagen(txtUrlImagen.Text);
                    txtPrecio.Text = articulo.Precio.ToString();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if(articulo == null) 

                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.DesMarca = (Marca)cbxMarca.SelectedItem;
                articulo.DesCategoria = (Categoria)cbxCategoria.SelectedItem;
                articulo.UrlImagen = txtUrlImagen.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                
                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                    
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }

              

                Close();
            }
            catch (Exception )
            {

                MessageBox.Show("Debes completar todos los campos para continuar");
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pcbArticulo.Load(imagen);
            }
            catch (Exception )
            {
                pcbArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

      

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string codigo = txtCodigo.Text;
            string descripcion = txtDescripcion.Text;
            string marca = cbxMarca.Text;
            string categoria = cbxCategoria.Text;
            string url = txtUrlImagen.Text;
            string precio = txtPrecio.Text;
            try
            {
                if(txtCodigo.Text != "" && txtNombre.Text != "" && txtDescripcion.Text != "" && cbxMarca.Text != "" && cbxCategoria.Text != "" && txtPrecio.Text != "") { 
                txtDatos.Text = "Nombre: " + nombre + Environment.NewLine + "Codigo: " + codigo + Environment.NewLine +
               "Descripcion: " + descripcion + Environment.NewLine + "Marca: " + marca + Environment.NewLine +
               "Categoría: " + categoria + Environment.NewLine + "Url Imagen: " + url + Environment.NewLine + "Precio: " + precio;

                }
                else
                {
                    MessageBox.Show("Debe completar todos los campos para acceder al Detalle del articulo");
                }
            }
            catch (Exception ex )
            {

                MessageBox.Show(ex.ToString());
            }
           
        }
    }
}
