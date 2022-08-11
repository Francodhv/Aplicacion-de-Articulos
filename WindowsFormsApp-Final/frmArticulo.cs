using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace WindowsFormsApp_Final
{
    public partial class frmArticulo : Form
    {
       
        private List<Articulo> articulos = new List<Articulo>();
        public frmArticulo()
        {
            InitializeComponent();
        }

       


        private void frmArticulo_Load(object sender, EventArgs e)
        {
            try
            {
                cargar();
                cmbColumna.Items.Add("Precio");
                cmbColumna.Items.Add("Nombre");

            }
            catch (Exception)
            {

                throw;
            }
          
            

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

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }
           
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                frmAltaArticulo agregar = new frmAltaArticulo();
                agregar.ShowDialog();
                cargar();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                articulos = negocio.listaArticulos();
                dgvArticulos.DataSource = articulos;
                ocultarColumnas();
                cargarImagen(articulos[0].UrlImagen);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["UrlImagen"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
                modificar.ShowDialog();
                cargar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private bool eliminar(bool eliminado = false)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad querés eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminarArticulo(seleccionado.Id);
                    cargar();
                    return  eliminado = true;
                }
                cargar();

                return eliminado;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        private void txtFiltroNombre_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> articulosFiltrados;
            string filtro = txtFiltroNombre.Text;

            if(filtro.Length >=2)
            {
                articulosFiltrados = articulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Codigo.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                articulosFiltrados = articulos;
            }

            dgvArticulos.DataSource = null;//Limpio  el dgv para darle una nueva lista
            dgvArticulos.DataSource = articulosFiltrados;
            ocultarColumnas();
            
        }

       private void txtFiltroDescripcion_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> articulosFiltrados;
            string filtro = txtFiltroDescripcion.Text;

            if (filtro.Length >= 2)
            {
                articulosFiltrados = articulos.FindAll(x => x.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                articulosFiltrados = articulos;
            }

            dgvArticulos.DataSource = null;//Limpio  el dgv para darle una nueva lista
            dgvArticulos.DataSource = articulosFiltrados;
            ocultarColumnas();
        }

        private void cmbColumna_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cmbColumna.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cmbRango.Items.Clear();
                cmbRango.Items.Add("Mayor a");
                cmbRango.Items.Add("Menor a");
                cmbRango.Items.Add("Igual a");
            }
            else if(opcion == "Nombre")
            {
                cmbRango.Items.Clear();
                cmbRango.Items.Add("Comienza con");
                cmbRango.Items.Add("Termina con");
                cmbRango.Items.Add("Contiene");
            }
        }

        private bool validarFiltro()
        {

            if (cmbColumna.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione la columna");
                return true;
            }
            if (cmbRango.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el rango");
                return true;
            }


            if (cmbColumna.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtValor.Text))
                {
                    MessageBox.Show("Debes cargar el valor");
                    return true;
                }
                if (!(soloNumeros(txtValor.Text)))
                {
                    MessageBox.Show("Complete el campo unicamente con valores numericos");
                    return true;
                }
            }
            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char item in cadena)
            {
                if (!(char.IsNumber(item)))
                {
                    return false;
                }
            }
            return true;

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;
                string rango = cmbRango.SelectedItem.ToString();
                string columna = cmbColumna.SelectedItem.ToString();
                string filtro = txtValor.Text;
               
                dgvArticulos.DataSource = negocio.filtrarRango(rango,filtro,columna);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
    
}
