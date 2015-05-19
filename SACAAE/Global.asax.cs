using SACAAE.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SACAAE
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private BackgroundWorker bw = new BackgroundWorker();
        private String CorreoAlertas = "Alertas-SACA@saca-ae.net";
        private String ContraseniaCorreo = "Administracion20142015.";
        private int PeriodoRevisionDias = 49;
        private int RevisionDiasAlerta = 100;
        SACAAEEntities entidades = new SACAAEEntities();
        
        protected void Application_Start()
        {
            /*HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
            */

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }
        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for(;;){
                    // Perform a time consuming operation and report progress.
                    RevisarProyectosYComisiones();
                    System.Threading.Thread.Sleep(86400*PeriodoRevisionDias);
                    
                
            }
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                Debug.WriteLine("Canceled!");
            }

            else if (!(e.Error == null))
            {
                Debug.WriteLine("Error: " + e.Error.Message);
            }

            else
            {
                Debug.WriteLine("Done!");
            }
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Debug.WriteLine(e.ProgressPercentage.ToString() + "%");
        }

        private void RevisarProyectosYComisiones()
        {
            String Titulo = "";
            String Cuerpo = "";
            Boolean hayAlertas = true;
            List<AlertaComisionProfesor> comisiones;
            List<AlertaProyectoProfesor> proyectos;
            IQueryable <Usuario> usuarios = from Aulas in entidades.Usuarios
                          select Aulas;

            
                if (AlertaProyectoProfesor.HayProyectosAtrasadas(RevisionDiasAlerta) && AlertaComisionProfesor.HayComisionesAtrasadas(RevisionDiasAlerta))
                {
                    Titulo = "SACA-AE Alertas:Comisiones y Proyectos están vencidos o pronto a vencer";
                    comisiones = AlertaComisionProfesor.getComisionesAtrasados(RevisionDiasAlerta);
                    proyectos = AlertaProyectoProfesor.getProyectosAtrasados(RevisionDiasAlerta);

                    Cuerpo += "\nLas siguientes comisiones están vencidas o pronto a vencer:\n";
                    foreach(var comision in comisiones)
                    {
                        Cuerpo += "\t-Profesor:" + comision.PROFESOR + "\n\t Comisión:" + comision.COMISION + "\n\t Inicio:" + comision.INCIO + "\n\t Fin:" + comision.FIN + "\n\n";
                    }

                    Cuerpo += "\nLos siguientes proyectos están vencidas o pronto a vencer:\n";
                    foreach(var proyecto in proyectos)
                    {
                        Cuerpo += "\t-Profesor:" + proyecto.PROFESOR + "\n\t Comisión:" + proyecto.PROYECTO + "\n\t Inicio:" + proyecto.INCIO + "\n\t Fin:" + proyecto.FIN + "\n\n";
                    }
                     
                }
                else if (AlertaProyectoProfesor.HayProyectosAtrasadas(RevisionDiasAlerta) && !AlertaComisionProfesor.HayComisionesAtrasadas(RevisionDiasAlerta))
                {
                    Titulo = "SACA-AE Alertas:Proyectos están vencidos o pronto a vencer";
                    proyectos = AlertaProyectoProfesor.getProyectosAtrasados(RevisionDiasAlerta);
                    Cuerpo += "\nLos siguientes proyectos están vencidas o pronto a vencer:\n";
                    foreach (var proyecto in proyectos)
                    {
                        Cuerpo += "\t-Profesor:" + proyecto.PROFESOR + "\n\t Comisión:" + proyecto.PROYECTO + "\n\t Inicio:" + proyecto.INCIO + "\n\t Fin:" + proyecto.FIN + "\n\n";
                    }
                }
                else if (!AlertaProyectoProfesor.HayProyectosAtrasadas(RevisionDiasAlerta) && AlertaComisionProfesor.HayComisionesAtrasadas(RevisionDiasAlerta))
                {
                    Titulo = "SACA-AE Alertas:Comisiones están vencidos o pronto a vencer";
                    comisiones = AlertaComisionProfesor.getComisionesAtrasados(RevisionDiasAlerta);

                    Cuerpo += "\nLas siguientes comisiones están vencidas o pronto a vencer:\n";
                    foreach (var comision in comisiones)
                    {
                        Cuerpo += "\t-Profesor:" + comision.PROFESOR + "\n\t Comisión:" + comision.COMISION + "\n\t Inicio:" + comision.INCIO + "\n\t Fin:" + comision.FIN + "\n\n";
                    }


                }
                else
                {
                    hayAlertas = false;
                    DoSomeFileWritingStuff("No hay alertas");
                    Debug.WriteLine("No existen comisiones o proyectos atrasados");
                }
                if (hayAlertas)
                {
                    foreach (var usuario in usuarios)
                    {

                        EnviarMail(usuario.Correo, Titulo, "Buen día " + usuario.Nombre+", el siguiente es un mensaje automático del sistema SACA-AE.\n"+Cuerpo
                            + "\n\n******Por favor no conteste este mensaje******");
                    }
                }
        
        }
        private void DoSomeFileWritingStuff(String mensaje)
        {
            Debug.WriteLine("Writing to file...");

            try
            {
                using (StreamWriter writer =
                 new StreamWriter(@"c:\temp\Cachecallback.txt", true))
                {
                    writer.WriteLine("{0}: " +DateTime.Now, mensaje);
                    writer.Close();
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine(x);
            }

            Debug.WriteLine("File write successful");
        }
        public void EnviarMail(String Correo, String Titulo, String Cuerpo)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.saca-ae.net");

                mail.From = new MailAddress(CorreoAlertas);
                mail.To.Add(Correo);
                mail.Subject = Titulo;
                mail.Body = Cuerpo;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(CorreoAlertas, ContraseniaCorreo);
                SmtpServer.EnableSsl = false;

                SmtpServer.Send(mail);
                DoSomeFileWritingStuff("Enviando Mail a"+Correo);
            }
            catch (Exception ex)
            {
                DoSomeFileWritingStuff(ex.ToString());
                Debug.WriteLine(ex.ToString());

            }
        }
    }

}