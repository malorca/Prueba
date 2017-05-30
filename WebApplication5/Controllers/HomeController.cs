using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        string cadena = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        SqlConnection db;
        SqlCommand cmd;

        public List<Servicios> ListaServicioG()
        {

            List<Servicios> lista = new List<Servicios>();
            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Lista", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Servicios ser = new Servicios();
                        ser.CodServ = dr["CodServ"].ToString();
                        ser.NomServ = dr["NomServ"].ToString();
                        ser.CodEspec = dr["CodEspec"].ToString();
                        ser.CodEmp = dr["CodEmp"].ToString();
                        ser.CodSede = dr["CodSede"].ToString();
                        ser.EstServ = bool.Parse(dr["EstServ"].ToString());
                        lista.Add(ser);
                    }
                }
            }
            return lista;
        }

        //LISTAR ESPECIALIDADES
        /*
          CodEspec
NomEspec
DescEspec
CodSede
CodTar
EstEspec
         */
        public List<Especialidades> ListarEspecialidades()
        {
            List<Especialidades> listaEspe = new List<Especialidades>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_especialidades", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    db.Open();
                    SqlDataReader sd = cmd.ExecuteReader();
                    while (sd.Read())
                    {
                        Especialidades esp = new Especialidades();
                        esp.CodEspec = sd["CodEspec"].ToString();
                        esp.NomEspec = sd["NomEspec"].ToString();
                        esp.DescEspec = sd["DescEspec"].ToString();
                        esp.CodSede = sd["CodSede"].ToString();
                        esp.CodTar = sd["CodTar"].ToString();
                        esp.EstEspec = bool.Parse(sd["EstEspec"].ToString());
                        listaEspe.Add(esp);

                    }


                }
                

            }

            return listaEspe;
        }




        //LISTAR EMPRESA TERCERO
        /*
         CodEmp
RazonEmp
RucEmp
DireccEmp
NomGeren
NomContacto
Tel1
Tel2
Correo1
Correo2
EstEmp
             */

        public List<Empresa_tercero> listarEmpresa_tercero()
        {
            List<Empresa_tercero> listaEmp = new List<Empresa_tercero>();

            using(db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_empresa_tercero", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    db.Open();
                    SqlDataReader sd = cmd.ExecuteReader();

                    while (sd.Read())
                    {
                        Empresa_tercero empre = new Empresa_tercero();
                        empre.CodEmp = sd["CodEmp"].ToString();
                        empre.RazonEmp = sd["RazonEmp"].ToString();
                        empre.RucEmp = sd["RucEmp"].ToString();
                        empre.DireccEmp = sd["DireccEmp"].ToString();
                        empre.NomGeren = sd["NomGeren"].ToString();
                        empre.NomContacto = sd["NomContacto"].ToString();
                        empre.Tel1 = sd["Tel1"].ToString();
                        empre.Tel2 = sd["Tel2"].ToString();
                        empre.Correo1 = sd["Correo1"].ToString();
                        empre.Correo2 = sd["Correo2"].ToString();
                        empre.EstEmp = bool.Parse(sd["EstEmp"].ToString());
                        listaEmp.Add(empre);

                    }





                }


            }

            return listaEmp;

        }
        public List<Sedes> ListarSedes()
         {
             List<Sedes> listaSedes = new List<Sedes>();

             using(db = new SqlConnection(cadena))
             {
                 using(cmd = new SqlCommand("Usp_sedes", db))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     db.Open();
                     SqlDataReader sd = cmd.ExecuteReader();

                     while (sd.Read())
                     {
                         Sedes sede = new Sedes();
                        sede.CodSede = sd["CodSede"].ToString();
                        sede.NomSede = sd["NomSede"].ToString();
                        sede.DireccSede = sd["DireccSede"].ToString();
                        sede.TelfSede = sd["TelfSede"].ToString();
                        sede.EstSede = bool.Parse(sd["EstSede"].ToString());
                        listaSedes.Add(sede);

                     }



                 }


             }

             return listaSedes;

         }


        //Listar por numero de documento y nombre de paciente

        public List<Pacientes> getpaciente(string numero,string nombre) {
            List<Pacientes> listaPaciente = new List<Pacientes>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_lista_num_nom", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumDoc", numero);
                    cmd.Parameters.AddWithValue("@NomPac", nombre);
                    db.Open();
                    SqlDataReader sd = cmd.ExecuteReader();

                    while (sd.Read())
                    {
                        Pacientes pac = new Pacientes();

                        pac.Historia = int.Parse(sd["Historia"].ToString());
                        pac.ApePat = sd["ApePat"].ToString();
                        pac.ApeMat = sd["ApeMat"].ToString();
                        pac.NomPac = sd["NomPac"].ToString();
                        pac.NumDoc = sd["NumDoc"].ToString();
                        listaPaciente.Add(pac);

                    }

                }

            }
            return listaPaciente;
        }



       public ActionResult ListarPaciente()
        {
                return View();

        }

        [HttpPost]
        public ActionResult ListarPaciente(string nombre,string numero)
        {
            ViewBag.paciente = getpaciente(nombre, numero);
            return View();

        }





        //LISTAR

        public ActionResult ListaServicio()
         {
             return View(ListaServicioG());
         }




         public ActionResult Edit(string id)
         {
             var edit = ListaServicioG().Where(x => x.CodServ == id).FirstOrDefault();


             return View(edit);
         }
         [HttpPost]
         public ActionResult Edit(Servicios ser)
         {

             try
             {
                 using (db = new SqlConnection(cadena))
                 {
                     using (cmd = new SqlCommand("Usp_mantenimiento_servicio", db))
                     {
                         cmd.CommandType = CommandType.StoredProcedure;
                         cmd.Parameters.AddWithValue("@CodServ", ser.CodServ);
                         cmd.Parameters.AddWithValue("@NomServ", ser.NomServ);
                         cmd.Parameters.AddWithValue("@CodEspec", ser.CodEspec);
                         cmd.Parameters.AddWithValue("@CodEmp", ser.CodEmp);
                         cmd.Parameters.AddWithValue("@CodSede", ser.CodSede);
                         cmd.Parameters.AddWithValue("@EstServ", 1);
                         cmd.Parameters.AddWithValue("@evento", "2");

                         db.Open();
                         int success = cmd.ExecuteNonQuery();
                         if (success == 1)
                         {
                             return RedirectToAction("ListaServicio");

                         }
                         else
                         {
                             return View(ser);
                         }

                     }

                 }
             }
             catch (Exception ex)
             {
                 return View(ser);
             }

         }


         public ActionResult Crear()
         {
             Servicios ser = new Servicios();
             ViewBag.especialidad = new SelectList(ListarEspecialidades(), "CodEspec", "NomEspec");
             ViewBag.empresa_tercero = new SelectList(listarEmpresa_tercero(), "CodEmp","RazonEmp");
             ViewBag.sedes = new SelectList(ListarSedes(),"CodSede","NomSede");

             return View(ser);
         }



         [HttpPost]
         public ActionResult Crear(Servicios ser)
         {

             try
             {
                 using (db = new SqlConnection(cadena))
                 {
                     using (cmd = new SqlCommand("Usp_mantenimiento_servicio", db))
                     {
                         cmd.CommandType = CommandType.StoredProcedure;
                         cmd.Parameters.AddWithValue("@CodServ", "");
                         cmd.Parameters.AddWithValue("@NomServ", ser.NomServ);
                         cmd.Parameters.AddWithValue("@CodEspec", ser.CodEspec);
                         cmd.Parameters.AddWithValue("@CodEmp", ser.CodEmp);
                         cmd.Parameters.AddWithValue("@CodSede", ser.CodSede);
                         cmd.Parameters.AddWithValue("@EstServ", ser.EstServ);
                         cmd.Parameters.AddWithValue("@evento", "1");

                         db.Open();
                         int success = cmd.ExecuteNonQuery();
                         if (success == 1)
                         {
                             return RedirectToAction("ListaServicio");

                         }
                         else
                         {
                             return View(ser);
                         }


                     }


                 }


             }
             catch (Exception ex)
             {
                 return View(ser);
             }



         }


        public ActionResult DeleteActivar(string id, string evento)
        {
            Servicios ser = new Servicios();

            try
            {

                using (db = new SqlConnection(cadena))
                {
                    using (cmd = new SqlCommand("Usp_mantenimiento_servicio", db))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodServ", id);
                        cmd.Parameters.AddWithValue("@NomServ", "");
                        cmd.Parameters.AddWithValue("@CodEspec", "");
                        cmd.Parameters.AddWithValue("@CodEmp", "");
                        cmd.Parameters.AddWithValue("@CodSede", "");
                        cmd.Parameters.AddWithValue("@EstServ", false);
                        if (evento == "1")
                        {
                            cmd.Parameters.AddWithValue("@evento", "3");
                        }
                        else if (evento == "2") {
                            cmd.Parameters.AddWithValue("@evento", "4");
                        }
                    
                        db.Open();
                        int success = cmd.ExecuteNonQuery();
                        if (success == 1)
                        {
                            return RedirectToAction("ListaServicio");

                        }
                        else
                        {
                            return View(ser);
                        }

                    }

                }



            }
            catch (Exception ex)
            {
                return View(ser);
            }


        }



        public ActionResult Detalle()
        {

            return View();

        }

        public JsonResult getEspecialidad() {
            var especialidad = ListarEspecialidades();
            return Json(especialidad, JsonRequestBehavior.AllowGet); 
           
        }

        public JsonResult getServicio(string codesp)
        {
            var especialidad = ListaServicioG().Where(x => x.CodEspec == codesp );
            return Json(especialidad, JsonRequestBehavior.AllowGet);

        }





        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}