//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SACAAE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Profesore
    {
        public Profesore()
        {
            this.ComisionesXProfesors = new HashSet<ComisionesXProfesor>();
            this.PlazaXProfesors = new HashSet<PlazaXProfesor>();
            this.ProfesoresXCursoes = new HashSet<ProfesoresXCurso>();
            this.ProyectosXProfesors = new HashSet<ProyectosXProfesor>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Link { get; set; }
        public int Estado { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
    
        public virtual ICollection<ComisionesXProfesor> ComisionesXProfesors { get; set; }
        public virtual Estado Estado1 { get; set; }
        public virtual ICollection<PlazaXProfesor> PlazaXProfesors { get; set; }
        public virtual ICollection<ProfesoresXCurso> ProfesoresXCursoes { get; set; }
        public virtual ICollection<ProyectosXProfesor> ProyectosXProfesors { get; set; }
    }
}
