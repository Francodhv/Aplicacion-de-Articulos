using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;


namespace negocio
{
    public class ArticuloNegocio
    {

        public List<Articulo> listaArticulos()
        {
            List<Articulo> articulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, ImagenUrl, Precio, A.IdMarca, A.IdCategoria, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M WHERE A.IdCategoria = C.Id AND A.IdMarca = M.Id");
                datos.ejectutarLectura();
                
                while (datos.Lector.Read())
                {
                    Articulo articulo = new Articulo();
                    articulo.Id = (int)datos.Lector["Id"];
                    articulo.Codigo = (string)datos.Lector["Codigo"];
                    articulo.Nombre = (string)datos.Lector["Nombre"];
                    articulo.Descripcion = (string)datos.Lector["Descripcion"];
                    articulo.DesMarca = new Marca();
                    articulo.DesMarca.Id = (int)datos.Lector["IdMarca"];
                    articulo.DesMarca.Descripcion = (string)datos.Lector["Marca"];
                    articulo.DesCategoria = new Categoria();
                   articulo.DesCategoria.Id = (int)datos.Lector["IdCategoria"];
                    articulo.DesCategoria.Descripcion = (string)datos.Lector["Categoria"];
                
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        articulo.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    }
                   
                    articulo.Precio = (decimal)datos.Lector["Precio"];

                    articulos.Add(articulo);

                }

                datos.cerrarConexion();
                return articulos;
            }
            catch (Exception ex)
            {

                throw ex;
            }

           
        }

        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)values(@cod, @nom, @Des, @IdMar, @IdCat, @img, @pre)");
                datos.setearParametro("@cod",nuevo.Codigo);
                datos.setearParametro("@nom", nuevo.Nombre);
                datos.setearParametro("@des", nuevo.Descripcion);
                datos.setearParametro("@IdMar", nuevo.DesMarca.Id);
                datos.setearParametro("@IdCat", nuevo.DesCategoria.Id);
                datos.setearParametro("@img", nuevo.UrlImagen);
                datos.setearParametro("@pre", nuevo.Precio);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Articulo modificado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @cod, Nombre = @nom, Descripcion = @des, IdMarca = @IdMar, IdCategoria = @IdCat, ImagenUrl = @img, Precio = @pre WHERE Id = @Id");
                datos.setearParametro("@cod",modificado.Codigo);
                datos.setearParametro("@nom", modificado.Nombre);
                datos.setearParametro("@des", modificado.Descripcion);
                datos.setearParametro("@IdMar", modificado.DesMarca.Id);
                datos.setearParametro("@IdCat", modificado.DesCategoria.Id);
                datos.setearParametro("@img", modificado.UrlImagen);
                datos.setearParametro("@pre", modificado.Precio);
                datos.setearParametro("@Id", modificado.Id);

                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarArticulo(int id)
        {
            AccesoDatos datos = new AccesoDatos();  
            try
            {
                datos.setearConsulta("update ARTICULOS set IdMarca = 0 WHERE Id = @id");
                datos.setearParametro("@id",id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtrarRango(string rango, string filtro, string criterio)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, ImagenUrl, Precio, A.IdMarca, A.IdCategoria, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M WHERE A.IdCategoria = C.Id AND A.IdMarca = M.Id AND ";
                if (criterio == "Precio")
                {
                    switch (rango)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Precio < " + filtro;
                            break;
                        default:
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                else if (criterio == "Nombre")
                {
                    switch (rango)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejectutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo articulo = new Articulo();
                    articulo.Id = (int)datos.Lector["Id"];
                    articulo.Codigo = (string)datos.Lector["Codigo"];
                    articulo.Nombre = (string)datos.Lector["Nombre"];
                    articulo.Descripcion = (string)datos.Lector["Descripcion"];
                    articulo.DesMarca = new Marca();
                    articulo.DesMarca.Id = (int)datos.Lector["IdMarca"];
                    articulo.DesMarca.Descripcion = (string)datos.Lector["Marca"];
                    articulo.DesCategoria = new Categoria();
                    articulo.DesCategoria.Id = (int)datos.Lector["IdCategoria"];
                    articulo.DesCategoria.Descripcion = (string)datos.Lector["Categoria"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        articulo.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    }

                    articulo.Precio = (decimal)datos.Lector["Precio"];

                    lista.Add(articulo);
                
                }
                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
