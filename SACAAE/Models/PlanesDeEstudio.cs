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
    
    public partial class PlanesDeEstudio
    {
        public PlanesDeEstudio()
        {
            this.BloqueAcademicoXPlanDeEstudios = new HashSet<BloqueAcademicoXPlanDeEstudio>();
            this.PlanesDeEstudioXSedes = new HashSet<PlanesDeEstudioXSede>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Modalidad { get; set; }
        public Nullable<int> FK_TipoEntidad { get; set; }
    
        public virtual ICollection<BloqueAcademicoXPlanDeEstudio> BloqueAcademicoXPlanDeEstudios { get; set; }
        public virtual Modalidade Modalidade { get; set; }
        public virtual ICollection<PlanesDeEstudioXSede> PlanesDeEstudioXSedes { get; set; }
        public virtual TipoEntidad TipoEntidad { get; set; }
    }
}
